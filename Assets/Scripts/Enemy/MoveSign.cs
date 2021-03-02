using UnityEngine;
using System.Collections;

public class MoveSign : MonoBehaviour {
    private bool isReverse = false;
    

	// Use this for initialization
	void Start () {
        Vector3 deltaDest = gameObject.transform.up * 50 + gameObject.transform.localPosition;
        gameObject.GetComponent<TweenPosition>().from = gameObject.transform.localPosition;
        gameObject.GetComponent<TweenPosition>().to = new Vector3( deltaDest.x,  deltaDest.y);
        gameObject.GetComponent<TweenPosition>().speed = 50;



    }
	
	// Update is called once per frame
	void Update () {
        if (gameObject.GetComponent<TweenPosition>().enabled == false)
        {
            if (isReverse == false)
            {
                gameObject.GetComponent<TweenPosition>().PlayForward();
                isReverse = true;
            }
            else
            {
                gameObject.GetComponent<TweenPosition>().PlayReverse();
                isReverse = false;
            }

        }

    }
}
