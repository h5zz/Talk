using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
public class ChatItemMessagebar : MonoBehaviour {

    private Dictionary<int,int> DicItemName = new Dictionary<int,int>();

    public List<GameObject> ListItemName;
    public List<InlineText> ListItemText;
    // Use this for initialization
    void Awake () {
        DicItemName.Add(0, 0);//  "公告");
        DicItemName.Add(1, 2);//, "世界");
        DicItemName.Add(2, 3);//, "场景");
        DicItemName.Add(3, 5);//, "帮派");
        DicItemName.Add(4, 6);//, "队伍");
        DicItemName.Add(5, 1);//, "系统");
    }
    private void Start()
    {
        //string text = "<color=#ffffff>七个</color>";
        //setItemText(-1, "\u3000\u3000"+text, 100);
    }
    public void setItemText(int id, string text, float maxWidth)
    {
        InlineText obj = setItemName(id);
        obj.text = text;

        if (Regex.IsMatch(text, (@"<a ([^>\n\s]+)>(.*?)(</a>)")))
        {
            //Debug.Log("可以点击");
            //obj.onHrefClick.AddListener(Onclick);
        }

        //Debug.LogError("外面的大小w" + obj.preferredWidth + "h" + obj.preferredHeight);
        //float w = (obj.preferredWidth > maxWidth) ? (w = maxWidth) : (w = obj.preferredWidth);
        //obj.setRectTransform(maxWidth);
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(obj.GetComponent<RectTransform>().sizeDelta.x,obj.GetComponent<RectTransform>().sizeDelta.y);
    }

    private void Onclick(string id)
    {
        if (id == "1")
        {
            //this.Close();
            //寻路
            //NavMeshPath path = new NavMeshPath();
            // MainPlayerController.Instance.mainPlayer.avatar.agent.CalculatePath(targetPos, path);

            //MainPlayerController.Instance.AutoRun(targetPos, null, path, true);
        }
        else
        {
            //弹道具Tips页
            Debug.LogError("---->Itemid: " + id);
        }
    }

    private InlineText setItemName(int Cid)
    {
        int id = 0;
        foreach(int k in DicItemName.Keys)
        {
            if (Cid == DicItemName[k]){
                id = k;
            }
        }
        for( int i = 0; i < ListItemName.Count; i++)
        {
            ListItemName[i].SetActive(false);
        }
        ListItemName[id].SetActive(true);

        return ListItemText[id];
    }
}

