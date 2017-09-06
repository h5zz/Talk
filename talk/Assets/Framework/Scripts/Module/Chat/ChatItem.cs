using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChatItem : MonoBehaviour {

    public Text TimeNameObj;
    public Image ChatTextBg;
    public InlineText ChatTextObj;
    public Text LevelObj;
    public Image IconObj;

    public float ItemHeigth = 180;
    public float ItemMaxWidth = 300;

    public void setText(string text, float maxWidth, Sprite iconSp=null)
    {
        //Debug.LogError("外面的大小w" + ChatTextObj.preferredWidth + "h" + ChatTextObj.preferredHeight);
        ChatTextObj.text = text;
        //float w = (ChatTextObj.preferredWidth > maxWidth) ? (w = maxWidth) : (w = ChatTextObj.preferredWidth);
        //float h = (ChatTextObj.preferredHeight > ItemHeigth) ? (h = ChatTextObj.preferredHeight) : (h = ItemHeigth);
        //ChatTextObj.setRectTransform(ItemMaxWidth, ItemHeigth);
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(ChatTextObj.GetComponent<RectTransform>().sizeDelta.x, ChatTextObj.GetComponent<RectTransform>().sizeDelta.y);
        ChatTextBg.GetComponent<RectTransform>().sizeDelta = new Vector2(ChatTextObj.GetComponent<RectTransform>().sizeDelta.x+ChatTextObj.fontSize, ChatTextObj.GetComponent<RectTransform>().sizeDelta.y+ChatTextObj.fontSize);
        if(iconSp != null)
        {
            IconObj.sprite = iconSp;
        }
    }

    public void setLevel(int value)
    {
        LevelObj.text = value.ToString();
    }

    public void setTimeName(string text)
    {
        TimeNameObj.text = text;
    }
}
