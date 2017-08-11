using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChatItemMessage : MonoBehaviour {
    
    public Text MiddeName;
    public InlineText ItemText ;
    public Text ItemRank;
    public Image ItemImage;
    public Text ItemName;
    private bool isWH = true;
    public bool isLeft = false;
    
	// Use this for initialization
	void Start () {
        Debug.Log("11>>>>>>>>>>");
    }
	
	// Update is called once per frame
	void Update () {
        if (ItemImage)
        {
            if (ItemText.Theight > 0)
            {
                if (isWH)
                {
                    if (ItemText.Twidth > 500) ItemText.Twidth = 500;
                    Vector2 rect = new Vector2(ItemText.Twidth + 25, ItemText.Theight + 25);
                    ItemImage.GetComponent<RectTransform>().sizeDelta = rect;
                    isWH = false;
                }
            }
        }
    }
    public void setLevel(string _text)
    {
        ItemRank.text = _text;
    }
    public void setNameTime(string _text)
    {
        ItemName.text = _text;
    }
    public void setItemText(string _text)
    {
        ItemText.text = _text;
    }
    public void setMiddelName(string _text)
    {
        MiddeName.text = _text;
    }
    public void setMiddelText(string _text)
    {
        ItemText.text = _text;
    }
}
