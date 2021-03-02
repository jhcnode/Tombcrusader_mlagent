using UnityEngine;
using System.Collections;

public class DepthManager : MonoBehaviour {
    public int RenderQueue = 3000;
    public Shader shader;


	// Use this for initialization
	void Start () {
        //if (gameObject.GetComponent<SkinnedMeshRenderer>()!=null)
        //gameObject.GetComponent<SkinnedMeshRenderer>().sharedMaterial.renderQueue = RenderQueue;
        //else if (gameObject.GetComponent<UI2DSprite>()!=null)
        //{
        //    Material material = new Material(shader);

        //    gameObject.GetComponent<UI2DSprite>().material = material;

        //    gameObject.GetComponent<UI2DSprite>().material.renderQueue = RenderQueue;

        //}
        //else if (gameObject.GetComponent<SpriteRenderer>() != null)
        //{
        //    gameObject.GetComponent<SpriteRenderer>().material.renderQueue = RenderQueue;

        //}
	
	}
	
	// Update is called once per frame
	void Update () {
        //if (gameObject.GetComponent<SkinnedMeshRenderer>() != null)
        //{
        //    gameObject.GetComponent<SkinnedMeshRenderer>().sharedMaterial.renderQueue = RenderQueue;
        //}
        //else if (gameObject.GetComponent<UI2DSprite>() != null)
        //{
        //    Material material = new Material(shader);

        //    gameObject.GetComponent<UI2DSprite>().material = material;

        //    gameObject.GetComponent<UI2DSprite>().material.renderQueue = RenderQueue;

        //}
        //else if (gameObject.GetComponent<SpriteRenderer>() != null)
        //{
        //    gameObject.GetComponent<SpriteRenderer>().material.renderQueue = RenderQueue;

        //}
	}
}
