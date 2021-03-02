using UnityEngine;
using System.Collections;

public class ubzb_mol_skinchanger : MonoBehaviour {
	
	// ------------------------------------------------
	//
	//				[ how to use ]
	//
	//			1. change skin
	//
	//			change_skin ( body_no );
	//
	//          mobile character only use 1 material for 1 draw call per character
	//			body + inarm weapon + outarm weapon + shield = 1 material
	//
	//			body_no  -> body skins number ( 0 ~ 15 ) / 16 or bigger = 15
	//
	// ------------------------------------------------
	
	// ------------------------------------------------
	// 				skin chager!!	(start)
	// ------------------------------------------------
	
	public void change_skin( int body_no )
	{
		
		Material[] temp_mat = new Material[1];
		
		Material temp_body_mat;

		
		if( body_mat.Length <= body_no )
		{
			body_no = body_mat.Length - 1;
		}
		
		temp_body_mat = body_mat[body_no];
		
		float temp_size = body_size [body_no];
		if(temp_size <= 0){temp_size = 1;}
		
		my_transform.localScale = new Vector3(temp_size,temp_size,temp_size);

		temp_mat[0] = temp_body_mat;

		my_skin_render.materials = temp_mat;
		
	}
	
	// ------------------------------------------------
	// 				skin chager!!	(end)
	// ------------------------------------------------
	
	//Data for changing skins (start)
	
	Transform my_transform;					//This character's transform
	
	public Material[] body_mat;				//character's body skin materials
	public float[] body_size;				//character's body size for each skin( 1 is basic )

	public SkinnedMeshRenderer my_skin_render;	//character's SkinnedMeshRenderer
	
	//Data for changing skins (end)
	
	void Start()
	{
		my_transform = transform;
	}
	
}
