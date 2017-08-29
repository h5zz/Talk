using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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

    

    public void Init()
    {

        GameObject chatPanel = TalkPanel.Instance.ChatPanel();
        
    }
    
}
