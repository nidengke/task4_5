# -*- coding: utf-8 -*-
from __future__ import unicode_literals
import json
from django.shortcuts import render

from django.http import HttpResponse


# Create your views here.
def login(request):
    returnWord = u'服务器返回:\n' + u'用户UI:' + unicode(request.POST['uid']) + '\n'+u'分数:' + unicode(request.POST['password'])
    print("接收到的数据:"+request.POST["uid"]+request.POST['password'])
    with open('userData.json', 'r') as f:
        loadDict = json.load(f)
        if (loadDict.has_key(request.POST["uid"])):
            if(request.POST['password'] == loadDict[request.POST["uid"]]["password"]):
                print("密码正确允许登录")
            else:
                print("密码错误")
        else:
            # print("新账户:"+request.POST["uid"] + "密码:"+request.POST['password'])
            userDate = {request.POST["uid"]: {"password":request.POST['password'], "score":0}}
            loadDict.update(userDate)
            with open('userData.json','w') as f:
                json.dump(loadDict,f)
    return HttpResponse(getScore())

def score(request):
    print("resievescoredata")
    currentData = {request.POST["uid"]:{"password":request.POST["password"],"score":request.POST["score"]}}
    print("接收到当前玩家的数据:",currentData)
    myloadDict={}
    with open('userData.json','r') as f:
        loadDict = json.load(f)
        loadDict.update(currentData)
        myloadDict = loadDict
        f.close()
    print(myloadDict)
    with open('userData.json','w') as f2:
        json.dump(myloadDict,f2)
        f2.close()
    return HttpResponse("gameOverResieveData")

def getScore():
    with open('userData.json','r+') as f:
        loadDict = json.load(f)
        dict_response = {}
        for key in loadDict.keys():
            temp = {key: loadDict[key]["score"]}
            dict_response.update(temp)
        z = zip(dict_response.values(), dict_response.keys())
        # print(dict_response)
        # print(z)
        str_response = ""
        for i in z:
            str_response = str_response + i[1] + ":" + str(i[0]) + "|"
        print(str_response)
        return  str_response


