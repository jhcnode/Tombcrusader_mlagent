using UnityEngine;
using System.Collections;

public class Destructor : MonoBehaviour {
    public GameObject gameUpdater;


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<SignInfo>() != null)
        {
            //gameUpdater.GetComponent<GameUpdater>().SignList.Remove(col.gameObject);
            Destroy(col.gameObject);
        }
    }

	// Use this for initialization
	void Start () {

        
        gameUpdater = GameObject.Find("Updater").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
