using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour {

    /// <summary>
    /// 注册消息
    /// </summary>
    /// <param name="view"></param>
    /// <param name="messages"></param>
	protected void RegisterMessage(IView view, List<string> messages)
    {
        if (messages == null || messages.Count == 0) return;
        Controller.Instance.RegisterViewCommand(view, messages.ToArray());
    }
    /// <summary>
    /// 移除消息
    /// </summary>
    /// <param name="view"></param>
    /// <param name="messages"></param>
    protected void RemoveMessage(IView view, List<string> messages)
    {
        if (messages == null || messages.Count == 0) return;
        Controller.Instance.RemoveViewCommand(view, messages.ToArray());
    }
}
