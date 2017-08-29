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
    complete = 1,   //全部（综合）
    world = 2,   //世界
    area = 3,   //场景（附近）
    camp = 4,   //阵营
    gang = 5,   //帮派
    team = 6,   //队伍
    horn = 7,   //喇叭
    privat = 8     //私聊
}
/// <summary>
/// 定义单条消息数据
/// </summary>
public class ChatInfoData
{
    public int VIP = 0;
    public int level = 0;
    public byte icon = 0;
    public long sendId = 0;
    public long teamid = 0;
    public long guildid = 0;
    public string name = null;
    public string time = null;
    public string text = null;
}

public class TalkModel {
    private static TalkModel _instance;
    public static TalkModel Instance
    {
        get
        {
            if (_instance == null) _instance = new TalkModel();
            return _instance;
        }
    }
    //设定存储消息条数
    public int infoListNum = 20;
    private List<ChatInfoData> ChatInfo = new List<ChatInfoData>();
    private Dictionary<ChannelType, List<ChatInfoData>> ChannelData = new Dictionary<ChannelType, List<ChatInfoData>>();
    public void Init()
    {


    }
    /// <summary>
    /// 接收存储服务消息
    /// </summary>
    /// <param name="output"></param>
    private void OnTalkMsg()
    {
        RespChatMsg msg = new RespChatMsg();
        //msg.Decode();

        var infoData = new ChatInfoData();
        infoData.time = DataTime();
        infoData.VIP = msg.senderVIP;
        infoData.icon = msg.senderIcon;
        infoData.name = msg.senderName;
        infoData.text = msg.text;
        infoData.level = msg.senderLvl;
        infoData.teamid = msg.senderTeamId;
        infoData.sendId = msg.senderID;
        infoData.guildid = msg.senderGuildId;
        //打包数据
        switch (msg.channel)
        {
            case 1:
                SetInfoData(ChannelType.complete, infoData);
                break;
            case 2:
                SetInfoData(ChannelType.world, infoData);
                break;
            case 3:
                SetInfoData(ChannelType.area, infoData);
                break;
            case 5:
                SetInfoData(ChannelType.gang, infoData);
                break;
            case 6:
                SetInfoData(ChannelType.team, infoData);
                break;
            case 8:
                SetInfoData(ChannelType.privat, infoData);
                break;
        }
    }

    private void SetInfoData(ChannelType _key, ChatInfoData _value)
    {
        if (ChannelData.ContainsKey(_key))
        {
            if (ChannelData[_key].Count >= infoListNum)
            {
                ChannelData[_key].RemoveAt(0);
            }
            ChannelData[_key].Add(_value);
        }
        else
        {
            ChatInfo.Add(_value);
            ChannelData.Add(_key, ChatInfo);
        }
    }
    private string DataTime()
    {
        System.DateTime _dt = System.DateTime.Now;
        string _dataTime = _dt.Month + "/" + _dt.Day + " " + _dt.Hour + ":" + _dt.Minute + ":" + _dt.Second + " ";
        return _dataTime;
    }
}
