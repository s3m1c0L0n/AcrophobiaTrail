﻿using UnityEngine;
using System.Collections;
//using Artngame.PDM;

namespace Artngame.SKYMASTER {

public class MoveItemOverGroundSM : MonoBehaviour {
	
	void Start () {

		if(HERO !=null){
		CAST_FORWARD_VEC=HERO.transform.forward;
		}else{CAST_FORWARD_VEC=this.gameObject.transform.forward;}
	}
	
	Vector3 CAST_FORWARD_VEC;
	public GameObject HERO;

	public float PROJ_SPEED = 5;

	public float Speed_x;
	public float Speed_z;
	public float Dist_Above_Terrain = 1;

	void LateUpdate () {

		float find_x =this.transform.position.x;
		float find_z =this.transform.position.z;

		float find_y = Terrain.activeTerrain.SampleHeight(new Vector3(find_x,0,find_z))+Dist_Above_Terrain+ Terrain.activeTerrain.transform.position.y;

		this.transform.position  =  this.transform.position+CAST_FORWARD_VEC * Time.deltaTime *PROJ_SPEED;
		this.transform.position  = new Vector3(this.transform.position.x,find_y ,this.transform.position.z);


	}
}
}