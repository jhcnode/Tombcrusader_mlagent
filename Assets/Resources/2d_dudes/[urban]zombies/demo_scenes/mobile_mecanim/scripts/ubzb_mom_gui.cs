using UnityEngine;
using System.Collections;

public class ubzb_mom_gui : MonoBehaviour {
	
	void OnGUI()
	{
		GUI.Box(new Rect(5,5,190,50), "");
		GUI.Box(new Rect(5,5,190,50), "ZOMBIE 1");
		if(GUI.Button(new Rect(10,25,90,20), "Skin")) {  next_body_skin(0);  }
		if(GUI.Button(new Rect(100,25,90,20), "Ani")) {  next_animation(0);  }
		
		GUI.Box(new Rect(5,80,190,50), "");
		GUI.Box(new Rect(5,80,190,50), "ZOMBIE 5");
		if(GUI.Button(new Rect(10,100,90,20), "Skin")) {  next_body_skin(4);  }
		if(GUI.Button(new Rect(100,100,90,20), "Ani")) {  next_animation(4);  }
		
		GUI.Box(new Rect(205,5,190,50), "");
		GUI.Box(new Rect(205,5,190,50), "ZOMBIE 2");
		if(GUI.Button(new Rect(210,25,90,20), "Skin")) {  next_body_skin(1);  }
		if(GUI.Button(new Rect(300,25,90,20), "Ani")) {  next_animation(1);  }
		
		GUI.Box(new Rect(205,80,190,50), "");
		GUI.Box(new Rect(205,80,190,50), "ZOMBIE 6");
		if(GUI.Button(new Rect(210,100,90,20), "Skin")) {  next_body_skin(5);  }
		if(GUI.Button(new Rect(300,100,90,20), "Ani")) {  next_animation(5);  }
		
		GUI.Box(new Rect(405,5,190,50), "");
		GUI.Box(new Rect(405,5,190,50), "ZOMBIE 3");
		if(GUI.Button(new Rect(410,25,90,20), "Skin")) {  next_body_skin(2);  }
		if(GUI.Button(new Rect(500,25,90,20), "Ani")) {  next_animation(2);  }
		
		GUI.Box(new Rect(405,80,190,50), "");
		GUI.Box(new Rect(405,80,190,50), "ZOMBIE 7");
		if(GUI.Button(new Rect(410,100,90,20), "Skin")) {  next_body_skin(6);  }
		if(GUI.Button(new Rect(500,100,90,20), "Ani")) {  next_animation(6);  }
		
		GUI.Box(new Rect(605,5,190,50), "");
		GUI.Box(new Rect(605,5,190,50), "ZOMBIE 4");
		if(GUI.Button(new Rect(610,25,90,20), "Skin")) {  next_body_skin(3);  }
		if(GUI.Button(new Rect(700,25,90,20), "Ani")) {  next_animation(3);  }
		
		GUI.Box(new Rect(605,80,190,50), "");
		GUI.Box(new Rect(605,80,190,50), "ZOMBIE 8");
		if(GUI.Button(new Rect(610,100,90,20), "Skin")) {  next_body_skin(7);  }
		if(GUI.Button(new Rect(700,100,90,20), "Ani")) {  next_animation(7);  }
		
		GUI.Box(new Rect(5,155,390,35), "");
		GUI.Box(new Rect(5,155,390,35), "");
		GUI.Label(new Rect(12,162,85,30), "Change Face");
		if(GUI.Button(new Rect(100,160,90,25), "Face Normal")) {  change_face("normal");   }
		if(GUI.Button(new Rect(200,160,90,25), "Face Hurt")) {  change_face("hurt");  }
		if(GUI.Button(new Rect(300,160,90,25), "Stop")) {  change_face("stop");  }
		
		GUI.Box(new Rect(405,155,390,35), "");
		GUI.Box(new Rect(405,155,390,35), "");
		GUI.Label(new Rect(412,162,100,30), "Change Camera");
		if(GUI.Button(new Rect(510,160,90,25), "Camera 1")) { change_cam(1); }
		if(GUI.Button(new Rect(605,160,90,25), "Camera 2")) { change_cam(2); }
		if(GUI.Button(new Rect(700,160,90,25), "Camera 3")) { change_cam(3); }
		
	}
	
	//change face (start)
	void change_face (string order)
	{
		for(int i = 0; i < anichanger.Length ; i++)
		{
			anichanger[i].change_face(order);
		}
	}
	//change face (end)
	
	//change next body skin (start)
	void next_body_skin (int ch_no)
	{
		int next_body_no = now_body_no [ch_no] + 1;
		
		if(next_body_no >= skinchanger[ch_no].body_mat.Length)
		{
			next_body_no = 0;
		}
		
		skinchanger[ch_no].change_skin( next_body_no );
		now_body_no[ch_no] = next_body_no;
	}
	//change next body skin (end)
	
	//change next animation (start)
	void next_animation (int ch_no)
	{
		int next_ani_no = now_animation_no[ch_no] + 1;
		
		int temp_ani_no = 0;
		string temp_ani_name;
		
		if(next_ani_no >= anichanger[ch_no].ani_list.Length )
		{
			next_ani_no = 0;
		}
		
		temp_ani_no = next_ani_no;
		temp_ani_name = anichanger[ch_no].ani_list[temp_ani_no];
		
		anichanger [ch_no].ani_player ( temp_ani_name, 0.1f );
		now_animation_no[ch_no] = next_ani_no;
	}
	//change next animation (end)
	
	//change camera to view (start)
	void change_cam ( int num )
	{
		if( num == 1 )
		{
			demo_cam[0].enabled = true;
			demo_cam[1].enabled = false;
			demo_cam[2].enabled = false;
		}
		else if ( num == 2 )
		{
			demo_cam[0].enabled = false;
			demo_cam[1].enabled = true;
			demo_cam[2].enabled = false;
		}
		else if ( num == 3 )
		{
			demo_cam[0].enabled = false;
			demo_cam[1].enabled = true;
			demo_cam[2].enabled = true;
		}
		
	}
	//change camera to view (end)
	
	public int[] now_body_no;
	public int[] now_animation_no;
	public ubzb_mom_skinchanger[] skinchanger;
	public ubzb_mom_anicontroller[] anichanger;
	
	public Camera[] demo_cam;
	
}
