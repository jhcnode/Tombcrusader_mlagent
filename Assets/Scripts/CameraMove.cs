using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
    public enum Shake {MOVE,ROTATION,STOP}
    private Shake shake = Shake.STOP;
    public float speed = 8;
    public float gradient = 0.1f;
    private Vector2 destination;
    private Vector2 currPos;
    private float vibaration;





   
    public void SetDestination(Vector2 dest)
    {
        destination = dest;
    }


    public void SetShakeState(Shake s)
    {
        shake = s;
        vibaration = 0;
    }


	// Use this for initialization
	void Start () {
        vibaration = 0;


	}
	
	// Update is called once per frame
	void Update () {
        if (shake == Shake.MOVE)
        {
            vibaration += Time.deltaTime * speed*10;
            float det = Mathf.Exp(-gradient * vibaration);
            float newX = (0.1f - 0.1f * vibaration) * det * Mathf.Sin(vibaration);
            gameObject.transform.position = new Vector3(newX, 0,  gameObject.transform.position.z);
            if (det <= 0)
            {
                newX = 0;
                gameObject.transform.position = new Vector3(newX, 0, gameObject.transform.position.z);
                shake = Shake.STOP;
            }
        }
        else if (shake == Shake.ROTATION)
        {
            vibaration += Time.deltaTime * speed*10;
            float det = Mathf.Exp(-gradient * vibaration);
            float newZ = (2 -2*vibaration) * det * Mathf.Sin(vibaration);
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.Rotate(0, 0, newZ);
            if (det <= 0)
            {
                newZ = 0;
                gameObject.transform.rotation = Quaternion.identity;
                shake = Shake.STOP;
            }
        }
       

     
	}
}
