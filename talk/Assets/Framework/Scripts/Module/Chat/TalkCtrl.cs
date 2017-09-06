using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.AI;
public class TalkCtrl {
    
    private static TalkCtrl _instance;
    public static TalkCtrl Instance
    {
        get
        {
            if(_instance == null) _instance = new TalkCtrl();
            return _instance;
        }
    }
    public GameObject chatPanel = null;

    public void Init()
    {
        GameObject UIRoot = GameObject.Find("UIRoot/Canvas/PopWindow") as GameObject;
        GameObject chatPanel = GameObject.Instantiate(Resources.Load("Prefabs/Chat/ChatPanel")) as GameObject;
        chatPanel.transform.SetParent(UIRoot.transform);
        chatPanel.transform.localPosition = Vector3.zero;
        chatPanel.transform.localScale = Vector3.one;
    }


    ///发送到服务器消息
    ///</summary>
    public void SendServer(int _channel, int _toID, string _msg)
    {
        Debug.LogError("_channel" + _channel);
        Debug.LogError("_msg" + _msg);

        TalkModel.Instance.RecvCompleteMsg(_msg);
        //ReqChatMsg chatmsg = new ReqChatMsg();
        //chatmsg.channel = _channel;
        //chatmsg.toID = _toID;
        //chatmsg.text = _msg;
        //NetInterface.Instance.SendMsgPkg(chatmsg);
    }
}
