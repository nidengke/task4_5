import json
# with open('userData.json', 'r') as f:
#     loadDict = json.load(f)
#     dict_response = {}
#     for key in loadDict.keys():
#         temp = {key:loadDict[key]["score"]}
#         dict_response.update(temp)
#     z = zip(dict_response.values(),dict_response.keys())
#     print(dict_response)
#     print(z)
#     str_response = ""
#     for i in z:
#         str_response = str_response +i[1]+":"+str(i[0])+"|"
#     print(str_response)
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

getScore()