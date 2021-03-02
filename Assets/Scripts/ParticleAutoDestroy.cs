using UnityEngine;
using System.Collections;

public class ParticleAutoDestroy : MonoBehaviour {
    private SkillInfo info;
    public void SetSkillInfo(SkillInfo skillinfo)
    {
        info = skillinfo;   
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        bool isStopped = true;
        if (info.skillName == "7")
        {
            isStopped = gameObject.transform.Find("Sky_fire").Find("Sky_fire").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("end");

        }
        else
        {
            isStopped = GetComponent<ParticleSystem>().isStopped;

        }



        if (isStopped==true)
        {
            GameObject.Destroy(gameObject);
        }
	}
}
