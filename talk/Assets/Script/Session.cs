using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Runtime.InteropServices;

#if TESTMODE
using Unity.IO.Compression;
#else
using System.IO.Compression;
#endif


public class ByteBuffer
{
    MemoryStream stream = null;
    BinaryWriter writer = null;
    BinaryReader reader = null;

    public ByteBuffer()
    {
        stream = new MemoryStream();
        writer = new BinaryWriter(stream);
        reader = new BinaryReader(stream);
    }

    public void Close()
    {
        if (writer != null) writer.Close();
        if (reader != null) reader.Close();

        stream.Close();
        writer = null;
        reader = null;
        stream = null;
    }
    public void Clear()
    {
        stream.SetLength(0);
    }

    public long GetPosition()
    {
        return stream.Position;
    }

    public void SetPosition(long pos)
    {
        stream.Seek(pos, SeekOrigin.Begin);
    }

    public long GetLength()
    {
        return stream.Length;
    }

    public void WriteBoolean(bool v)
    {
        writer.Write(v);
    }

    public void WriteByte(byte v)
    {
        writer.Write(v);
    }

    public void WriteInt(int v)
    {
        writer.Write((int)v);
    }

    public void WriteUInt(uint v)
    {
        writer.Write((uint)v);
    }

    public void WriteShort(ushort v)
    {
        writer.Write((ushort)v);
    }

    public void WriteLong(long v)
    {
        writer.Write((long)v);
    }

    public void WriteFloat(float v)
    {
        //byte[] temp = BitConverter.GetBytes(v);
        //Array.Reverse(temp);
        //writer.Write(BitConverter.ToSingle(temp, 0));
        writer.Write(v);
    }

    public void WriteDouble(double v)
    {
        //byte[] temp = BitConverter.GetBytes(v);
        //Array.Reverse(temp);
        //writer.Write(BitConverter.ToDouble(temp, 0));
        writer.Write(v);
    }

    public void WriteString(string v)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(v);
        writer.Write((int)bytes.Length);
        writer.Write(bytes);
    }

    public void WriteBytes(byte[] v)
    {
        writer.Write(v);
    }

    public void WriteBytes(byte[] v, int length)
    {
        MemoryStream stream_ = new MemoryStream();
        BinaryWriter writer_ = new BinaryWriter(stream_);
        int sub = length - v.Length;
        if (sub <= 0)
        {
            writer_.Write(v, 0, length);
        }
        else
        {
            writer_.Write(v);
            for (; sub > 0;)
            {
                writer_.Write((byte)0);
                sub--;
            }
        }
        writer_.Flush();
        byte[] tmp = stream_.ToArray();
        writer.Write(tmp);
        writer_.Close();
        stream_.Close();
        writer_ = null;
        stream_ = null;
    }

    public bool ReadBoolean()
    {
        return reader.ReadBoolean();
    }

    public byte ReadByte()
    {
        return reader.ReadByte();
    }

    public byte[] ReadBytes(int size)
    {
        return reader.ReadBytes(size);
    }

    public int ReadInt()
    {
        return (int)reader.ReadInt32();
    }

    public uint ReadUInt()
    {
        return (uint)reader.ReadUInt32();
    }

    public ushort ReadShort()
    {
        return (ushort)reader.ReadInt16();
    }

    public long ReadLong()
    {
        return (long)reader.ReadInt64();
    }

    public float ReadFloat()
    {
        //byte[] temp = BitConverter.GetBytes(reader.ReadSingle());
        //Array.Reverse(temp);
        //return BitConverter.ToSingle(temp, 0);
        return reader.ReadSingle();
    }

    public double ReadDouble()
    {
        //byte[] temp = BitConverter.GetBytes(reader.ReadDouble());
        //Array.Reverse(temp);
        //return BitConverter.ToDouble(temp, 0);
        return reader.ReadDouble();
    }

    public String ReadString()
    {
        int len = ReadInt();
        byte[] buffer = new byte[len];
        buffer = reader.ReadBytes(len);
        return Encoding.UTF8.GetString(buffer);
    }

    public String ReadFixString(int len)
    {

        byte[] buffer = new byte[len];
        buffer = reader.ReadBytes(len);
        return Encoding.UTF8.GetString(buffer).TrimEnd('\0');
    }

    public byte[] ToBytes()
    {
        writer.Flush();
        return stream.ToArray();
    }

    public void Flush()
    {
        writer.Flush();
    }
}

public interface MessagePkg
{
    void Encode(ByteBuffer output);
    void Decode(ByteBuffer input);
    ushort GetMsgId();
}

public class Connection
{
    public static int connectTimerId = -1;
    private int baseSeconds = 500;
    private float maxTime = 1;    //分钟
    private int totalTime = 0;
    private int timerId = -1;
    public Connection() { }
    /// <summary>
    /// 注册代理
    /// </summary>
    public void OnRegister()
    {
        memStream = new MemoryStream();
        reader = new BinaryReader(memStream);
    }

    /// <summary>
    /// 移除代理
    /// </summary>
    public void OnRemove()
    {
        this.Close();
        reader.Close();
        memStream.Close();
    }

    
    public void ConnectServer(String ip, int port, Func<int> onConn = null)
    {
        this.ip = ip;
        this.port = port;
        this.onConnected = onConn;
        client = new TcpClient();
        client.SendTimeout = 1000;
        client.ReceiveTimeout = 1000;
        client.NoDelay = true;
        try
        {
            client.BeginConnect(ip, port, new AsyncCallback(OnConnect), null);
        }
        catch (Exception e)
        {
            Close();
            Debug.LogError(e.Message);

        }

    }

    private NetworkStream outStream = null;
    private const int MAX_READ = 8192;
    private byte[] byteBuffer = new byte[MAX_READ];

    void OnConnect(IAsyncResult asr)
    {
        outStream = client.GetStream();
        if (this.onConnected != null)
        {
            this.onConnected();
        }
      
    }

   



    void OnReceive(byte[] bytes, int length)
    {
        memStream.Seek(0, SeekOrigin.End);
        memStream.Write(bytes, 0, length);
        //Reset to beginning
        memStream.Seek(0, SeekOrigin.Begin);


        //Create a new stream with any leftover bytes
        byte[] leftover = reader.ReadBytes((int)RemainingBytes());
        memStream.SetLength(0);     //Clear
        memStream.Write(leftover, 0, leftover.Length);
    }

    private long RemainingBytes()
    {
        return memStream.Length - memStream.Position;
    }

   

    public void Close()
    {
        if (client != null)
        {
            if (client.Connected) client.Close();
            client = null;
        }
    }

    public byte[] compress(byte[] input)
    {
        int[] len = new int[1];
        len[0] = 1024;
        byte[] output = new byte[10240];
        byte[] ret;
        try
        {

            deflating(output, len, input, input.Length, 0);
        }
        catch (Exception e)
        {
            Debug.LogError("compress error" + e);
        }
        ret = new byte[len[0]];
        Array.Copy(output, ret, len[0]);
        return ret;
    }

    public byte[] decompress(byte[] input, int size)
    {
        int[] len = new int[1];
        len[0] = size;
        byte[] output = new byte[size];
        byte[] ret;
        int value = 0;
        try
        {

            value = inflating(output, len, input, input.Length, 0);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        ret = new byte[len[0]];
        Array.Copy(output, ret, len[0]);
        return ret;
    }

    public static byte[] Compress(byte[] inputBytes)
    {
        using (MemoryStream outStream = new MemoryStream())
        {
            using (GZipStream zipStream = new GZipStream(outStream, CompressionMode.Compress))
            {
                zipStream.Write(inputBytes, 0, inputBytes.Length);
                zipStream.Flush();
                zipStream.Dispose();
                return outStream.ToArray();
            }
        }
    }

    private static byte[] readBuffer = new byte[1024];
    public static byte[] Decompress(byte[] inputBytes)
    {

        using (MemoryStream inputStream = new MemoryStream(inputBytes))
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                using (GZipStream zipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                {

                    int len;
                    while ((len = zipStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                    {
                        outStream.Write(readBuffer, 0, len);
                    }
                    zipStream.Flush();
                    zipStream.Dispose();
                    return outStream.ToArray();
                }
            }

        }
    }

    private int bodySize = 0;
    private int msgId = 0;
    private int compressFlag = 0;
  

   
    public int Write(MessagePkg msg)
    {
        int ret = 0;
        output.Clear();
        msg.Encode(output);
        rawBytes.Clear();
        tmpRawBytes.Clear();
        tmpData.Clear();

        rawBytes.WriteShort(magic);

        tmpData.WriteShort(msg.GetMsgId());
        tmpData.WriteShort((ushort)netIdx);
        byte[] tmpBytes = output.ToBytes();
        tmpData.WriteBytes(tmpBytes);
        long dataSize = output.ToBytes().Length;

        System.Random rand = new System.Random();
        byte result = (byte)rand.Next(255);
        int checkSum = CheckSum(result, tmpData.ToBytes(), (int)tmpData.GetPosition());

        if (output.GetPosition() > 0)
        {
            if (dataSize > COMPRESS_THRESHOLD)
            {
                byte[] data = null;
              
                int len = data.Length;
                rawBytes.WriteInt(len);
                rawBytes.WriteByte(result);
                rawBytes.WriteByte((byte)checkSum);
                rawBytes.WriteInt((int)dataSize);

                tmpData.Clear();
                tmpData.WriteShort(msg.GetMsgId());
                tmpData.WriteShort((ushort)netIdx);
                rawBytes.WriteBytes(EncryptPak(tmpData.ToBytes(), (int)tmpData.GetPosition()));
                rawBytes.WriteBytes(EncryptPak(data, data.Length));
            }
            else
            {
                rawBytes.WriteInt(output.ToBytes().Length);
                rawBytes.WriteByte(result);
                rawBytes.WriteByte((byte)checkSum);
                rawBytes.WriteInt(0);
                tmpBytes = EncryptPak(tmpData.ToBytes(), tmpData.ToBytes().Length);
                rawBytes.WriteBytes(tmpBytes);
            }
        }
        else
        {
            rawBytes.WriteInt(0);
            rawBytes.WriteByte(result);
            rawBytes.WriteByte((byte)checkSum);
            rawBytes.WriteInt(0);
            rawBytes.WriteBytes(EncryptPak(tmpData.ToBytes(), tmpData.ToBytes().Length));
        }

        WriteMessage(rawBytes.ToBytes());
        netIdx = netIdx + 1;
        if (netIdx > 9999)
        {
            netIdx = 1;
        }
        return ret;
    }

    public int Write(byte[] msg, ushort msgId)
    {
        int ret = 0;
        rawBytes.Clear();
        tmpRawBytes.Clear();
        tmpData.Clear();

        rawBytes.WriteShort(magic);

        tmpData.WriteShort(msgId);
        tmpData.WriteShort((ushort)netIdx);
        byte[] tmpBytes = msg;
        tmpData.WriteBytes(tmpBytes);
        long dataSize = msg.Length;

        System.Random rand = new System.Random();
        byte result = (byte)rand.Next(255);
        int checkSum = CheckSum(result, tmpData.ToBytes(), (int)tmpData.GetPosition());

        if (dataSize > 0)
        {
            if (dataSize > COMPRESS_THRESHOLD)
            {
                byte[] data = null;
             
                int len = data.Length;
                rawBytes.WriteInt(len);
                rawBytes.WriteByte(result);
                rawBytes.WriteByte((byte)checkSum);
                rawBytes.WriteInt((int)dataSize);

                tmpData.Clear();
                tmpData.WriteShort(msgId);
                tmpData.WriteShort((ushort)netIdx);
                rawBytes.WriteBytes(EncryptPak(tmpData.ToBytes(), (int)tmpData.GetPosition()));
                rawBytes.WriteBytes(EncryptPak(data, data.Length));
            }
            else
            {
                rawBytes.WriteInt(msg.Length);
                rawBytes.WriteByte(result);
                rawBytes.WriteByte((byte)checkSum);
                rawBytes.WriteInt(0);
                tmpBytes = EncryptPak(tmpData.ToBytes(), tmpData.ToBytes().Length);
                rawBytes.WriteBytes(tmpBytes);
            }
        }
        else
        {
            rawBytes.WriteInt(0);
            rawBytes.WriteByte(result);
            rawBytes.WriteByte((byte)checkSum);
            rawBytes.WriteInt(0);
            rawBytes.WriteBytes(EncryptPak(tmpData.ToBytes(), tmpData.ToBytes().Length));
        }

        WriteMessage(rawBytes.ToBytes());
        netIdx = netIdx + 1;
        if (netIdx > 9999)
        {
            netIdx = 1;
        }
        return ret;
    }

    public int WriteLuaMessage(byte[] msg, ushort msgId)
    {
        int ret = 0;
        rawBytes.Clear();
        tmpRawBytes.Clear();
        tmpData.Clear();

        rawBytes.WriteShort(magic);

        tmpData.WriteShort(msgId);
        tmpData.WriteShort((ushort)netIdx);
        byte[] tmpBytes = msg;
        tmpData.WriteBytes(tmpBytes);
        long dataSize = msg.Length;

        System.Random rand = new System.Random();
        byte result = (byte)rand.Next(255);
        int checkSum = CheckSum(result, tmpData.ToBytes(), (int)tmpData.GetPosition());

        if (dataSize > 0)
        {
            if (dataSize > COMPRESS_THRESHOLD)
            {
                byte[] data = null;
               
                int len = data.Length;
                rawBytes.WriteInt(len);
                rawBytes.WriteByte(result);
                rawBytes.WriteByte((byte)checkSum);
                rawBytes.WriteInt((int)dataSize);

                tmpData.Clear();
                tmpData.WriteShort(msgId);
                tmpData.WriteShort((ushort)netIdx);
                rawBytes.WriteBytes(EncryptPak(tmpData.ToBytes(), (int)tmpData.GetPosition()));
                rawBytes.WriteBytes(EncryptPak(data, data.Length));
            }
            else
            {
                rawBytes.WriteInt(msg.Length);
                rawBytes.WriteByte(result);
                rawBytes.WriteByte((byte)checkSum);
                rawBytes.WriteInt(0);
                tmpBytes = EncryptPak(tmpData.ToBytes(), tmpData.ToBytes().Length);
                rawBytes.WriteBytes(tmpBytes);
            }
        }
        else
        {
            rawBytes.WriteInt(0);
            rawBytes.WriteByte(result);
            rawBytes.WriteByte((byte)checkSum);
            rawBytes.WriteInt(0);
            rawBytes.WriteBytes(EncryptPak(tmpData.ToBytes(), tmpData.ToBytes().Length));
        }

        WriteMessage(rawBytes.ToBytes());
        netIdx = netIdx + 1;
        if (netIdx > 9999)
        {
            netIdx = 1;
        }
        return ret;
    }

    public void WriteMessage(byte[] message)
    {
        MemoryStream ms = null;
        using (ms = new MemoryStream())
        {
            ms.Position = 0;
            BinaryWriter writer = new BinaryWriter(ms);
            writer.Write(message);
            writer.Flush();
            if (client != null && client.Connected)
            {
                byte[] payload = ms.ToArray();
                outStream.BeginWrite(payload, 0, payload.Length, new AsyncCallback(OnWrite), null);
            }
            else
            {
             
            }
        }
    }

  

    void OnWrite(IAsyncResult r)
    {
        try
        {
            outStream.EndWrite(r);
        }
        catch (Exception ex)
        {
            Debug.LogError("OnWrite--->>>" + ex.Message);
        }
    }

    private int CheckSum(int seed, byte[] data, int len)
    {
        int ret = 0;
        int i = 0;
        while (i < len)
        {
            int o = data[i];
            int pos = ((seed + o + len) % encrypt_key_len);
            ret = ret ^ encrypt_key[pos];
            i = i + 1;
        }

        return ret;
    }

    public byte[] EncryptPak(byte[] data, int len)
    {
        ByteBuffer bytes = new ByteBuffer();
        for (int i = 0; i < len; i++)
        {
            int o = data[len - i - 1];
            o = o ^ encrypt_key[(i + len) % encrypt_key_len];
            bytes.WriteByte((byte)o);
        }
        return bytes.ToBytes();
    }

    public byte[] DecryptPak(byte[] data, int len)
    {
        ByteBuffer bytes = new ByteBuffer();
        for (int i = 0; i < len; i++)
        {
            int o = data[len - i - 1];
            o = o ^ encrypt_key[(len - i - 1 + len) % encrypt_key_len];
            bytes.WriteByte((byte)o);
        }

        return bytes.ToBytes();
    }

    public void TestCrypt()
    {
        byte[] input = { 19, 2, 30, 4, 5 };
        byte[] tmp = EncryptPak(input, 5);
        System.Console.WriteLine(tmp.ToString());
        tmp = DecryptPak(tmp, 5);


    }

    private String ip;
    private int port;
    private TcpClient client = null;

    private Func<int> onConnected;
    private static int FIRST = 0;
    private static int HEADERS = 1;
    private static int BODYS = 2;
    private int parserState = FIRST;

    private ByteBuffer output = new ByteBuffer();
    private ByteBuffer rawBytes = new ByteBuffer();
    private ByteBuffer tmpRawBytes = new ByteBuffer();
    private ByteBuffer tmpData = new ByteBuffer();

    private MemoryStream memStream;
    private BinaryReader reader;


    private byte[] encrypt_key = {
    0x77, 0xee, 0x99, 0x07, 0x02,
    0x70, 0xe9, 0x9e, 0x0e, 0x79,
    0xe0, 0x97, 0x09, 0x7e, 0xe7,
    0x90, 0x1d, 0x6a, 0xf3, 0x84,
    0x1a, 0x6d, 0xf4, 0x84, 0x13,
    0x64, 0xfd, 0x8a, 0x14, 0x63};
    private int encrypt_key_len = 30;
    private int COMPRESS_THRESHOLD = 128;


    public static ushort magic = 0x52FA;
    private int netIdx = 1;
#if !UNITY_EDITOR && UNITY_IOS
    // On iOS plugins are statically linked into
    // the executable, so we have to use __Internal as the
    // library name.
    [DllImport ("__Internal")]
    extern static int deflating(byte[] dest, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] destLen, byte[] source, int sourceLen, int raw = 0);
#else
    // Other platforms load plugins dynamically, so pass the name
    // of the plugin's dynamic library.
    [DllImport("zlib", EntryPoint = "deflating")]
    extern static int deflating(byte[] dest, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] destLen, byte[] source, int sourceLen, int raw = 0);
#endif

#if !UNITY_EDITOR && UNITY_IOS
    [DllImport ("__Internal")]
    extern static int inflating(byte[] dest, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] destLen, byte[] source, int sourceLen, int raw = 0);
#else
    [DllImport("zlib", EntryPoint = "inflating")]
    extern static int inflating(byte[] dest, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] destLen, byte[] source, int sourceLen, int raw = 0);
#endif
};


