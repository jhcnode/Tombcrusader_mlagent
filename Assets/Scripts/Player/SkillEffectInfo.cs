using UnityEngine;
using System.Collections;

public class SkillEffectInfo : MonoBehaviour {
    public string sign;
    public  GameObject skillEffect;
    public float coolTime;
    public float timeDuration;
    private bool isUse = true;
    [HideInInspector]
    private bool isCoolTrigger = false;
    private float cool_timer = 0;
   
    private GameUpdater updater;


    public bool GetIsUse()
    {
        return isUse;
    }
    public void PlaySkill()
    {
        if (isUse == true)
        {
            isCoolTrigger = true;
            isUse = false;
        }

    }
    void Awake()
    {
        updater = GameObject.Find("Updater").GetComponent<GameUpdater>();
    }
	// Use this for initialization
	void Start () {
      


	}
	// Update is called once per frame
	void Update () {

        if (isCoolTrigger == true)
        {
            if (coolTime > cool_timer)
            {
                cool_timer += Time.deltaTime;
                isUse = false;
                isCoolTrigger = true;
                
            }
            else
            {
                cool_timer = 0;
                isUse = true;
                isCoolTrigger = false;
            }
        }


	}
}
