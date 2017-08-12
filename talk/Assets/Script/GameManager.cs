using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Base {
    private void Awake()
    {
        //初始化游戏管理器
        Init();
    }
    private void Init()
    {
        Debug.Log("进入gamemanager");

        //后续这里要有ModelManager管理实现
        //TalkModel.Instance.Init();
        //后续这里要有UIManager管理实现
        //TalkPanel.Instance.Init();
        //后续这里要有CtrlManager管理实现
        TalkCtrl.Instance.Init();

    }
}
