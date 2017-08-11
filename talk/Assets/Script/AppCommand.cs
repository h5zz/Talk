using System;
using System.Collections.Generic;
using UnityEngine;


public class AppCommand
{

    protected IController m_controller;
    static GameObject m_GameManager;
    static Dictionary<string, object> m_Managers = new Dictionary<string, object>();
    GameObject AppGameManager
    {
        get
        {
            if (m_GameManager == null)
            {
                m_GameManager = GameObject.Find("Main");
            }
            return m_GameManager;
        }
    }
    /**
     * 扩展管理后续添加
     * */

    ///<summary>
    ///添加管理器
    ///</summary>
    public void AddManager(string typeName, object obj)
    {
        /*添加管理器到字典中*/
        if (!m_Managers.ContainsKey(typeName))
        {
            m_Managers.Add(typeName, obj);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="typeName"></param>
    /// <returns></returns>
    public T AddManager<T>(string typeName) where T : Component
    {
        object result = null;
        m_Managers.TryGetValue(typeName, out result);
        if (result != null)
        {
            return (T)result;
        }
        Component c = AppGameManager.AddComponent<T>();
        m_Managers.Add(typeName, c);
        return default(T);
    }
    /// <summary>
    /// 删除管理器
    /// </summary>
    public void RemoveManager(string typeName)
    {
        if (!m_Managers.ContainsKey(typeName))
        {
            return;
        }
        object manager = null;
        m_Managers.TryGetValue(typeName, out manager);
        Type type = manager.GetType();
        if (type.IsSubclassOf(typeof(MonoBehaviour)))
        {
            GameObject.Destroy((Component)manager);
        }
        m_Managers.Remove(typeName);
    }
}
