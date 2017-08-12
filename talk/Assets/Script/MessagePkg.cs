using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;


public class RespChatMsg : MessagePkg
{
    public static ushort msgId = 7002;
    public ushort GetMsgId() { return msgId; }
    public Int64 senderID = 0;
    public String senderName = "";
    public Int64 senderTeamId = 0;
    public Int64 senderGuildId = 0;
    public byte senderGuildPos = 0;
    public Int32 senderVIP = 0;
    public Int32 senderLvl = 0;
    public byte senderIcon = 0;
    public Int32 senderFlag = 0;
    public byte senderCityPos = 0;
    public Int32 senderVflag = 0;
    public byte senderIsGM = 0;
    public byte senderCamp = 0;
    public Int64 sendTime = 0;
    public Int32 hornId = 0;
    public Int32 channel = 0;
    public String text = "";
    public void Encode(ByteBuffer ouput)
    {
        //output.WriteLong(this.senderID);
        //output.WriteBytes(Encoding.UTF8.GetBytes(this.senderName), 32);
        //output.WriteLong(this.senderTeamId);
        //output.WriteLong(this.senderGuildId);
        //output.WriteByte(this.senderGuildPos);
        //output.WriteInt(this.senderVIP);
        //output.WriteInt(this.senderLvl);
        //output.WriteByte(this.senderIcon);
        //output.WriteInt(this.senderFlag);
        //output.WriteByte(this.senderCityPos);
        //output.WriteInt(this.senderVflag);
        //output.WriteByte(this.senderIsGM);
        //output.WriteByte(this.senderCamp);
        //output.WriteLong(this.sendTime);
        //output.WriteInt(this.hornId);
        //output.WriteInt(this.channel);
        //output.WriteString(this.text);
    }
    public void Decode(ByteBuffer input)
    {
        this.senderID = input.ReadLong();
        this.senderName = input.ReadFixString(32);
        this.senderTeamId = input.ReadLong();
        this.senderGuildId = input.ReadLong();
        this.senderGuildPos = input.ReadByte();
        this.senderVIP = input.ReadInt();
        this.senderLvl = input.ReadInt();
        this.senderIcon = input.ReadByte();
        this.senderFlag = input.ReadInt();
        this.senderCityPos = input.ReadByte();
        this.senderVflag = input.ReadInt();
        this.senderIsGM = input.ReadByte();
        this.senderCamp = input.ReadByte();
        this.sendTime = input.ReadLong();
        this.hornId = input.ReadInt();
        this.channel = input.ReadInt();
        this.text = input.ReadString();
    }
}