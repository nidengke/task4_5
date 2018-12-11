using System.Collections;
using System.Collections.Generic;
using System.IO;//引用此命名空间是用于数据的写入与读取
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Linq;


///<summary>
///游戏结束分数排名处理
///<summary>
public class Result : MonoBehaviour
{
    /// <summary>
    /// 本地存储
    /// </summary>
    GameObject[] scoreTextList;
    GameObject[] playerName;
    GameObject getScore;
    GameObject Restart;
    Text currentText1;
    Text currentText2;
    public int[] rankScoreArray;
    byte[] bytes;
    public static bool resultSaveFlag = true;
    private string[] strScoreList;
    //排序后字典
    private IEnumerable<KeyValuePair<string, int>> dictSort;

    Dictionary<string, int> dict = new Dictionary<string, int>();



    private void Awake()
    {
        bytes = new byte[1000];
        rankScoreArray = new int[11];
        scoreTextList = new GameObject[10];
        playerName = new GameObject[10];
    }
    void Start()
    {
        for(int i =0;i<10;i++)
            scoreTextList[i] = GameObject.FindGameObjectWithTag("Score"+i);
        for (int i = 0; i < 10; i++)
            playerName[i] = GameObject.FindGameObjectWithTag("Player" + i);
        getScore = GameObject.FindGameObjectWithTag("getScore");
        Restart = GameObject.FindGameObjectWithTag("Restart");
        Restart.GetComponent<Button>().onClick.AddListener(OnClick);


    }
    void OnClick()
    {
        StartCoroutine(TestPost());
        Debug.Log("restart");
        //SceneManager.LoadScene("start");
        SceneManager.LoadScene("Login");
    }
    //private void Update()
    //{
    //    if (PlayerContral.flag)
    //    {
    //        Debug.Log("最终接受字符串是:" + str_response);
    //        Debug.Log("score is resive" + PlayerContral.score);
    //        Load();
    //        //for (int j = 0; j < rankScoreArray.Length; j++)
    //        //    Debug.Log("数字"+j+"是:"+ rankScoreArray[j]);
    //        int loopNumber = 0;
    //        foreach (var scoreText in scoreTextList)
    //        {
    //            currentText= scoreText.GetComponent<Text>();
    //            currentText.text = rankScoreArray[loopNumber].ToString();
    //            loopNumber++;
    //        }
    //        Text thisTimeScore = getScore.GetComponent<Text>();
    //        thisTimeScore.text = PlayerContral.score.ToString();
    //        PlayerContral.flag = false;
    //        SaveData();
    //    }
    //}
    private void Update()
    {
        if (PlayerContral.flag)
        {
            //Debug.Log("最终接受字符串是:" + LoginManager.str_resultScore);
            //Debug.Log("score is resive" + PlayerContral.score);
            stringHandle(LoginManager.str_resultScore);
            //for (int j = 0; j < rankScoreArray.Length; j++)
            //    Debug.Log("数字"+j+"是:"+ rankScoreArray[j]);
            int loopNumber = 0;
            foreach (KeyValuePair<string, int> kvp in dictSort)
            {
                if (loopNumber > 9)
                    break;
                var scoreText = scoreTextList[loopNumber];
                if (scoreText)
                {
                    currentText1 = scoreText.GetComponent<Text>();
                    currentText1.text = kvp.Value.ToString();
                }

                var player = playerName[loopNumber];
                if(player)
                {
                    currentText2 = player.GetComponent<Text>();
                    currentText2.text = kvp.Key.ToString();
                }

                loopNumber++;
            }
            Text thisTimeScore = getScore.GetComponent<Text>();
            thisTimeScore.text = PlayerContral.score.ToString();
            PlayerContral.flag = false;
        }
    }
    private void stringHandle(string str)
    {
        if (!dict.Keys.Contains<string>(LoginManager.str_username))
            dict.Add(LoginManager.str_username, PlayerContral.score);
        strScoreList=str.Split('|');
        foreach (var strScore in strScoreList)
        {
            if (strScore!="")
            {
                if (!dict.Keys.Contains<string>(strScore.Split(':')[0]))
                    dict.Add(strScore.Split(':')[0], int.Parse(strScore.Split(':')[1]));
            }
        }
        dictSort = from objDic in dict orderby objDic.Value descending select objDic;

    }


    public void Load()
    {
        rankScoreArray[10] = PlayerContral.score;
        for (int i=0;i<10;i++)
        {
            if (PlayerPrefs.HasKey("user" + i))
                rankScoreArray[i] = PlayerPrefs.GetInt("user" + i);
            else
                rankScoreArray[i] = 1;
        }
        Array.Sort(rankScoreArray);
        Array.Reverse(rankScoreArray);
    }

    public void SaveData()
    {
        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.SetInt("user"+i, rankScoreArray[i]);
        }
    }

    /// <summary>
    /// 服务器存储
    /// </summary>
    //variables
    //在C#中, yield必须在IEnumerator中
    string finalReturn;
    IEnumerator TestPost()
    {
        //WWW的三个参数: url, postData, headers
        string url = "http://127.0.0.1:8000/score";
        byte[] post_data;
        Hashtable headers;   //System.Collections.Hashtable
        string str_params;
        str_params = "uid=" +  LoginManager.str_username + "&" + "password=" + LoginManager.str_password+ "&" + "score="+PlayerContral.score;
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
            this.finalReturn = www_instance.text;
        }
    }
    
}









