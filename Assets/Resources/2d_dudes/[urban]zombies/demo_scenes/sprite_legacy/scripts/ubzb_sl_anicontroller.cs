using UnityEngine;
using System.Collections;

public class ubzb_sl_anicontroller : MonoBehaviour {

	// ------------------------------------------------
	//
	//				[ how to use this class ]
	//
	//			< 1. animation play with animation name >
	//
	//			ubzb_sl_anicontroller.ani_player("animation name" , "order");
	//
	//			animation name = animation name what you want to play
	//			order = once -> wrapMode.once / roop -> wrapMode.roop / pingpong -> wrapMode.pingpong
	//
	//			<!> Comparison with before played animation and next play animation
	//			<!> If they are using same face -> Crossfade animation
	//			<!> Or if they are not same face -> Stop and play animation 
	//
	//			< 2. change face >
	//
	//			ubzb_ml_anicontroller.change_face("order");
	//
	//			order = normal / hurt / stop
	//
	//			change_face ( "normal" );  -> Face fixed to normal look
	//			change_face ( "hurt" );  -> Face fixed to hurt look
	//			change_face ( "stop" );  -> Stop Face fix
	//
	// ------------------------------------------------
	
	//ani_player(start)
	
	public void ani_player (string ani_name , string order)
	{
		bool next_facenormal = true;
		
		//find ani_name in ani_list_facehurt
		foreach (string check_name in ani_list_facehurt)
		{
			if( check_name == ani_name )
			{
				next_facenormal = false;	//If find same name in list next animation is hurt face animation
				break;
			}
		}
		
		
		if(order == "loop")
		{
			GetComponent<Animation>()[ani_name].wrapMode = WrapMode.Loop;
		}
		else if(order == "pingpong")
		{
			GetComponent<Animation>()[ani_name].wrapMode = WrapMode.PingPong;
		}
		else
		{
			GetComponent<Animation>()[ani_name].wrapMode = WrapMode.Once;
		}
		
		if(before_facenormal ==  next_facenormal)	//same face
		{
			GetComponent<Animation>().CrossFade( ani_name , 0.1f );
		}
		else   	// different face
		{
			GetComponent<Animation>().Play(ani_name);
		}
		
		before_facenormal = next_facenormal;	//save face
	}
	
	//ani_player(end)
	
	//Face chager(start)
	
	public void change_face (string order)
	{
		if(order == "normal")
		{
			//change face to normal face
			GetComponent<Animation>().Play("zb_face_normal");
		}
		else if(order == "hurt")
		{
			//change face to hurt face
			GetComponent<Animation>().Play("zb_face_hurt");
		}
		else
		{
			//Stop change face
			GetComponent<Animation>().Stop ("zb_face_normal");
			GetComponent<Animation>().Stop ("zb_face_hurt");
		}
	}
	
	//Face chager(end)
	
	//Data for animation (start)
	
	public string[] ani_list;				//character's animation name list ( normal face )
	public string[] ani_list_facehurt;		//character's animation name list ( hurt face )
	bool before_facenormal = true;					//save animation's face setting before played
	
	//Data for animation (end)
	
	//Data for change face (start)
	
	public Transform face_normal;			//normal face bone's Transform
	public Transform face_hurt;				//Hurt face bone's Transform
	
	//Data for change face (end)
	
	void Start () {
		
		//Setting animation name list for zombie_urban1 (start)
		
		ani_list = new string[26];
		
		ani_list[0] = "zb_attack_2h1";			//Dual wield attack animation type 1
		ani_list[1] = "zb_attack_2h2";			//Dual wield attack animation type 2
		ani_list[2] = "zb_attack_2h3";			//Dual wield attack animation type 3
		ani_list[3] = "zb_attack_2h4";			//Dual wield attack animation type 4
		
		ani_list[4] = "zb_attack_ih1";			//Inside hand attack animation type 1
		ani_list[5] = "zb_attack_ih2";			//Inside hand attack animation type 2
		ani_list[6] = "zb_attack_ih3";			//Inside hand attack animation type 3
		ani_list[7] = "zb_attack_ih4";			//Inside hand attack animation type 4
		
		ani_list[8] = "zb_attack_oh1";			//Outside hand attack animation type 1
		ani_list[9] = "zb_attack_oh2";			//Outside hand attack animation type 1
		ani_list[10] = "zb_attack_oh3";			//Outside hand attack animation type 1
		ani_list[11] = "zb_attack_oh4";			//Outside hand attack animation type 1
		
		ani_list[12] = "zb_dead1";				//Dead animation with normal face type 1
		ani_list[13] = "zb_dead2";				//Dead animation with normal face type 2						
		
		ani_list[14] = "zb_hurt1";				//Hurt animation with normal face type 1
		ani_list[15] = "zb_hurt2";				//Hurt animation with normal face type 2						
		ani_list[16] = "zb_hurt3";				//Hurt animation with normal face type 3						
		
		ani_list[17] = "zb_idle1";				//Idle animation type 1
		ani_list[18] = "zb_idle2";				//Idle animation type 2								
		
		ani_list[19] = "zb_shield1";			//Block with inside hand's shield animation type 1
		
		ani_list[20] = "zb_skill1";				//Using skill(buff,magic) animation type 1
		ani_list[21] = "zb_skill2";				//Using skill(buff,magic) animation type 2
		
		ani_list[22] = "zb_walk1";				//Walk animation type 1
		ani_list[23] = "zb_walk2";				//Walk animation type 2
		
		ani_list[24] = "zb_walk1_fast";			//Fast walk animation type 1
		ani_list[25] = "zb_walk2_fast";			//Fast walk animation type 2
		
		//Setting animation name list for zombie_urban1 (end)
		
		//Setting animation name list for zombie_urban1 with hurt face (start)
		
		ani_list_facehurt = new string[5];
		
		ani_list_facehurt[0] = "zb_dead1_facehurt";		//Dead animation with hurt face type 1
		ani_list_facehurt[1] = "zb_dead2_facehurt";		//Dead animation with hurt face type 2						
		
		ani_list_facehurt[2] = "zb_hurt1_facehurt";		//Hurt animation with hurt face type 1
		ani_list_facehurt[3] = "zb_hurt2_facehurt";		//Hurt animation with hurt face type 2						
		ani_list_facehurt[4] = "zb_hurt3_facehurt";		//Hurt animation with hurt face type 3	
		
		//Setting animation name list for zombie_urban1 with hurt face (end)
		
		//Find and setting face bones to change face (start)
		//must need "zb_face_normal" & "zb_face_hurt" already listed in animation list
		
		face_normal = transform.Find ("humanoids2d_bone/root/body/body_rot/head/face_normal");
		face_hurt = transform.Find ("humanoids2d_bone/root/body/body_rot/head/face_hurt");
		
		GetComponent<Animation>()["zb_face_normal"].AddMixingTransform(face_normal);
		GetComponent<Animation>()["zb_face_normal"].AddMixingTransform(face_hurt);
		GetComponent<Animation>()["zb_face_normal"].blendMode = AnimationBlendMode.Blend;
		GetComponent<Animation>()["zb_face_normal"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>()["zb_face_normal"].layer = 30;		
		
		GetComponent<Animation>()["zb_face_hurt"].AddMixingTransform(face_normal);
		GetComponent<Animation>()["zb_face_hurt"].AddMixingTransform(face_hurt);
		GetComponent<Animation>()["zb_face_hurt"].blendMode = AnimationBlendMode.Blend;
		GetComponent<Animation>()["zb_face_hurt"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>()["zb_face_hurt"].layer = 30;						
		
		//Find and setting face bones to change face (end)
		
	}

}
