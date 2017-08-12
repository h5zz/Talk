using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*定义数据
 * 接收数据存储
 * 发送数据存储
 * */


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

    public void Init()
    {


    }
}
