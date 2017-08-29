using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// APP启动控制器
/// 调用框架
/// 实现模块管理
/// AppCommand实现模块管理定义
/// (暂不现实接口，消息定义)
/// 直接启动
/// </summary>
public class AppStartUpCommand : AppCommand {

    /// <summary>
    /// 实现单例
    /// </summary>
    private static AppStartUpCommand _instance;
    public static AppStartUpCommand Instance
    {
        get
        {
            if (_instance == null) _instance = new AppStartUpCommand();
            return _instance;
        }
    }
    /// <summary>
    /// 启动框架
    /// </summary>
    public void StartUp()
    {
        Debug.Log("启动游戏");
        AddManager<GameManager>("GameManager");
    }
}
