using UnityEngine;
using System.Collections;

public class ubzb_sl_skinchanger : MonoBehaviour {

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
		
		if( body_skin.Length <= body_no )
		{
			body_no = body_skin.Length - 1;
		}
			my_anim.Play(body_skin[body_no]);
			my_anim.SetLayerWeight(2,1f);			
		
		if( out_arm_weapon_skin.Length <= outside_weapon_no )
		{
			my_anim.Play("oa_empty_skin");
			my_anim.SetLayerWeight(3,1f);
		}
		else
		{
			my_anim.Play(out_arm_weapon_skin[outside_weapon_no]);
			my_anim.SetLayerWeight(3,1f);
		}
		
		if( in_arm_weapon_skin.Length <= inside_weapon_no )
		{
			my_anim.Play("ia_empty_skin");
			my_anim.SetLayerWeight(4,1f);
		}
		else
		{
			my_anim.Play(in_arm_weapon_skin[inside_weapon_no]);
			my_anim.SetLayerWeight(4,1f);
		}
		
		if( in_arm_shield_skin.Length <= inside_shield_no )
		{
			my_anim.Play("sh_empty_skin");
			my_anim.SetLayerWeight(5,1f);
		}
		else
		{	
			my_anim.Play(in_arm_shield_skin[inside_shield_no]);
			my_anim.SetLayerWeight(5,1f);
		}
		
	}
	
	// ------------------------------------------------
	// 				skin chager!!	(end)
	// ------------------------------------------------
	
	//Data for changing skins (start)

	public string[] body_skin;				//character's body skin name
	public string[] out_arm_weapon_skin;	//character's weapon skin name for outside arm
	public string[] in_arm_weapon_skin;		//character's weapon skin name for inside arm
	public string[] in_arm_shield_skin;		//character's shield skin name for inside arm

	//Data for changing skins (end)

	//Data for animator (start)
	
	private Animator my_anim;
	
	//Data for animator (end)

	void Start()
	{
		//Setting animator (start)
		
		my_anim = GetComponent<Animator>();
		
		//Setting animator (end)

		//Setting skin animations (start)

		body_skin = new string[8];
		body_skin[0] = "zombie1_skin";
		body_skin[1] = "zombie2_skin";
		body_skin[2] = "zombie3_skin";
		body_skin[3] = "zombie4_skin";
		body_skin[4] = "zombie5_skin";
		body_skin[5] = "zombie6_skin";
		body_skin[6] = "zombie7_skin";
		body_skin[7] = "zombie8_skin";

		out_arm_weapon_skin = new string[8];
		out_arm_weapon_skin[0] = "oa_wood_club_skin";
		out_arm_weapon_skin[1] = "oa_wood_axe_skin";
		out_arm_weapon_skin[2] = "oa_umbrella_skin";
		out_arm_weapon_skin[3] = "oa_steel_stand_skin";
		out_arm_weapon_skin[4] = "oa_steel_axe_skin";
		out_arm_weapon_skin[5] = "oa_fire_axe_skin";
		out_arm_weapon_skin[6] = "oa_chain_saw_skin";
		out_arm_weapon_skin[7] = "oa_black_sword_skin";

		in_arm_weapon_skin = new string[8];
		in_arm_weapon_skin[0] = "ia_wood_club_skin";
		in_arm_weapon_skin[1] = "ia_wood_axe_skin";
		in_arm_weapon_skin[2] = "ia_umbrella_skin";
		in_arm_weapon_skin[3] = "ia_steel_stand_skin";
		in_arm_weapon_skin[4] = "ia_steel_axe_skin";
		in_arm_weapon_skin[5] = "ia_fire_axe_skin";
		in_arm_weapon_skin[6] = "ia_chain_saw_skin";
		in_arm_weapon_skin[7] = "ia_black_sword_skin";

		in_arm_shield_skin = new string[2];
		in_arm_shield_skin[0] = "sh_basket_cover_skin";
		in_arm_shield_skin[1] = "sh_green_bucket_skin";

		//Setting skin animations (end)

	}

}
