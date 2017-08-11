using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AssetBundleInfo
{
    public AssetBundle m_AssetBundle;
    public int m_ReferencedCount;
}
public class LoadAssetRequest
{
    public Type assetType;
    public string[] assetNames;
    //public LuaFunction luaFunc;
    public Action<UnityEngine.Object> sharpFunc;
}
public class ResourceManager : Base {
    string m_BaseDownLoadingURL = "";
    Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();//key值为ab包，value为该ab包的引用
    Dictionary<string, AssetBundleInfo> m_LoadedAssetBundles = new Dictionary<string, AssetBundleInfo>();//已经加载好的ab包字典
    Dictionary<string, List<LoadAssetRequest>> m_LoadRequests = new Dictionary<string, List<LoadAssetRequest>>();//请求加载字典，key值为ab包，value为该包中资源列表
    string tmp = string.Empty;

#if UNITY_EDITOR
    FileStream fs = null;
    StreamWriter sw = null;
#endif

    public void Initialize(string manifestName, Action initOK)
    {
#if UNITY_EDITOR
        string recordPath = Application.dataPath.ToLower() + "/record.txt";
        FileStream fs = new FileStream(recordPath, FileMode.Create);
        sw = new StreamWriter(fs);
#endif
    }

    void Update () {
		
	}
}
