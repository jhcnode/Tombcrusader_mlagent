using UnityEngine;
using System.Collections;

public class LeaveGhostTrigger : MonoBehaviour {
    public float leave_interval=100;
    private bool isPlay = false;
    private bool isPlayTrigger = true;


    public void SetPlay(bool _isPlay)
    {
        isPlay = _isPlay;
    }
    public  bool GetPlay()
    {
        return isPlay;
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
     
        if (isPlay)
        {
            if (isPlayTrigger == true)
            {

                gameObject.transform.parent.GetComponent<MoveSign>().enabled = false;
                gameObject.transform.parent.GetComponent<TweenPosition>().enabled = false;
                gameObject.GetComponent<TweenPosition>().speed = 200;
                gameObject.GetComponent<TweenPosition>().ResetToBeginning();
                Vector3 currPos = gameObject.transform.localPosition;
                gameObject.GetComponent<TweenPosition>().from = new Vector3(currPos.x, currPos.y, currPos.z);
                gameObject.GetComponent<TweenPosition>().to = new Vector3(currPos.x, currPos.y + leave_interval, currPos.z);
                isPlayTrigger = false;
            }
     
        }

        if (gameObject.GetComponent<TweenPosition>().enabled == false && isPlayTrigger==false)
        {
            if (isPlay == true)
            {
                isPlay = false;
                isPlayTrigger = true;
                GameObject.Destroy(gameObject);

            }
        }
        else
        {
            Vector3 from = gameObject.GetComponent<TweenPosition>().from;
            Vector3 to = gameObject.GetComponent<TweenPosition>().to;
            float maxDistance = Vector3.Distance(from, to);
            float currDistance = Vector3.Distance(gameObject.transform.localPosition, to);
            Color color = gameObject.GetComponent<SpriteRenderer>().color;
            color.a = (currDistance / maxDistance);
            gameObject.GetComponent<SpriteRenderer>().color = color;




        }
    }
}
