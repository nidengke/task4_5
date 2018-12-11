using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///<summary>
public class Login2 : MonoBehaviour
{


    //variables
    public string str_uid = "";
    public string str_score = "";
    public string str_response = "";

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //在C#中, 需要用到yield的话, 必须建立在IEnumerator类中执行
    IEnumerator TestPost()
    {
        //WWW的三个参数: url, postData, headers
        string url = "http://127.0.0.1:8000";
        byte[] post_data;
        Hashtable headers;   //System.Collections.Hashtable

        string str_params;
        str_params = "uid=" + str_uid + "&" + "password=" + str_score;
        post_data = System.Text.UTF8Encoding.UTF8.GetBytes(str_params);
        //Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
        //byte[] post_data = encode.GetBytes("uid=中文&score=100");
        headers = new Hashtable();
        //headers.Add("Content-Type","application/x-www-form-urlencoded");
        headers.Add("CONTENT_TYPE", "text/plain");

        //发送请求
        WWW www_instance = new WWW(url, post_data, headers);

        //web服务器返回
        yield return www_instance;
        if (www_instance.error != null)
        {
            Debug.Log(www_instance.error);
        }
        else
        {
            this.str_response = www_instance.text;
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 20, 60, 20), "UID:   ");
        GUI.Label(new Rect(10, 45, 60, 20), "Score: ");
        //注意:因为每一帧都在刷, 所以[文本框]是这种写法:
        str_uid = GUI.TextField(new Rect(60, 20, 160, 20), str_uid);
        str_score = GUI.TextField(new Rect(60, 45, 160, 20), str_score);

        //发送Http的POST请求
        if (GUI.Button(new Rect(120, 80, 100, 25), "发送请求"))
        {
            StartCoroutine(TestPost());
        }

        this.str_response = GUI.TextArea(new Rect(10, 150, 210, 100), this.str_response);
    }
}
