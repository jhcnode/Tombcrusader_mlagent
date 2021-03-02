using UnityEngine;
using System.Collections;

public class ubzb_mm_anicontroller : MonoBehaviour {

	// ------------------------------------------------
	//
	//				[ how to use this class ]
	//
	//			< 1. animation play with animation name >
	//
	//			ubzb_mm_anicontroller.ani_player("animation name" , "aimation changing time");
	//
	//			animation name = animation name what you want to play
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
	
	public void ani_player (string ani_name , float c_time)
	{

		my_anim.CrossFade( ani_name , c_time );

	}
	
	//ani_player(end)
	
	//Face chager(start)
	
	public void change_face (string order)
	{
		if(order == "normal")
		{
			//change face to normal face
			my_anim.SetLayerWeight(1,1f);
			my_anim.Play ("zb_face_normal");
		}
		else if(order == "hurt")
		{
			//change face to hurt face
			my_anim.SetLayerWeight(1,1f);
			my_anim.Play ("zb_face_hurt");
		}
		else
		{
			//Stop change face
			my_anim.SetLayerWeight(1,0f);
		}
	}
	
	//Face chager(end)
	
	//Data for animation (start)
	
	public string[] ani_list;				//character's animation name list ( normal face )

	//Data for animation (end)

	//Data for animator (start)

	private Animator my_anim;

	//Data for animator (end)
	
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

		//Setting animator (start)

		my_anim = GetComponent<Animator>();
		
		//Setting animator (end)
	}

}
