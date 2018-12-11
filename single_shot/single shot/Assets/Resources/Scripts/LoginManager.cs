using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

///<summary>
///
///<summary>
public class LoginManager : MonoBehaviour
{
    //variables
    public static string str_username = "";
    public static string str_password = "";
    public  string str_response = "";

    public static string str_resultScore="";
    //public Text usernameText;
    //public Text passwordText;
    public InputField usernameInputField;
    public InputField passwordInputField;

    public void LoginButton()
    {
        str_username = usernameInputField.text;
        str_password = passwordInputField.text;
        StartCoroutine(TestPost());
        //为1登陆通过,为0已有账号且密码输入错误
        if (this.str_response!=null&& this.str_response!="")
        {
            //Debug.Log("返回值为:" + str_response);
            str_resultScore = str_response;
            SceneManager.LoadScene("main");
        }
        else
        {
            Debug.Log("点击login登陆");
        }
    }
    


    //在C#中, yield必须在IEnumerator中
    IEnumerator TestPost()
    {
        //WWW的三个参数: url, postData, headers
        string url = "http://127.0.0.1:8000/start";
        byte[] post_data;
        Hashtable headers;   //System.Collections.Hashtable

        string str_params;
        str_params = "uid=" + str_username + "&" + "password=" + str_password;
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

}
