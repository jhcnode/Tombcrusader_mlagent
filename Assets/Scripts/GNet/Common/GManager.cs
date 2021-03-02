using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using DeepGes;

//Game Class 정의(Abstract)
public abstract class Game : MonoBehaviour
{
    public GameMode mode;
    public GameObject prefab_GiveItem;
    public abstract void AwakeInitialize();
    public abstract void GameInitialize();
    public abstract void GameClose();
}
//컨트롤러 모드 정의
public enum ControlMode
{
    Mouse, ViveController
}
//게임 모드 정의
public enum GameMode
{
    Switching, Park, Game0_DrawPattern, Finish,
}

public class GManager : MonoBehaviour {

    //컨트롤러 모드
    public ControlMode controlMode;
    //바이브 컨트롤러 - 오브젝트
    public GameObject leftController;
    public GameObject rightController;
    //게임 모드
    public GameMode gameMode;
    //게임 리스트
    public List<Game> gameList;

    //유저
    [HideInInspector] public GUser user;
    //네트워크
    [HideInInspector] public GNetwork net;
    //메인 미션 NPC 및 미션 아이템
    public GameObject mainNPC;
    private float sinValue;                     //상하 sin 값
    //Fade In/Out Screen Black Panel
    [HideInInspector] public GameObject screenViewCanvas;
    private Image blackScreen;
    private bool switchFlag;                    //BlackScreen State 값
    private bool switchCompleteFlag;
    private bool netFlag;
    private float switchAlpha;                  //BlackScreen 알파 값
    private GameMode prevTarget;                //이전 mode
    private GameMode switchTarget;              //Switching 대상

    private AsyncOperation async;       //Async 오브젝트

    public float LapTime;

    //프로그램 시작 시
    void Awake () {
 
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        //참조하는 게임오브젝트 및 컴포넌트 찾아서 가져오기
        user = GameObject.Find("User").GetComponent<GUser>();
        net = GetComponent<GNetwork>();
        screenViewCanvas = GameObject.Find("ScreenViewCanvas").gameObject;
        blackScreen = GameObject.Find("ScreenViewCanvas").transform.Find("BlackScreen").GetComponent<Image>();
        //변수 초기화
        sinValue = 0.0f;
        switchFlag = false;
        //======================================
        //switchAlpha = 0.0f;
        //======================================
        switchAlpha = 1.0f;
        switchTarget = GameMode.Park;
        gameMode = GameMode.Switching;
        //======================================

  
        //게임 전체 꺼놓기, Park 만 켜놓기
        for(int i=0;i<gameList.Count;i++)
        {
            gameList[i].AwakeInitialize();
            gameList[i].enabled = false;
        }

    }

    //코루틴 - 로딩
    public IEnumerator StartLoad()
    {
        async = SceneManager.LoadSceneAsync("StartScene");
        async.allowSceneActivation = false;
        yield return async;
    }

    //업데이트
    void Update () {
        //게임모드 - Switching 상태인 경우 Black Panel Fade In/Out 처리
        //Debug.Log(switchAlpha);
        if (gameMode == GameMode.Switching)
        {
            if (switchFlag == false)
            {
                switchAlpha += 1.5f * Time.deltaTime;
                if (switchAlpha >= 1.0f)
                {
                    switchAlpha = 1.0f;
                    switchFlag = true;
                    if (switchTarget == GameMode.Finish)
                    {
                        Application.backgroundLoadingPriority = ThreadPriority.Low;
                        StartCoroutine("StartLoad");
                    }
                    else
                    {
                        //현재 활성화된 모드 찾아서 끄기
                        for (int i = 0; i < gameList.Count; i++)
                        {
                            if (gameList[i].mode == prevTarget)
                            {
                                gameList[i].GameClose();
                                gameList[i].enabled = false;
                                break;
                            }
                        }
                        //Switching 대상 게임 초기화
                        for (int i = 0; i < gameList.Count; i++)
                        {
                            if (gameList[i].mode == switchTarget)
                            {
                                gameList[i].enabled = true;
                                gameList[i].GameInitialize();
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                if (switchTarget == GameMode.Finish)
                {
                    if (async.progress >= 0.9f)
                    {
                        async.allowSceneActivation = true;
                    }
                }
                else
                {
                    switchAlpha -= 1.5f * Time.deltaTime;
                    if (switchAlpha <= 0.0f)
                    {
                        switchAlpha = 0.0f;
                        switchFlag = false;
                        gameMode = switchTarget;
                    }
                }
            }
        }

        blackScreen.color = new Color(0.0f, 0.0f, 0.0f, switchAlpha);
	}

    //게임모드 Switch 요청 함수
    public void GameModeSwitch(GameMode mode)
    {
        //Switch 준비
        if (gameMode != GameMode.Switching)
        {
            prevTarget = gameMode;
            //현재 활성화된 모드 찾아서 끄기
            for (int i = 0; i < gameList.Count; i++)
            {
                if (gameList[i].mode == prevTarget)
                {
                    gameList[i].enabled = false;

                    break;
                }
            }
            //Target mode 지정
            switchTarget = mode;
            gameMode = GameMode.Switching;

        }
    }



  
}
