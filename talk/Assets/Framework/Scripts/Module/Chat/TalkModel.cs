using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*定义数据
 * 接收数据存储
 * 发送数据存储
 * */
/// <summary>
/// 定义频道集
/// </summary>
public enum ChannelType
{
    complete = 0,   //全部（综合）
    sys = 1,   //系统
    world = 2,   //世界
    area = 3,   //场景（附近）
    camp = 4,   //阵营
    gang = 5,   //帮派
    team = 6,   //队伍
    horn = 7,    //喇叭
    privat = 8,    //私聊
    busscreen = 9 //公屏
}
/// <summary>
/// 定义单条消息数据
/// </summary>
public class ChatInfoData
{
    public int channelId { set; get; }
    public int level { set; get; }
    public byte icon { set; get; }
    public long sendId { set; get; }
    public long teamId { set; get; }
    public string name { set; get; }
    public string time { set; get; }
    public string text { set; get; }
}

public class TalkModel
{

    private static TalkModel _instance;
    public static TalkModel Instance
    {
        get
        {
            if (_instance == null) _instance = new TalkModel();
            return _instance;
        }
    }

    private Dictionary<int, int> MaxInfoNum = new Dictionary<int, int>();
    private Dictionary<ChannelType, List<ChatInfoData>> ChannelData = new Dictionary<ChannelType, List<ChatInfoData>>();
    private List<ChatInfoData> ListChatInfo = new List<ChatInfoData>();

    public void Init()
    {
        AddInitHandle();
        //Debug.LogError("talkconatroller");
        ///<summary>
        ///监听消息7002
        ///</summary>
        //EventDispatcher.addEventHandler(RespChatMsg.msgId, OnTalkMsg);

    }
    //频道对应面板id
    private Dictionary<int, int> ChannelPanelId = new Dictionary<int, int>();


    /// <summary>
    /// 接收频道消息
    /// </summary>
    /// <param name="output"></param>
    private void OnTalkMsg(ByteBuffer output)
    {
        RespChatMsg msg = new RespChatMsg();
        msg.Decode(output);
        ChatInfoData DataInfo = new ChatInfoData();
        DataInfo.channelId = msg.channel;
        DataInfo.level = msg.senderLvl;
        DataInfo.icon = msg.senderIcon;
        DataInfo.sendId = msg.senderID;
        DataInfo.teamId = msg.sendTime;
        DataInfo.name = msg.senderName;
        DataInfo.time = GetDataTime();
        DataInfo.text = msg.text;
        ChatRecvMessage(DataInfo);
    }
    /// <summary>
    /// 接收公告信息
    /// </summary>
    private object[] value;
    public void RecvCompleteMsg(object value = null)
    {
        ChatInfoData DataInfo = new ChatInfoData();
        //Debug.LogError("4号位公告消息" + value.ToString());
        DataInfo.channelId = 2;
        DataInfo.time = GetDataTime();
        DataInfo.text = value.ToString();
        ChatRecvMessage(DataInfo);
    }

    /// <summary>
    /// 根据显示面板信息项设置消息存储最大值
    /// </summary>
    private void AddInitHandle()
    {
        //设定存储消息条数
        MaxInfoNum.Add(0, 100);
        MaxInfoNum.Add(1, 100);
        MaxInfoNum.Add(2, 100);
        MaxInfoNum.Add(3, 100);
        MaxInfoNum.Add(4, 100);
        MaxInfoNum.Add(5, 100);
        MaxInfoNum.Add(6, 100);
        MaxInfoNum.Add(7, 100);
        ChannelPanelId.Add(0, 0);//"综合(公告)"->
        ChannelPanelId.Add(1, 2);//"世界"
        ChannelPanelId.Add(2, 3);//"场景"
        ChannelPanelId.Add(3, 5);//"帮派"
        ChannelPanelId.Add(4, 6);//"队伍"
        ChannelPanelId.Add(5, 1);//"系统"
        ChannelPanelId.Add(6, 8);//"私聊"
        ChannelPanelId.Add(7, 9);//"公屏"->
    }

    /// <summary>
    /// 存储聊天消息
    /// 按显示条件存储对应id消息数据
    /// </summary>
    private void ChatRecvMessage(ChatInfoData DataInfo)
    {
        //Debug.LogError("消息来自：" + DataInfo.channelId);
        //公屏
        SetInfoData(ChannelType.busscreen, DataInfo);
        //综合
        SetInfoData(ChannelType.complete, DataInfo);
        switch (DataInfo.channelId)
        {
            case 0:
                break;
            case 1:
                SetInfoData(ChannelType.sys, DataInfo);
                break;
            case 2:
                SetInfoData(ChannelType.world, DataInfo);
                break;
            case 3:
                SetInfoData(ChannelType.area, DataInfo);
                break;
            case 5:
                SetInfoData(ChannelType.gang, DataInfo);
                break;
            case 6:
                SetInfoData(ChannelType.team, DataInfo);
                break;
            case 8:
                SetInfoData(ChannelType.privat, DataInfo);
                break;
            default:
                break;
        }
    }

    private void SetInfoData(ChannelType _key, ChatInfoData _value)
    {
        if (ChannelData.ContainsKey(_key))
        {
            int pannelId = GetPid((int)_key);
            if (ChannelData[_key].Count >= MaxInfoNum[pannelId])
            {
                ChannelData[_key].RemoveAt(0);
                object[] data0 = { _key, ChannelData[_key][ChannelData[_key].Count - 1], false };
                EventDispatcher.DispatchInnerEvent(EventDispatcher.talkMessageEvent, data0);
            }
            ChannelData[_key].Add(_value);
        }
        else
        {
            ListChatInfo.Add(_value);
            ChannelData.Add(_key, ListChatInfo);
        }
        object[] data1 = { _key, ChannelData[_key][ChannelData[_key].Count - 1], true };
        EventDispatcher.DispatchInnerEvent(EventDispatcher.talkMessageEvent, data1);
    }

    private string GetDataTime()
    {
        System.DateTime _dt = System.DateTime.Now;
        string _dataTime = _dt.Month + "/" + _dt.Day + " " + _dt.Hour + ":" + _dt.Minute + ":" + _dt.Second + " ";
        return _dataTime;
    }
    public int GetPid(int c_id)
    {
        foreach (int k in ChannelPanelId.Keys)
        {
            if (ChannelPanelId[k] == c_id)
            {
                return k;
            }
        }
        return 0;
    }
    private int GetCid(int p_id)
    {
        foreach (int k in ChannelPanelId.Keys)
        {
            if (k == p_id)
            {
                return ChannelPanelId[k];
            }
        }
        return 0;
    }
   
}