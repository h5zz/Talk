using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    }
}
