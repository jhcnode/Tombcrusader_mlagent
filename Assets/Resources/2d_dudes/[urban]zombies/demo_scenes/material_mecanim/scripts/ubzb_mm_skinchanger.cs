using UnityEngine;
using System.Collections;

public class ubzb_mm_skinchanger : MonoBehaviour {

	// ------------------------------------------------
	//
	//				[ how to use ]
	//
	//			1. change skin
	//
	//			change_skin ( body_no , outside_weapon_no , inside_weapon_no , inside_shield_no );
	//
	//			body_no  -> body skins number ( 0 ~ 7 ) / 8 or bigger = 7
	//			outside_weapon_no -> outside arms's weapon ( 0 ~ 7 ) / 8 or bigger = no draw
	//			inside_weapon_no  -> inside arms's weapon ( 0 ~ 7 ) / 8 or bigger = no draw
	//			inside_shield_no  -> inside arms's shield ( 0 ~ 1 ) / 2 or bigger = no draw
	//
	// ------------------------------------------------
	
	// ------------------------------------------------
	// 				skin chager!!	(start)
	// ------------------------------------------------
	
	public void change_skin( int body_no , int outside_weapon_no , int inside_weapon_no , int inside_shield_no )
	{
		
		Material[] temp_mat = new Material[7];
		
		Material temp_body_mat;
		Material temp_out_arm_weapon_mat;
		Material temp_in_arm_weapon_mat;
		Material temp_in_arm_shield_mat;
		
		
		if( body_mat.Length <= body_no )
		{
			body_no = body_mat.Length - 1;
		}
		
		temp_body_mat = body_mat[body_no];
		
		float temp_size = body_size [body_no];
		if(temp_size <= 0){temp_size = 1;}
		
		my_transform.localScale = new Vector3(temp_size,temp_size,temp_size);
		
		if( out_arm_weapon_mat.Length <= outside_weapon_no )
		{
			temp_out_arm_weapon_mat = empty_mat;
		}
		else
		{
			temp_out_arm_weapon_mat = out_arm_weapon_mat[outside_weapon_no];
		}
		
		if( in_arm_weapon_mat.Length <= inside_weapon_no )
		{
			temp_in_arm_weapon_mat = empty_mat;
		}
		else
		{	
			temp_in_arm_weapon_mat = in_arm_weapon_mat[inside_weapon_no];
		}
		
		if( in_arm_shield_mat.Length <= inside_shield_no )
		{
			temp_in_arm_shield_mat = empty_mat;
		}
		else
		{	
			temp_in_arm_shield_mat = in_arm_shield_mat[inside_shield_no];
		}
		
		temp_mat[0] = temp_body_mat;
		temp_mat[1] = temp_in_arm_weapon_mat;
		temp_mat[2] = temp_body_mat;
		temp_mat[3] = temp_in_arm_shield_mat;
		temp_mat[4] = temp_body_mat;
		temp_mat[5] = temp_out_arm_weapon_mat;
		temp_mat[6] = temp_body_mat;
		
		my_skin_render.materials = temp_mat;
		
	}
	
	// ------------------------------------------------
	// 				skin chager!!	(end)
	// ------------------------------------------------
	
	//Data for changing skins (start)
	
	Transform my_transform;					//This character's transform
	
	public Material[] body_mat;				//character's body skin materials
	public float[] body_size;				//character's body size for each skin( 1 is basic )
	
	public Material[] out_arm_weapon_mat;	//character's weapon skin materials for outside arm
	public Material[] in_arm_weapon_mat;	//character's weapon skin materials for inside arm
	public Material[] in_arm_shield_mat;	//character's shield skin materials for inside arm
	public Material empty_mat;				//character's empty skin materials for nodraw something
	
	public SkinnedMeshRenderer my_skin_render;	//character's SkinnedMeshRenderer
	
	//Data for changing skins (end)
	
	void Start()
	{
		my_transform = transform;
	}

}
