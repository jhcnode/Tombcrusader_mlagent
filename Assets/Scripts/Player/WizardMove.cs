using UnityEngine;
using System.Collections;

public class WizardMove : MonoBehaviour {
    public float speed = 1.0f;
    private Vector2 currPos;
    private Vector2 destination;
    private GameUpdater gameUpdater;
    private float MaxDistance=0;
    private Animation animation;
    private bool actTrigger = false;


    public void SetDestination(Vector2 dest)
    {
        destination = dest;
    }
    public void resetMaxDistance()
    {
        MaxDistance = 0;
    }

    public void Move(GameObject zombie)
    {
        currPos = gameObject.transform.position;
        Vector2 zombiePos = new Vector2(currPos.x, zombie.transform.position.y);
        if (Vector2.Distance(currPos, zombiePos) > MaxDistance)
        {
            MaxDistance = Vector2.Distance(currPos, zombiePos);
            SetDestination(zombiePos);
        }

        actTrigger = true;

        

    }

	// Use this for initialization
	void Start () {
        //gameUpdater = GameObject.Find("Updater").transform.GetComponent<GameUpdater>();
        destination = gameObject.transform.position;
        animation = gameObject.transform.GetComponentInChildren<Animation>();

	}
	
	// Update is called once per frame
	void Update () {

        currPos = gameObject.transform.position;
        if (Vector2.Distance(currPos, destination) > 0.1f)
        {
            Vector2 dir = destination - currPos;
            dir.Normalize();
            float z = gameObject.transform.position.z;
            Vector2 pos = currPos + dir * Time.deltaTime * speed;
            gameObject.transform.position = new Vector3(pos.x, pos.y, z);

            if (animation.IsPlaying("zb_walk1") == false)
            {
                animation["zb_walk1"].speed = 5;
                animation.Play("zb_walk1");
            }




        }
        else
        {

            float z = gameObject.transform.position.z;
            gameObject.transform.position = new Vector3(destination.x, destination.y, z);
            if (actTrigger == true)
            {
                animation["zb_attack_2h4"].speed = 2;
                animation.Play("zb_attack_2h4");
                actTrigger = false;
            }
            else
            {
                if (animation.IsPlaying("zb_attack_2h4") == false)
                {
                    if (animation.IsPlaying("zb_idle1") == false)
                    {
                        animation.Stop();
                        animation["zb_idle1"].speed = 1;
                        animation.Play("zb_idle1");
                    }
                }
            }

        }


    }
}
