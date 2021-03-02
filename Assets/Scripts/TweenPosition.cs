using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenPosition : MonoBehaviour {
    public Vector3 from;
    public Vector3 to;
    public float speed = 100;
    private bool isResetToBeginning = false;
    private bool isPlayForward = true;
    private bool isPlayReverse = false;

    public void ResetToBeginning()
    {

        isResetToBeginning = true;
        isPlayForward = true;
        gameObject.GetComponent<TweenPosition>().enabled = true;
    }
    void Awake()
    {
        if (isResetToBeginning == false)
        {
            gameObject.GetComponent<TweenPosition>().enabled = false;
        }
    }
    public void PlayForward()
    {
        isPlayForward = true;
        gameObject.GetComponent<TweenPosition>().enabled = true;
    }

    public void PlayReverse()
    {
        isPlayForward = false;
        gameObject.GetComponent<TweenPosition>().enabled = true;

    }
    // Use this for initialization
    void Start () {

    }
	
   
	// Update is called once per frame
	void Update () {

        Vector3 _from;
        Vector3 _to;
        if (isPlayForward == true)
        {
            _from = from;
            _to = to;

        }
        else
        {
            _from = to;
            _to = from;
        }

        Vector3 dir = _to - _from;
        dir.Normalize();
        Vector3 pos = gameObject.transform.localPosition + dir * Time.deltaTime* speed;
        if (Vector3.Distance(pos, _to) > 5.0f)
        {
            gameObject.transform.localPosition = pos;
        }
        else
        {
            gameObject.transform.localPosition = _to;
            gameObject.GetComponent<TweenPosition>().enabled = false;
            isResetToBeginning = false;

        }


    }
}
