using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
//맵 정보 리스트 구조체
public class ListMapStruct
{
    public int stageNumber;
    public int maxScore;
    public float maxTotalScore;
    public bool isLock = true;
}


//랭크 정보 구조체
public class ListRankStruct
{
    public int rank;
    public int score;
}
public class SkillLoadInfo
{
    public string skillName;
    public int level;
}
public class StateLoadInfo
{
    public string stateName;
    public int level;
}

public class ListUserStruct
{
    public ListUserStruct()
    {

    }
    public ListUserStruct(ref ListUserStruct _ListUserStruct)
    {
        money = _ListUserStruct.money;
        for (int i = 0; i < _ListUserStruct.skillLoadInfoList.Count; ++i )
        {
            skillLoadInfoList.Add(_ListUserStruct.skillLoadInfoList[i]);
        }
        for (int i = 0; i < _ListUserStruct.stateLoadInfoList.Count; ++i)
        {
            stateLoadInfoList.Add(_ListUserStruct.stateLoadInfoList[i]);
        }
    }

    public int money;
    public List<SkillLoadInfo> skillLoadInfoList = new List<SkillLoadInfo>();
    public List<StateLoadInfo> stateLoadInfoList = new List<StateLoadInfo>(); 
};

class RankSort : IComparer <ListRankStruct>
{
    public int Compare(ListRankStruct a, ListRankStruct b)
    {
        return b.score.CompareTo(a.score);

    }
}


public class FileLoader : MonoBehaviour {

    //기본경로
    public string FileSystemPath;
    //파일 관련 변수
    private XmlDocument xmlFile;
    private XmlNode rootNode;
    private XmlNode childNode;
    private XmlNodeList list;
    public List<ListMapStruct> mapInfoList;
    public List<ListRankStruct> rankInfoList;
    public ListUserStruct userInfo;

  


    void Awake()
    {
        FileSystemPath =Application.persistentDataPath + "/Document";
        //FileSystemPath = System.IO.Directory.GetCurrentDirectory() + "/Document";
        xmlFile = new XmlDocument();
        if (Directory.Exists(FileSystemPath) == false)
            Directory.CreateDirectory(FileSystemPath);

        if (Directory.Exists(FileSystemPath + "/StageInfo") == false)
            Directory.CreateDirectory(FileSystemPath + "/StageInfo");


        if (Directory.Exists(FileSystemPath + "/RankInfo") == false)
            Directory.CreateDirectory(FileSystemPath + "/RankInfo");

        if (Directory.Exists(FileSystemPath + "/UserInfo") == false)
            Directory.CreateDirectory(FileSystemPath + "/UserInfo");

        mapInfoList = new List<ListMapStruct>();
        rankInfoList = new List<ListRankStruct>();


    }
	
	// Update is called once per frame
	void Update () {
	
	}


    public void SaveUserInfo(ListUserStruct _userInfo)
    {
        if (Directory.Exists(FileSystemPath + "/UserInfo") == false)
            Directory.CreateDirectory(FileSystemPath + "/UserInfo");


        userInfo = null;
        userInfo = _userInfo;


        userInfo.skillLoadInfoList = _userInfo.skillLoadInfoList;
        userInfo.stateLoadInfoList = _userInfo.stateLoadInfoList;



        Save_Root("UserInfo");

        Save_Child("Money");
        Save_Element("Value", userInfo.money + "");


        Save_Child("SkillCount");
        Save_Element("Count", userInfo.skillLoadInfoList.Count + "");
        for (int i = 0; i < userInfo.skillLoadInfoList.Count; ++i)
        {
            Save_Child("SkillInfo_" + i);
            Save_Element("skillName", userInfo.skillLoadInfoList[i].skillName + "");
            Save_Element("level", userInfo.skillLoadInfoList[i].level + "");
        }


        Save_Child("StateCount");
        Save_Element("Count", userInfo.stateLoadInfoList.Count + "");
        for (int i = 0; i < userInfo.stateLoadInfoList.Count; ++i)
        {
            Save_Child("StateInfo_" + i);
            Save_Element("stateName", userInfo.stateLoadInfoList[i].stateName + "");
            Save_Element("level", userInfo.stateLoadInfoList[i].level + "");
        }

        Save_File(FileSystemPath + "/UserInfo/UserInfo.List");


    }
    public void LoadUserInfo()
    {
        if (Directory.Exists(FileSystemPath + "/UserInfo") == false)
            Directory.CreateDirectory(FileSystemPath + "/UserInfo");

        if (userInfo!=null)
        {
            userInfo.skillLoadInfoList.Clear();
            userInfo.stateLoadInfoList.Clear();
            userInfo = null;
        }
        userInfo = new ListUserStruct();
        if (File.Exists(FileSystemPath + "/UserInfo/UserInfo.List") == false)
        {
            userInfo.money = 1000000;

            Save_Root("UserInfo");

            Save_Child("Money");
            Save_Element("Value", userInfo.money + "");

            Save_Child("SkillCount");
            Save_Element("Count", 0 + "");


            Save_Child("StateCount");
            Save_Element("Count", 0 + "");


            Save_File(FileSystemPath + "/UserInfo/UserInfo.List");

        }
        else
        {
            Load_File(FileSystemPath + "/UserInfo/UserInfo.List");
            Load_Nodes("UserInfo/Money");
            userInfo.money = int.Parse(Load_Data("Value"));
            Load_Nodes("UserInfo/SkillCount");
            int Count = int.Parse(Load_Data("Count"));
            for (int i = 0; i < Count; ++i)
            {
                SkillLoadInfo skillLoadInfo = new SkillLoadInfo();
                Load_Nodes("UserInfo/SkillInfo_" + i);
                skillLoadInfo.skillName =Load_Data("skillName");
                skillLoadInfo.level = int.Parse(Load_Data("level"));
                userInfo.skillLoadInfoList.Add(skillLoadInfo);
            }
            Load_Nodes("UserInfo/StateCount");
            Count = int.Parse(Load_Data("Count"));
            for (int i = 0; i < Count; ++i)
            {
                StateLoadInfo stateLoadInfo = new StateLoadInfo();
                Load_Nodes("UserInfo/StateInfo_" + i);
                stateLoadInfo.stateName = Load_Data("stateName");
                stateLoadInfo.level = int.Parse(Load_Data("level"));
                userInfo.stateLoadInfoList.Add(stateLoadInfo);
            }

        }

    }

    public void SaveRankInfo(ListRankStruct Info)
    {

        if (Directory.Exists(FileSystemPath + "/RankInfo") == false)
            Directory.CreateDirectory(FileSystemPath + "/RankInfo");

        LoadRankInfo();
        bool isEqual = false;
        for (int i = 0; i < rankInfoList.Count; ++i)
        {
            if (rankInfoList[i].score == Info.score)
            {
                isEqual = true;
                break;
            }
        }

        if (isEqual==false)
        {
            rankInfoList.Add(Info);
            rankInfoList.Sort(new RankSort());
            if (rankInfoList.Count>5)
            {
                rankInfoList.Remove(rankInfoList[rankInfoList.Count-1]);
            }
        }


        Save_Root("RankList");
        Save_Child("Count");
        Save_Element("RankCount", rankInfoList.Count + "");
        for (int i = 0; i < rankInfoList.Count; ++i)
        {
            Save_Child("RankInfo_" + i);
            rankInfoList[i].rank = i + 1;
            Save_Element("rank", rankInfoList[i].rank + "");
            Save_Element("score", rankInfoList[i].score + "");


        }
        Save_File(FileSystemPath + "/RankInfo/RankInfo.List");

        




    }
    public void LoadRankInfo()
    {
        rankInfoList.Clear();

        if (Directory.Exists(FileSystemPath + "/RankInfo") == false)
            Directory.CreateDirectory(FileSystemPath + "/RankInfo");

        if (File.Exists(FileSystemPath + "/RankInfo/RankInfo.List") == false)
        {
            for (int i = 0; i < 5; ++i)
            {
                ListRankStruct rankInfo = new ListRankStruct();
                rankInfo.rank = i+1;
                rankInfo.score = 0;
                rankInfoList.Add(rankInfo);
            }


            Save_Root("RankList");
            Save_Child("Count");
            Save_Element("RankCount", rankInfoList.Count + "");
            for (int i = 0; i < rankInfoList.Count; ++i)
            {
                Save_Child("RankInfo_" + i);
                Save_Element("rank", rankInfoList[i].rank + "");
                Save_Element("score", rankInfoList[i].score + "");

            }
            Save_File(FileSystemPath + "/RankInfo/RankInfo.List");
        }
        else
        {

            Load_File(FileSystemPath + "/RankInfo/RankInfo.List");
            Load_Nodes("RankList/Count");
            int Count = int.Parse(Load_Data("RankCount"));
            for (int i = 0; i < Count; ++i)
            {
                ListRankStruct rankInfo = new ListRankStruct();
                Load_Nodes("RankList/RankInfo_" + i);
                rankInfo.rank = int.Parse(Load_Data("rank"));
                rankInfo.score = int.Parse(Load_Data("score"));
                rankInfoList.Add(rankInfo);

            }
        }

    }

    public void SaveMapInfo(ListMapStruct Info)
    {
        if (Directory.Exists(FileSystemPath + "/StageInfo") == false)
            Directory.CreateDirectory(FileSystemPath + "/StageInfo");


        LoadMapInfo();
        for (int i = 0; i < mapInfoList.Count; ++i)
        {
            if (mapInfoList[i].stageNumber == Info.stageNumber)
            {
                if (mapInfoList[i].maxScore< Info.maxScore)
                {
                    mapInfoList[i].maxScore = Info.maxScore;
                }

                mapInfoList[i].maxTotalScore = Info.maxTotalScore;
    
                break;
            }

        }
        int nextStageNumber = Info.stageNumber + 1;
        if (mapInfoList.Count < nextStageNumber)
        {
            ListMapStruct stageInfo = new ListMapStruct();
            stageInfo.isLock = false;
            stageInfo.maxScore = 0;
            stageInfo.stageNumber = nextStageNumber;
            mapInfoList.Add(stageInfo);
        }

        


        Save_Root("StageList");
        Save_Child("Count");
        Save_Element("StageCount", mapInfoList.Count + "");
        for (int i = 0; i < mapInfoList.Count; ++i)
        {
            Save_Child("StageInfo_"+i );
            Save_Element("stageNumber", mapInfoList[i].stageNumber + "");
            Save_Element("maxTotalScore", mapInfoList[i].maxTotalScore + "");
            Save_Element("maxScore", mapInfoList[i].maxScore + "");
            Save_Element("isLock", mapInfoList[i].isLock + "");

        }
        Save_File(FileSystemPath + "/StageInfo/StageInfo.List");
        


    }
    public void LoadMapInfo()
    {
        mapInfoList.Clear();
        if (Directory.Exists(FileSystemPath + "/StageInfo") == false)
            Directory.CreateDirectory(FileSystemPath + "/StageInfo");

        if (File.Exists(FileSystemPath + "/StageInfo/StageInfo.List") == false)
        {
            ListMapStruct stageInfo = new ListMapStruct();

            stageInfo.isLock = false;
            stageInfo.maxScore = 0;
            stageInfo.maxTotalScore = 1;
            stageInfo.stageNumber = 1;
            mapInfoList.Add(stageInfo);



            Save_Root("StageList");
            Save_Child("Count");
            Save_Element("StageCount", 1 + "");
            for (int i = 0; i < mapInfoList.Count; ++i)
            {
                Save_Child("StageInfo_" + i);
                Save_Element("stageNumber", mapInfoList[i].stageNumber + "");
                Save_Element("maxTotalScore", mapInfoList[i].maxTotalScore + "");
                Save_Element("maxScore", mapInfoList[i].maxScore + "");
                Save_Element("isLock", mapInfoList[i].isLock + "");

            }
            Save_File(FileSystemPath + "/StageInfo/StageInfo.List");
        }
        else
        {

            Load_File(FileSystemPath + "/StageInfo/StageInfo.List");
            Load_Nodes("StageList/Count");
            int Count = int.Parse(Load_Data("StageCount"));
            for (int i = 0; i < Count; ++i)
            {
                ListMapStruct stageInfo = new ListMapStruct();
                Load_Nodes("StageList/StageInfo_"+i);
                stageInfo.stageNumber = int.Parse(Load_Data("stageNumber"));
                stageInfo.maxTotalScore = float.Parse(Load_Data("maxTotalScore"));
                stageInfo.maxScore = int.Parse(Load_Data("maxScore"));
                stageInfo.isLock = bool.Parse(Load_Data("isLock"));
                mapInfoList.Add(stageInfo);

            }
        }

    }
    //Save 함수 - Root 지정
    private void Save_Root(string Name)
    {
        xmlFile.RemoveAll();
        xmlFile.AppendChild(xmlFile.CreateXmlDeclaration("1.0", "UTF-8", "yes"));
        rootNode = xmlFile.CreateNode(XmlNodeType.Element, Name, string.Empty);
        xmlFile.AppendChild(rootNode);
    }
    //Save 함수 - Child 지정
    private void Save_Child(string Name)
    {
        childNode = xmlFile.CreateNode(XmlNodeType.Element, Name, string.Empty);
        rootNode.AppendChild(childNode);
    }
    //Save 함수 - Element 지정
    private void Save_Element(string Name, string Data)
    {
        XmlElement Element = xmlFile.CreateElement(Name);
        Element.InnerText = Data;
        childNode.AppendChild(Element);
    }
    //Save 함수 - File 저장
    private void Save_File(string Path)
    {
        xmlFile.Save(Path);
        xmlFile.RemoveAll();
    }
    //Load 함수 - File 불러오기
    private void Load_File(string Path)
    {
        xmlFile.RemoveAll();
        if (File.Exists(Path) == true) xmlFile.Load(Path);
    }
    //Load 함수 - Node 지정
    private void Load_Nodes(string Node)
    {
        list = xmlFile.SelectNodes(Node);
    }
    //Load 함수 - Data 입력
    private string Load_Data(string Element)
    {
        return list[0][Element].InnerText;
    }
}
