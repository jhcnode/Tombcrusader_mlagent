using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SkillInfo
{
    public enum MagicType { NORMAL, ENTIRE };
    public MagicType magicType;
    public GameObject target;
    public SkillEffectInfo effectInfo;
    public GameObject ApplySkill;
    public string skillName;
    public List<GameObject> signList=new List<GameObject>();
    public float duration_timer;
    [HideInInspector]
    public float acceleration = 1;
    [HideInInspector]
    public bool isPlay = false;
    



    public void Apply()
    {
        GameUpdater gameUpdater = GameObject.Find("Updater").GetComponent<GameUpdater>();

      
        if (isPlay==false)
        {
            GameObject applySkill = (GameObject)GameObject.Instantiate(ApplySkill);
            applySkill.transform.localScale = new Vector3(1, 1, 1);
            applySkill.transform.parent = gameUpdater.World.transform;
            Transform audio = applySkill.transform.Find("Audio");
            if (audio != null)
            {
                audio.parent = Camera.main.transform;
                audio.transform.localPosition = new Vector3(0, 0, 0);

            }

            applySkill.transform.position = target.transform.position;

            applySkill.GetComponent<ParticleAutoDestroy>().SetSkillInfo(this);
            ApplySkill = applySkill;
               

            isPlay = true;
        }
        

        isPlay = true;

    }


    public void EffectDeactivated()
    {

        for (int i = signList.Count - 1; i >= 0; --i)
        {
            GameObject sign = signList[i];
            GameObject ghost = sign.transform.Find("ghost").gameObject;
            ghost.transform.parent = sign.transform.parent;
            ghost.GetComponent<LeaveGhostTrigger>().SetPlay(true);

            target.GetComponent<EnemyAI>().SignList.Remove(sign);
            signList.Remove(sign);
            GameObject.Destroy(sign);
        }
        
    }


}

public class SkillEffectSystem : MonoBehaviour
{
    public List<SkillEffectInfo> SkillEffectList;
    private GameObject TopLeftAnchor;
    private GameObject TopRightAnchor;
    [HideInInspector]
    public GameUpdater updater;
    private FileLoader fileLoader;



	// Use this for initialization
	void Start () {
      

        updater = GameObject.Find("Updater").GetComponent<GameUpdater>();
       
        SkillEffectList = new List<SkillEffectInfo>();
        SkillEffectInfo[] SkillEffects = gameObject.GetComponentsInChildren<SkillEffectInfo>();
        for (int i = 0; i < SkillEffects.Length; ++i)
        {

            SkillEffectList.Add(SkillEffects[i]);
      
        }
     
       


	}
	
	// Update is called once per frame
	void Update () {
	}
}
