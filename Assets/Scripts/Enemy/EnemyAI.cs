using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    public AudioVolumeManager[] zombieDeadSFX;

    public GameObject[] Prefabs;
    public List<GameObject> SignList;
    public List<SkillInfo> appliedSkillList;

    [Range(0, 1000)]
    public float MaxSignRandomRange = 1000;
    [Range(0, 1000)]
    public float MInSignRandomRange = 0;
    [Range(0, 3)]
    public int SignCount=3;

    public float originSpeed;
    public float currSpeed = 100;
    protected float DestX = 0;
    protected float RandomPosY;
    protected GameUpdater gameUpdater;
    protected SkillEffectSystem skillEffectSystem;


    public bool isMove = true;
    protected GameObject TopLeftAnchor;
    protected GameObject TopRightAnchor;
    //animation
    protected Animation animation;
    private bool isDeaded = false;
    private bool isDeadTrigger = false;
    private bool isDeadAnimation = false;
    private bool isDeadAnimationComplete = false;
    private bool isWalkAnimation = false;
    private float randomAnimation_Dead=0;
    private DepthManager depthManager;

    //sound
    protected int deadZombieSFXIndex = 0;
    protected bool isDeadSound = false;
    protected bool isDeadSoundComplete = false;

    private Camera UICam;





	// Use this for initialization
	void Start () {

        depthManager = gameObject.transform.Find("Sprite").GetComponentInChildren<DepthManager>();
   
        appliedSkillList =new List<SkillInfo>();

        gameUpdater = GameObject.Find("Updater").transform.GetComponent<GameUpdater>();

        GameObject UI_Root = GameObject.Find("UI Root");
        UICam = UI_Root.transform.Find("Camera(UI)").GetComponent<Camera>();
        TopLeftAnchor = UI_Root.transform.Find("TopLeftAnchor").gameObject;
        TopRightAnchor = UI_Root.transform.Find("TopRightAnchor").gameObject;
        GameObject BottonRightAnchor =UI_Root.transform.Find("BottonRightAnchor").gameObject;
        GameObject MaxSpawnPosY = UI_Root.transform.Find("MaxSpawnPosY").gameObject;
        RandomPosY = Random.Range(BottonRightAnchor.transform.position.y, MaxSpawnPosY.transform.position.y);
        gameObject.transform.position = new Vector3(BottonRightAnchor.transform.position.x, RandomPosY, gameObject.transform.position.z);
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, -1);


        animation = gameObject.transform.Find("Sprite").GetComponentInChildren<Animation>();



        skillEffectSystem = GameObject.Find("Updater").transform.Find("SkillEffectSystem").GetComponent<SkillEffectSystem>();


	}

    // Update is called once per frame
    void Update()
    {
        
            if (TopLeftAnchor.transform.position.x > gameObject.transform.position.x)
            {
                //for(int i=0; i< SignList.Count; ++i)
                //{
                //    gameUpdater.experimentModule.Add_MissingCount(SignList[i].GetComponent<SignInfo>().sign);
                //}
  
                gameUpdater.zombieList.Remove(gameObject);
                GameObject.Destroy(gameObject);


            }


            isMove = false;
            for (int i = 0; i < SignList.Count; ++i)
            {
                //모든 표식중 삭제 플래그가 한개라도 꺼져있으면
                if (SignList[i].GetComponent<SignInfo>().remove == false)
                {
                    //적의 이동은 활성화
                    isMove = true;
                }

            }

            if (isMove == true)
            {
                if (animation.IsPlaying("zb_walk2") == false)
                {
                    animation.PlayQueued("zb_walk2", QueueMode.PlayNow);
                    animation["zb_walk2"].speed = currSpeed * 4.0f;

                }

                float currX = gameObject.transform.position.x - Time.deltaTime * currSpeed;
                gameObject.transform.position = new Vector3(currX, RandomPosY, gameObject.transform.position.z);
            }
            else
            {
                if (SignList.Count > 0)
                {
                    if (animation.IsPlaying("zb_idle2") == false)
                        animation.PlayQueued("zb_idle2", QueueMode.PlayNow);
                }

            }


            if (SignList.Count <= 0)
            {


                if (isDeadTrigger == false)
                {
                    gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 1);
                    randomAnimation_Dead = (Random.Range(0, 1000) / 1000.0f);
                    deadZombieSFXIndex = (int)(Random.Range(0, 3000) / 1000.0f);
                    isDeadTrigger = true;
                }

                if (randomAnimation_Dead < 0.5f)
                {
                    if (animation.IsPlaying("zb_dead1") == false)
                    {
                        if (isDeadAnimation == false)
                        {

                            animation.PlayQueued("zb_dead1", QueueMode.PlayNow);
                            isDeadAnimation = true;

                        }
                        else
                        {
                            isDeadAnimationComplete = true;
                        }
                    }

                }
                else
                {
                    if (animation.IsPlaying("zb_dead2") == false)
                    {
                        if (isDeadAnimation == false)
                        {
                            animation.PlayQueued("zb_dead2", QueueMode.PlayNow);
                            isDeadAnimation = true;

                        }
                        else
                        {
                            isDeadAnimationComplete = true;
                        }
                    }
                }
                if (isDeadSound == false)
                {
                    zombieDeadSFX[deadZombieSFXIndex].transform.parent =UICam.transform;
                    zombieDeadSFX[deadZombieSFXIndex].transform.localPosition = new Vector3(0, 0, 0);
                    zombieDeadSFX[deadZombieSFXIndex].audio.Play();

                    isDeadSound = true;
                }
                else
                {
                    if (zombieDeadSFX[deadZombieSFXIndex].audio.isPlaying == false)
                    {
                        isDeadSoundComplete = true;
                    }
                }


                if (isDeadAnimationComplete == true && isDeadSoundComplete == true)
                {

                    gameUpdater.zombieList.Remove(gameObject);
                    GameObject.Destroy(zombieDeadSFX[deadZombieSFXIndex].gameObject);
                    GameObject.Destroy(gameObject);
                }

            }





            for (int i = appliedSkillList.Count - 1; i >= 0; --i)
            {

                //타겟이 없는 스킬은 삭제 
                if (appliedSkillList[i].target == null)
                {
                    appliedSkillList.Remove(appliedSkillList[i]);
                    continue;
                }


                //실험용 if문->스킬 이펙트를 모두 적용시 없앰
                if (appliedSkillList[i].effectInfo == null)
                {
                    appliedSkillList[i].EffectDeactivated();
                    appliedSkillList.Remove(appliedSkillList[i]);
                    continue;
                }
                else
                {
                    appliedSkillList[i].effectInfo.PlaySkill();
                    if (appliedSkillList[i].effectInfo.timeDuration >= appliedSkillList[i].duration_timer)
                    {
                        appliedSkillList[i].duration_timer += Time.deltaTime;
                        appliedSkillList[i].Apply();

                    }
                    else
                    {

                        appliedSkillList[i].EffectDeactivated();
                        appliedSkillList.Remove(appliedSkillList[i]);
                        continue;

                    }

                }
            }
        


    }
}
