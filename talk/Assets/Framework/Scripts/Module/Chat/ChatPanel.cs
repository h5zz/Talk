using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {

        string st = "abc%sabc%s";
        int i = st.IndexOf("%s");
        st = st.Remove(i, 2);
        st = st.Insert(i, "123");
        //Debug.LogError(">>>>>" + st.IndexOf("%s"));
        Debug.LogError("st:" + st);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
