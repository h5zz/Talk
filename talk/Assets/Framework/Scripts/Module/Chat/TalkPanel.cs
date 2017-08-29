using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using UnityEngine.AI;

public class TalkPanel : MonoBehaviour
{
    private static TalkPanel _instance;
    public static TalkPanel Instance
    {
        get
        {
            if (_instance == null) _instance = new TalkPanel();
            return _instance;
        }
    }
    public GameObject ChatPanel()
    {
        GameObject ChatPanel = Resources.Load("Prefabs/Chat/ChatPanel") as GameObject;
        GameObject popWin = GameObject.Find("UIRoot/Canvas/PopWindow").gameObject;
        //Debug.Log(">>>>>" + PopWin);
        GameObject _chatPanel = Instantiate(ChatPanel);
        _chatPanel.transform.SetParent(popWin.transform);
        _chatPanel.transform.localPosition = Vector3.zero;
        _chatPanel.transform.localScale = Vector3.one;
        return _chatPanel;
    }
}
