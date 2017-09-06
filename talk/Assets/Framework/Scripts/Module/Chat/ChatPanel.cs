using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using UnityEngine.AI;
public class ChatPanel : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		
	}
    /// <summary>
    /// 1.注册相关ui事件
    /// 2.侦听消息派发
    /// 3.消息交互
    /// </summary>
    /// 
    private Sprite[] IconSp;//头像集
    void Start()
    {

        ///---添加注册所有事件
        AddScrollbar();//滚动
        AddPanelBtnEvent();//左边按钮与面板
        AddClickBelowPanel();//表情道具等附加

        //--侦听消息
        EventDispatcher.addInnerEventHandler(EventDispatcher.talkMessageEvent, RecvMessageHandle);
        UnItemObj = Resources.Load("Prefabs/Chat/ChatItemMessageBar") as GameObject;
        LItemObj = Resources.Load("Prefabs/Chat/ChatItemL") as GameObject;
        RItemObj = Resources.Load("Prefabs/Chat/ChatItemR") as GameObject;
    }

    ///---
    ///公屏相关定义
    ///    
    public GameObject panelItem;
    public GameObject messageContent;
    public Scrollbar messageScrollbar;
    private bool isAddMessageInfo = false;


    private void AddScrollbar()
    {
        messageScrollbar.onValueChanged.AddListener(delegate (float value) {
            if (isAddMessageInfo)
            {
                messageScrollbar.value = 0;
                isAddMessageInfo = false;
            }
        });
    }

    ///---
    ///左边面板相关定义
    ///---
    public GameObject talkLeftPanel;
    public Button btnLeftPanel;//左边窗体
    public Button btnClos;//关闭左边窗体区域

   

    ///---
    ///表情道具等附加
    ///---

    private int sendToID = 0;
    private int ChatChannel = 2;
    private string repanelNameBelow = null;
    private int currentBelowPanel = 0;
    public GameObject panelBelow;
    public Button btnSend;//发送消息
    public Button btnBelow;//表情附加
    public List<Toggle> BelowWidgetBtnList;//右下角按钮
    public List<GameObject> BelowWidgetPanelList;//右下角面板
    private void AddClickBelowPanel()
    {
        //注册表情面板开关
        int k = -1;
        btnBelow.onClick.AddListener(delegate () {
            k *= -1;
            if (k == 1)
                panelBelow.SetActive(true);
            else
                panelBelow.SetActive(false);
        });
        //注册附加面板开关
        for (int i = 0; i < BelowWidgetBtnList.Count; i++)
        {
            Toggle BtnObj = BelowWidgetBtnList[i];
            int v = i;
            BelowWidgetBtnList[i].onValueChanged.AddListener(delegate (bool isOn)
            {
                if (isOn)
                {
                    setSibling(isOn, BtnObj);
                    BelowWidgetSelectHandle(v);
                }
                else
                {
                    setSibling(isOn, BtnObj);
                }
            });
        }


        Clickface();//注册表情选择

    }

    private void BelowWidgetSelectHandle(int BelowPanelid)
    {
        if (currentBelowPanel == BelowPanelid) return;
        currentBelowPanel = BelowPanelid;
        switch (currentBelowPanel)
        {
            case 0://0表情
                SetBelowWidgetPanelListActive(0);
                break;
            case 1://1记录
                SetBelowWidgetPanelListActive(1);
                break;
            case 2://2道具
                SetBelowWidgetPanelListActive(2);
                break;
            case 3://3装备
                SetBelowWidgetPanelListActive(3);
                break;
            case 4://4定位
                //SetBelowWidgetPanelListActive(2);
                break;
        };

    }
    private void ClickBtnsEmoji(GameObject go)
    {
        inputText.text += "[" + go.name + "}";
    }
    //设置面板隐藏
    private void SetBelowWidgetPanelListActive(int value)
    {
        for (int i = 0; i < BelowWidgetPanelList.Count; i++)
        {
            BelowWidgetPanelList[i].gameObject.SetActive(false);
        }
        BelowWidgetPanelList[value].gameObject.SetActive(true);
    }


    //底部提示条
    public GameObject InputNoticeObj;
    //聊天输入条
    public GameObject InputObj;
    //输入框
    public InputField inputText;
    //切输入框提示文本；
    public Text NoticeText;
    //聊天页签
    public Toggle ChatBtn;
    //好友页签
    public Toggle FriendBtn;
    //聊天面板
    //好友面板

    //右边好友和聊天页签 //好友和聊天页签切换
    

    //快捷聊天条目记录信息
    public GameObject RecordItemObj;

    //道具ID
    private int ItemId;
    //是否坐标
    private bool isLocation = false;
    //是否道具
    private bool isProp = false;
    //是否装备 
    private bool isEquip = false;
    //是否快捷聊天
    private bool isRecord = false;
    //普通聊天 
    private bool isInput = false;
    //快捷聊天信息
    private string RecordStr;


    private void Clickface()
    {
        Button[] btnsEmoji = BelowWidgetPanelList[0].GetComponentsInChildren<Button>();
        for (int i = 0; i < btnsEmoji.Length; i++)
        {
            GameObject emojiTempGo = btnsEmoji[i].gameObject;
            btnsEmoji[i].onClick.AddListener(delegate ()
            {
                ClickBtnsEmoji(emojiTempGo);
            });
        }
    }


    private List<GameObject> recordList = new List<GameObject>();
    //删除记录列表
    private void DestoryObjList(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Destroy(list[i]);
        }
    }




    /// <summary>
    /// 根据条件选择发送
    /// 发出聊天信息
    /// </summary>
    private void ClickSend()
    {
        if ((inputText.text.Trim() == null || inputText.text.Trim() == "")) return;
        string _TempInputText = "";

        _TempInputText = inputText.text;
        TalkCtrl.Instance.SendServer(ChatChannel, sendToID, _TempInputText);
        inputText.text = "";

    }

    /// <summary>
    /// 点击按钮打开面板
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_obj"></param>
    /// <param name="_img"></param>
    /// <param name="_txt"></param>
    /// <param name="_value"></param>
    private void PanelBtnVisible(string _name, GameObject _obj)
    {
        if (repanelNameBelow == _name)
        {
            return;
        }
        for (int i = 0; i < ChatPanelList.Count; i++)
        {
            ChatPanelList[i].SetActive(false);
        }
        _obj.SetActive(true);
    }


    /// <summary>
    /// ==================
    ///  排序按钮对应面板与频道号无关
    ///  =================
    /// </summary>
    private enum ChannelPanelName
    {
        PanelComplete = 0,//综合
        PanelWorld = 1,//世界
        PanelNearBy = 2,//区域
        PanelSociety = 3,//帮派
        PanelTeam = 4,//队伍
        panelSys = 5//系统
    }

    /// <summary>
    /// ==================
    ///  聊天面板与好友面板
    ///  注册相关按钮事件
    ///  =================
    /// </summary>

    private int ChatChannelId = 2;
    private int TeamtoID = 0;
    private int currentPanelId = 1;
    //聊天面板列表
    public List<GameObject> ChatPanelList;
    public List<Toggle> ChatPanelBtnList;
    public List<Scrollbar> ChatScrollbarList;
    private Dictionary<int, bool> isContentHeight = new Dictionary<int, bool>();


    private void AddPanelBtnEvent()
    {
        //注册聊天面板选择按钮

        for (int i = 0; i < ChatPanelBtnList.Count; i++)
        {
            Toggle BtnObj = ChatPanelBtnList[i];
            int v = i;
            ChatPanelBtnList[i].onValueChanged.AddListener(delegate (bool isOn) {
                if (isOn)
                {
                    setSibling(isOn, BtnObj);
                    ChatSelectPanelHandel(v);
                }
                else
                {
                    setSibling(isOn, BtnObj);
                }
            });

        }
        for (int i = 0; i < Enum.GetNames(typeof(ChannelPanelName)).Length; i++)
        {
            isContentHeight.Add(i, false);
        }
        //注册滚动条
        for (int i = 0; i < ChatScrollbarList.Count; i++)
        {
            ChatScrollbarList[i].onValueChanged.AddListener(delegate (float value)
            {
                setPanelScrollbar();
            });
        }

        btnSend.onClick.AddListener(ClickSend);
    }
    private void setSibling(bool isOn, Toggle obj)
    {
        if (isOn) obj.transform.FindChild("Text").SetAsFirstSibling();
        else obj.transform.FindChild("Text").SetAsLastSibling();
    }
    private void setPanelScrollbar()
    {
        if (isContentHeight[currentPanelId])
        {
            ChatScrollbarList[currentPanelId].value = 0;
            isContentHeight[currentPanelId] = false;
        }
    }


    string tips001 = "请切换到其它频道进行聊天";
    private void ChatSelectPanelHandel(int panelid)
    {
        Debug.Log(">>>>>>>><<<<<<<<<<<<" + currentPanelId);
        if (currentPanelId == panelid) return;
        currentPanelId = panelid;
        switch (panelid)
        {
            case 0://0综合
                InputObj.SetActive(false);
                InputNoticeObj.SetActive(true);
                NoticeText.text = tips001;
                PanelVisible(currentPanelId);
                break;
            case 1://1世界聊天
                ChatChannel = 2;
                InputObj.SetActive(true);
                InputNoticeObj.SetActive(false);
                PanelVisible(currentPanelId);
                break;
            case 2://2区域聊天
                ChatChannel = 3;
                InputObj.SetActive(true);
                InputNoticeObj.SetActive(false);
                PanelVisible(currentPanelId);
                break;
            case 3://3帮派
                ChatChannel = 5;
                InputObj.SetActive(false);
                InputNoticeObj.SetActive(true);
                NoticeText.text = tips001;
                PanelVisible(currentPanelId);
                break;
            case 4://4队伍
                ChatChannel = 6;
                TeamtoID = 0;
                if (TeamtoID > 0)
                {
                    InputObj.SetActive(true);
                    InputNoticeObj.SetActive(false);
                    PanelVisible(currentPanelId);
                }
                else
                {
                    InputObj.SetActive(false);
                    InputNoticeObj.SetActive(true);
                    PanelVisible(currentPanelId);
                    NoticeText.text = tips001;
                }
                break;
            case 5://5系统
                ChatChannel = 1;
                InputObj.SetActive(false);
                InputNoticeObj.SetActive(true);
                NoticeText.text = tips001;
                PanelVisible(currentPanelId);
                break;
        }
    }
    /// <summary>
    /// 侦听派发消息，进行面板消息分发
    /// </summary>
    private GameObject UnItemObj;
    private GameObject LItemObj;
    private GameObject RItemObj;

    /// <summary>
    /// 消息包来了
    /// </summary>
    /// <param name="obj"></param>
    private void RecvMessageHandle(object obj)
    {
        //解包数据
        var data = obj as System.Object[];
        //消息位置
        Enum channelName = data[0] as Enum;
        //消息数据
        ChatInfoData msg = data[1] as ChatInfoData;
        //添加还是删除
        bool isMessage = (bool)data[2];

        switch (channelName.ToString())
        {
            case "busscreen":
                if (isMessage)
                    CreatBusScreenUnItem(msg.channelId, msg.name, msg.text);//所有消息进公屏
                else
                    DeleteItemBar((int)ChannelType.busscreen);
                break;
            case "complete":
                if (isMessage)
                    CreatCompleteUnItem(msg.channelId, msg.name, msg.text);//所有消息进综合
                else
                    DeleteItemBar((int)ChannelType.complete);
                break;
            case "sys":
                if (isMessage)
                    CreatSysUnItem(msg.channelId, msg.text);//系统消息
                else
                    DeleteItemBar((int)ChannelType.sys);
                break;
            case "world":
                if (isMessage)
                    CreatItemBar(msg.channelId, msg);//世界消息
                else
                    DeleteItemBar((int)ChannelType.world);
                break;
            case "area":
                if (isMessage)
                    CreatItemBar(msg.channelId, msg);//场景消息
                else
                    DeleteItemBar((int)ChannelType.area);
                break;
            case "gang":
                CreatItemBar(msg.channelId, msg);//暂无帮派
                break;
            case "team":
                if (isMessage)
                    CreatItemBar(msg.channelId, msg);//队伍消息
                else
                    DeleteItemBar((int)ChannelType.team);
                break;
            case "privat":
                break;

        }

    }
    private void PanelVisible(int id)
    {
        for (int i = 0; i < ChatPanelList.Count; i++)
        {
            ChatPanelList[i].gameObject.SetActive(false);
        }
        ChatPanelList[id].SetActive(true);
    }
    /// <summary>
    /// 删除公告，系统，消息条
    /// </summary>
    /// <param name="obj"></param>
    private void DeleteItemBar(int id)
    {
        GameObject CurrentPanelContent = ChatPanelList[TalkModel.Instance.GetPid(id)].transform.FindChild("Viewport/Content").gameObject;
        GameObject Obj = CurrentPanelContent.transform.GetChild(0).gameObject;
        Destroy(Obj);
    }
    private void Onclick(string id)
    {
        if (id == "1")
        {
            //this.Close();
            //寻路
            NavMeshPath path = new NavMeshPath();
            // MainPlayerController.Instance.mainPlayer.avatar.agent.CalculatePath(targetPos, path);

            //MainPlayerController.Instance.AutoRun(targetPos, null, path, true);
        }
        else
        {
            //弹道具Tips页
            Debug.LogError("---->Itemid: " + id);
        }
    }
    /// <summary>
    /// 格式化处理里系统消息方法
    /// </summary>
    /// <param name="text"></param>
    private string SetSysMessage(string text)
    {
        string _text = "";
        string[] strs = text.Split(',');
        string name = "";
        //LuaFramework.ItemConfig.Model itemModel = LuaFramework.ConfigManager.Instance.itemConfig.GetData(int.Parse(strs[0]));
        //if (itemModel != null)
        //{
        //    name = itemModel.name;
        //}
        //else
        //{
        //    LuaFramework.EquipConfig.Model equipModel = LuaFramework.ConfigManager.Instance.equipConfig.GetData(int.Parse(strs[0]));
        //    if (equipModel != null)
        //    {
        //        name = equipModel.name;
        //    }
        //}
        //string st = LuaFramework.ConfigManager.Instance.buttontxtConfig.GetData(17011).txt;
        //int i = st.IndexOf("%s");
        //st = st.Remove(i, 2);
        //st = st.Insert(i, name);
        //int j = st.IndexOf("%s");
        //st = st.Remove(j, 2);
        //st = st.Insert(j, strs[1]);
        //_text = "\u3000\u3000" + st;
        return _text;
    }
    private void CreatSysUnItem(int id, string text)
    {
        //currentPanelId = TalkController.Instance.GetPid(id);
        GameObject CurrentPanelContent = ChatPanelList[TalkModel.Instance.GetPid(id)].transform.FindChild("Viewport/Content").gameObject;
        GameObject _ItemObj = Instantiate(UnItemObj) as GameObject;
        _ItemObj.transform.SetParent(CurrentPanelContent.transform);
        _ItemObj.transform.localPosition = Vector3.zero;
        _ItemObj.transform.localScale = Vector3.one;

        ChatItemMessagebar setway = _ItemObj.GetComponent<ChatItemMessagebar>();
        setway.setItemText(id, SetSysMessage(text), 700);
        isContentHeight[TalkModel.Instance.GetPid(id)] = true;
    }
    //综合
    private void CreatCompleteUnItem(int channelid, string name, string text)
    {
        GameObject CurrentPanelContent = ChatPanelList[TalkModel.Instance.GetPid(0)].transform.FindChild("Viewport/Content").gameObject;
        GameObject _ItemObj = Instantiate(UnItemObj) as GameObject;
        _ItemObj.transform.SetParent(CurrentPanelContent.transform);
        _ItemObj.transform.localPosition = Vector3.zero;
        _ItemObj.transform.localScale = Vector3.one;
        string _text = "";
        ChatItemMessagebar setway = _ItemObj.GetComponent<ChatItemMessagebar>();
        if (channelid == 1) return;

        _text = "\u3000\u3000" + "<color=#53c4e0>【" + name + "】</color>" + text;

        setway.setItemText(channelid, _text, 700);
        isContentHeight[TalkModel.Instance.GetPid(0)] = true;
    }
    //公屏
    private void CreatBusScreenUnItem(int channelid, string name, string text)
    {
        GameObject _ItemObj = Instantiate(UnItemObj) as GameObject;
        _ItemObj.transform.SetParent(messageContent.transform);
        _ItemObj.transform.localPosition = Vector3.zero;
        _ItemObj.transform.localScale = Vector3.one;
        string _text = "";
        ChatItemMessagebar setway = _ItemObj.GetComponent<ChatItemMessagebar>();
        if (channelid == 1) return;

        _text = "\u3000\u3000" + "<color=#53c4e0>【" + name + "】</color>" + text;
        setway.setItemText(channelid, _text, 700);
        isAddMessageInfo = true;
    }

    /// <summary>
    /// 创建世界,气泡类消息
    /// </summary>
    int bname = 0;
    private void CreatItemBar(int id, ChatInfoData data)
    {
        GameObject CurrentPanelContent = ChatPanelList[TalkModel.Instance.GetPid(id)].transform.FindChild("Viewport/Content").gameObject;
        if (MainPlayerModel.roleID == data.sendId)
        {
            GameObject _ItemObj = Instantiate(RItemObj) as GameObject;
            _ItemObj.transform.SetParent(CurrentPanelContent.transform);
            _ItemObj.transform.localPosition = Vector3.zero;
            _ItemObj.transform.localScale = Vector3.one;
            _ItemObj.name = bname.ToString();

            ChatItem setway = _ItemObj.transform.FindChild("GameObject").GetComponent<ChatItem>();
            setway.setTimeName(data.name + ":" + data.time);
            setway.setLevel(data.level);

            if (data.icon != 0)
            {
                Sprite icon = IconSp[(int)data.icon - 1];
                setway.setText(data.text, 600, icon);
            }
            setway.setText(data.text, 600);
        }
        else
        {
            GameObject _ItemObj = Instantiate(LItemObj) as GameObject;
            _ItemObj.transform.SetParent(CurrentPanelContent.transform);
            _ItemObj.transform.localPosition = Vector3.zero;
            _ItemObj.transform.localScale = Vector3.one;
            _ItemObj.name = bname.ToString();
            ChatItem setway = _ItemObj.transform.FindChild("GameObject").GetComponent<ChatItem>();
            setway.setTimeName(data.name + ":" + data.time);
            setway.setLevel(data.level);

            if (data.icon != 0)
            {
                Sprite icon = IconSp[(int)data.icon - 1];
                setway.setText(data.text, 600, icon);
            }
            setway.setText(data.text, 600);
        }
        isContentHeight[TalkModel.Instance.GetPid(id)] = true;
        bname++;
    }
}
