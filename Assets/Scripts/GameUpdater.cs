using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PDollarGestureRecognizer;
using Experiment;



public class GameUpdater : MonoBehaviour {

    //private List<ExperimentItem> ExperimentList;
    public ExperimentModule experimentModule;
    private AudioVolumeManager bgm;
    public GameObject Wizard;
    public GameObject[] zombiePrefabs;
    public bool is_infiniteStage = false;
    private float SpawnTime;
    public int StageCount;
   

    private float SpawnCurrTime = 0;


    private int zombieSpawnedCount=0;
    public int getScorePerSign = 10;
    public int MaxZomibeCount;
    public float zombieMaxSpeed;
    public float zombieMinSpeed;





    private bool stageTrigger = false;






    public List<GameObject> zombieList;
    [HideInInspector]
    public GameObject World;


    private bool isHpSlider=false;
    private int getEnemyScore = 0;
    private int getBossScore = 0;
  
    public int resultScore = 0;
    private int comboCount;



    private int usedEntireSkillCount = 0;
    public Camera worldCamera;
    private List<GameObject> checkSignList;

    public SkillEffectSystem skillEffectSystem;

    public GameObject RandomSign(ref GameObject zombie)
    {
        GameObject[] cpy_SignCountList = zombie.GetComponent<EnemyAI>().Prefabs;
        GameObject prefab = cpy_SignCountList[0];

        //List<ExperimentItem> cpy_SignCountList = new List<ExperimentItem>();

        //for (int i = 0; i < ExperimentList.Count; ++i)
        //{
        //    if (ExperimentList[i].key != ExperimentList[i].value)
        //    {
        //        cpy_SignCountList.Add(ExperimentList[i]);
        //    }
        //}



        int pick_index = (int)(Random.Range(0, cpy_SignCountList.Length * 1000) / 1000.0f);
        string signName = cpy_SignCountList[pick_index].GetComponent<SignInfo>().sign;

        for (int i = 0; i < zombie.GetComponent<EnemyAI>().Prefabs.Length; ++i)
        {
            if (zombie.GetComponent<EnemyAI>().Prefabs[i].GetComponent<SignInfo>().sign == signName)
            {
                prefab = zombie.GetComponent<EnemyAI>().Prefabs[i];

                break;
            }

        }

        //for (int i = 0; i < ExperimentList.Count; ++i)
        //{
        //    if (ExperimentList[i].signName == prefab.GetComponent<SignInfo>().sign)
        //    {
        //        ExperimentList[i].value = ExperimentList[i].value + 1;
        //        break;
        //    }
        //}
        //cpy_SignCountList.Clear();


        return prefab;

    }

    public void CreateZombie(ref GameObject zombie)
    {

        GameObject signList = zombie.transform.Find("SignList").gameObject;
        MoveSign[] SignTransforms = signList.GetComponentsInChildren<MoveSign>();
        int currCount = 0;

        for (int i = 0; i < SignTransforms.Length; ++i)
        {
            if (signList != SignTransforms[i].gameObject)
            {
             
                GameObject sign = RandomSign(ref zombie);
                GameObject SignPrefabs = (GameObject)Instantiate(sign);
                Vector2 localPosition = SignPrefabs.transform.localPosition;
                SignPrefabs.transform.parent = SignTransforms[i].transform;
                SignPrefabs.transform.localEulerAngles = new Vector3(0, 0, 0);
                SignPrefabs.transform.localScale = new Vector3(30, 30, 1);
                SignPrefabs.transform.localPosition = new Vector3(localPosition.x, localPosition.y, -1);
                currCount += 1;
                zombie.GetComponent<EnemyAI>().SignList.Add(SignPrefabs);
                     

                if (currCount >= zombie.GetComponent<EnemyAI>().SignCount)
                {
                    break;
                }
            }
        }

    }
 
  
    void Awake()
    {
        //ExperimentList = gameObject.GetComponent<ExperimentModule>().ExperimentList;
        //experimentModule = gameObject.GetComponent<ExperimentModule>();
    }


	// Use this for initialization
	void Start () {
        


        bgm = GameObject.Find("UI Root").transform.Find("Camera(UI)").Find("BGM").GetComponent<AudioVolumeManager>();
        zombieList = new List<GameObject>();
        World = GameObject.Find("UI Root").transform.Find("Panel(World)").gameObject;
        zombieList = new List<GameObject>();
        skillEffectSystem = GameObject.Find("Updater").transform.Find("SkillEffectSystem").GetComponent<SkillEffectSystem>();
        SpawnCurrTime = 0;




        checkSignList = new List<GameObject>();
       







        if (is_infiniteStage == true)
        {
            stageTrigger = true;
            StageCount = 0;
        }


	}

    public void CheckGetEnemyScore()
    {
        int signDeadCount = checkSignList.Count;
        getEnemyScore = getEnemyScore + signDeadCount * getScorePerSign;
        comboCount = checkSignList.Count;

    }

    public void CameraMoveBySituation(string skillName)
    {
        worldCamera.GetComponent<CameraMove>().gradient = 0.25f;
        if (checkSignList.Count >= 2)
        {
            worldCamera.GetComponent<CameraMove>().SetShakeState(CameraMove.Shake.MOVE);
        }

    }


    // Update is called once per frame
    void Update()
    {

            if (stageTrigger == true)
            {

                
                    zombieList.Clear();
                    zombieSpawnedCount = 0;
                    //experimentModule.Add_all_key(3);

                    MaxZomibeCount = (int)(60 * Mathf.Exp(0.01f * (float)StageCount));
                    //for (int i = 0; i < gameObject.GetComponent<ExperimentModule>().ExperimentList.Count; ++i)
                    //{
                    //    MaxZomibeCount += gameObject.GetComponent<ExperimentModule>().ExperimentList[i].key;
                    //}

                    StageCount += 1;
                    zombieMaxSpeed = 0.5f;
                    zombieMinSpeed = 0.5f;
                    SpawnTime = 1.0f;//StageTime / (float)MaxZomibeCount;
                    stageTrigger = false;

                

            }

            if (zombieSpawnedCount < MaxZomibeCount)
            {
                SpawnCurrTime += Time.deltaTime;
                if (SpawnCurrTime > SpawnTime)
                {
                    float randomZombie = (int)Random.Range(0, zombiePrefabs.Length * 1000);
                    int zombieIndex = (int)(randomZombie / 1000.0f);
                    GameObject zombie = (GameObject)Instantiate(zombiePrefabs[zombieIndex]);
                    zombie.transform.parent = World.transform;
                    zombie.transform.localScale = new Vector3(1, 1, 1);
                    zombie.GetComponent<EnemyAI>().SignCount = (int)(Random.Range( 1000, 3000) / 1000.0f);
                    zombie.GetComponent<EnemyAI>().originSpeed = Random.Range(zombieMinSpeed * 1000, zombieMaxSpeed * 1000) / 1000.0f;
                    zombie.GetComponent<EnemyAI>().currSpeed = zombie.GetComponent<EnemyAI>().originSpeed;
                    CreateZombie(ref zombie);
                    zombieList.Add(zombie);
                    
                    SpawnCurrTime = 0;
                    zombieSpawnedCount += 1;
                }
            }
            else
            {
                if (zombieList.Count<=0  && stageTrigger==false)
                {
                    //experimentModule.SaveStageData("StageData" + (StageCount - 1) + ".csv", "StageData" + StageCount + ".csv");
                    //experimentModule.Update_Data();
                    //experimentModule.SaveTotalData("TotalData.csv");
                    stageTrigger = true;

                }

            }



    }


        
	



    public void OnGestureResult(string GestureClass)
    {

            int skillcount = skillEffectSystem.SkillEffectList.Count;
            GameObject pSkillEffect = null;
            SkillEffectInfo effectInfo = null;
            bool applySkill = true;
            string skillName = null;
            for (int i = 0; i < skillcount; ++i)
            {
                //표식 검사전 스킬정보에 대해 검사
                if (skillEffectSystem.SkillEffectList[i].sign == GestureClass)
                {
                    skillName = GestureClass;
                    effectInfo = skillEffectSystem.SkillEffectList[i];
                    pSkillEffect = skillEffectSystem.SkillEffectList[i].skillEffect;
                    applySkill = effectInfo.GetIsUse();
                    break;
                }

            }
            if (applySkill == true)
            {

                for (int i = zombieList.Count - 1; i >= 0; i--)
                {
              


                    //스킬 적용 
                    for (int j = zombieList[i].transform.GetComponent<EnemyAI>().SignList.Count - 1; j >= 0; j--)
                    {

               
                            //일반스킬일 경우
                            bool isRemove = zombieList[i].transform.GetComponent<EnemyAI>().SignList[j].transform.GetComponent<SignInfo>().remove;
                            if (isRemove == false)
                            {

                                string sign = zombieList[i].transform.GetComponent<EnemyAI>().SignList[j].transform.GetComponent<SignInfo>().sign;
                                if (sign == GestureClass)
                                {

                                    SkillInfo skillInfo = new SkillInfo();
                                    skillInfo.ApplySkill = pSkillEffect;
                                    skillInfo.skillName = skillName;
                                    skillInfo.target = zombieList[i];
                                    skillInfo.signList.Add(zombieList[i].transform.GetComponent<EnemyAI>().SignList[j].transform.GetComponent<SignInfo>().gameObject);
                                    skillInfo.effectInfo = effectInfo;
                                    skillInfo.magicType = SkillInfo.MagicType.NORMAL;
                                    zombieList[i].transform.GetComponent<EnemyAI>().appliedSkillList.Add(skillInfo);
                                    zombieList[i].transform.GetComponent<EnemyAI>().SignList[j].transform.GetComponent<SignInfo>().remove = true;
                                    Wizard.GetComponent<WizardMove>().Move(zombieList[i]);
                                    checkSignList.Add(zombieList[i].GetComponent<EnemyAI>().SignList[j]);
                                    //gameObject.GetComponent<ExperimentModule>().Add_CorrectCount(sign);

                                }
                                }
                    }
                }
                CameraMoveBySituation(skillName);
                CheckGetEnemyScore();
            }
            Wizard.GetComponent<WizardMove>().resetMaxDistance();
            checkSignList.Clear();
            
        
        
    }
}
