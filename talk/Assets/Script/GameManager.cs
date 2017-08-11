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
        TalkCtrl.Instance.Init();
    }
}
