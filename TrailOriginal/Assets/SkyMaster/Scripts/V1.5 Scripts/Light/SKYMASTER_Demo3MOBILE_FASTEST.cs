using UnityEngine;
using System.Collections;

namespace Artngame.SKYMASTER {

public class SKYMASTER_Demo3MOBILE_FASTEST : MonoBehaviour {

	#pragma warning disable 414


		//VOLUME CLOUDS
		//public Transform target ;
		public float damping = 6.0f;
		public bool smooth = true;
		//bool enable_lookat=true;
		//Circle_Around_ParticleSKYMASTER Sun_rotator;
		//Circle_Around_ParticleSKYMASTER Cam_rotator;
		//MouseLookSKYMASTER MouseLOOK;
		VolumeClouds_SM Clouds;
		VolumeClouds_SM Clouds_ORIGIN;
		//ParticleEmitter Emitter_ORIGIN;	 //v3.4.6	
		Light SunLight;		
		//public float Sun_time_start = 14.43f;	
		//public Transform SUN;
		//public bool HUD_ON=true;
		//public Transform Clouds_top;		
		//float Dome_rot;		
		float SunLight_r;
		float SunLight_g;
		float SunLight_b;
		float Camera_up;
		float Sun_up;		
		float sun_rot_speed;
		float cam_rot_speed;

		int Cloud_divider;
		float Cloud_bed_height;
		float Cloud_bed_width;
		GameObject Cloud_instance;
		float cloud_max_scale;
		int cloud_max_particle_size;
		
		bool Rot_clouds = false;

		void LateUpdate () {
			
//			if(MouseLOOK.enabled){
//				
//				enable_lookat = false;
//				
//			}else{
//				enable_lookat = true;
//			}
//			
//			if (target & enable_lookat) {
//				if (smooth)
//				{
//					// Look at and dampen the rotation
//					Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
//					transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
//				}
//				else
//				{
//					// Just lookat
//					transform.LookAt(target);
//				}
//			}
			
			//ROT CLOUDS
			if(Rot_clouds){
				if(Clouds!=null){
					Clouds.wind.x = 2*Mathf.Cos(Time.fixedTime*0.1f);
					Clouds.wind.z = 2*Mathf.Sin(Time.fixedTime*0.1f);
					Clouds.speed = 2f;
					Clouds.multiplier = 2;
				}
			}else{
				Clouds.speed = 0.5f;
				Clouds.multiplier = 1;
			}
			
			
		}


	void Start () {

		if(SKYMASTER_OBJ!=null){
				SUNMASTER = SKYMASTER_OBJ.GetComponent(typeof(SkyMasterManager)) as SkyMasterManager;
		}
		SPEED = SUNMASTER.SPEED;

			SUNMASTER.Seasonal_change_auto = false;

			//VOLUME CLOUDS
			//Sun_rotator = SUN.GetComponent(typeof(Circle_Around_ParticleSKYMASTER)) as Circle_Around_ParticleSKYMASTER;
			//Cam_rotator = Camera.main.gameObject.GetComponent(typeof(Circle_Around_ParticleSKYMASTER)) as Circle_Around_ParticleSKYMASTER;
			//MouseLOOK = Camera.main.gameObject.GetComponent(typeof(MouseLookSKYMASTER)) as MouseLookSKYMASTER;
			Cloud_instance = (GameObject)Instantiate(Clouds_top.gameObject,Clouds_top.transform.position, Quaternion.identity);
			
			Cloud_instance.SetActive(true);
			Clouds_ORIGIN = Clouds_top.gameObject.GetComponent(typeof(VolumeClouds_SM)) as VolumeClouds_SM;
			//Emitter_ORIGIN =  Clouds_top.gameObject.GetComponent(typeof(ParticleEmitter)) as ParticleEmitter;
			Cloud_instance.SetActive(false);
			
			Cloud_instance.SetActive(true);
			
			if(Cloud_instance!=null){
				Clouds = Cloud_instance.GetComponent(typeof(VolumeClouds_SM)) as VolumeClouds_SM;
				
			}else{
				Debug.Log ("AAA");
			}
		
	}

	

	public float Sun_time_start = 14.43f;	//at this time, rotation of sunsystem must be 62 (14, -1.525879e-05, -1.525879e-05 WORKS !!)

	public GameObject SKYMASTER_OBJ;

		SkyMasterManager SUNMASTER;

	public GameObject SUN;
		public GameObject TREES;

	public bool HUD_ON=true;

	float HDR=0.8f;
	float Esun=22;
	float Kr=0.0025f;
	float Km=0.0015f;
	float GE=-0.96f;
	float SPEED = 0.01f;

	float fSamples = 3;
	float fScaleDepth = 0.5f;

	bool set_sun_start=false;
	float Ring_factor=0;

	Vector3 CURRENT_Force_color = new Vector3(0.65f,0.52f,0.475f);
	float Coloration = 0.28f;
	Vector4 TintColor = new Vector4(0,0,0,0);

	bool enable_controls=false;

	public GameObject Clouds_top;
	public GameObject Clouds_bottom;
	public GameObject Flat_Clouds_top;
	public GameObject Flat_Clouds_bottom;
	public GameObject Cloud_Dome;
	public GameObject Cloud_Rays;
	public GameObject Cloud_Static;

	float Dome_rot = 0;

	public GameObject Tornado1;
	public GameObject Tornado2;

		public GameObject Butterflies;
		public GameObject FreezeEffect;
		public GameObject LightningStorm;

		public bool Auto_Season_Cycle=false;
		float Cycle_speed = 1500;
		bool GI_controls_on = false;

	void OnGUI() {

			float BOX_WIDTH = 100;float BOX_HEIGHT = 30;
			float BASE_X = -5;
			float BASE1= 60;

			//VOLUME CLOUDS

			if(enable_controls){
			SunLight = SUN.GetComponent<Light>();
			float widthS = Screen.currentResolution.width;
			
			widthS = Camera.main.pixelWidth;

			
			//Destroy (
			GUI.TextArea( new Rect(widthS - (BASE_X+11*15), 1*BASE1+(3.5f*40)+7-30, 180, 30),"Cloud Centers = "+Clouds_ORIGIN.divider);
			Cloud_divider = (int)GUI.HorizontalSlider(new Rect(widthS - (BASE_X+11*15), 1*BASE1+(3.5f*40)+7, 100, 30),Clouds_ORIGIN.divider,2,45);
			
			GUI.TextArea( new Rect(widthS - (BASE_X+11*15), 1*BASE1+(3.5f*40)+7-30-60, 180, 30),"Cloud Bed size = "+Clouds_ORIGIN.max_bed_corner);
			Cloud_bed_width = GUI.HorizontalSlider(new Rect(widthS - (BASE_X+11*15), 1*BASE1+(3.5f*40)+7-60, 100, 30),Clouds_ORIGIN.max_bed_corner,300,1500);
			
			GUI.TextArea( new Rect(widthS - (BASE_X+11*15), 1*BASE1+(3.5f*40)+7-30-60-60, 180, 30),"Cloud Bed Height = "+Clouds_ORIGIN.cloud_bed_heigh);
			Cloud_bed_height = GUI.HorizontalSlider(new Rect(widthS - (BASE_X+11*15), 1*BASE1+(3.5f*40)+7-60-60, 100, 30),Clouds_ORIGIN.cloud_bed_heigh,500,1000);
			
			GUI.TextArea( new Rect(widthS - (BASE_X+11*15), 1*BASE1+(3.5f*40)+7+30, 180, 30),"Cloud Max size = "+Clouds_ORIGIN.cloud_max_scale);
			cloud_max_scale = GUI.HorizontalSlider(new Rect(widthS - (BASE_X+11*15), 1*BASE1+(3.5f*40)+7+30+30, 100, 30),Clouds_ORIGIN.cloud_max_scale,1,4);
			
		//	
		//	GUI.TextArea( new Rect(widthS - (BASE_X+11*15), 1*BASE1+(3.5f*40)+7+30+30+30, 180, 30),"Cloud Particle size = "+Emitter_ORIGIN.maxSize);
			//cloud_max_particle_size = (int)GUI.HorizontalSlider(new Rect(widthS - (BASE_X+11*15), 1*BASE1+(3.5f*40)+7+30+30+30+30, 100, 30),Emitter_ORIGIN.maxSize,200,300);
			
			GUI.TextArea( new Rect(widthS - (BASE_X+11*15), 1*BASE1+(3.5f*40)+7+30+30+30+30+30, 170, 40),"Recreate button will renew the clouds");
			
			if (GUI.Button(new Rect(widthS - (BASE_X+11*15), 1*BASE1+(3.5f*40)+7+30+30+30+40+60, 170, 40), "Rotate Clouds")){
				if(Rot_clouds){
					Rot_clouds = false;
				}else{
					Rot_clouds = true;
				}
			}

				//RESTART EFFECT !!!!
				if (GUI.Button(new Rect(widthS - (BASE_X+11*15),40+ 1*BASE1+(3.5f*40)+7+30+30+30+40+60, 170, 40), "Recreate Clouds")){
					Destroy (Cloud_instance);
					//Object INSTA = Instantiate(Clouds_top);
					
					Clouds_ORIGIN.max_bed_corner = Cloud_bed_width;
					Clouds_ORIGIN.min_bed_corner = -Cloud_bed_width;
					Clouds_ORIGIN.cloud_bed_heigh = Cloud_bed_height;
					Clouds_ORIGIN.divider = Cloud_divider;
					Clouds_ORIGIN.cloud_max_scale = cloud_max_scale;
					//Emitter_ORIGIN.maxSize = cloud_max_particle_size;
					
					Cloud_instance = (GameObject)Instantiate(Clouds_top.gameObject, Clouds_top.transform.position, Quaternion.identity);
					Cloud_instance.SetActive(true);
					Clouds = Cloud_instance.GetComponent(typeof(VolumeClouds_SM)) as VolumeClouds_SM;				
				}
			
			Clouds_ORIGIN.max_bed_corner = Cloud_bed_width;
			Clouds_ORIGIN.min_bed_corner = -Cloud_bed_width;
			Clouds_ORIGIN.cloud_bed_heigh = Cloud_bed_height;
			Clouds_ORIGIN.divider = Cloud_divider;
			Clouds_ORIGIN.cloud_max_scale = cloud_max_scale;
			//Emitter_ORIGIN.maxSize = cloud_max_particle_size;
			
			}
			// END VOLUME CLOUDS







		if(SUNMASTER.Current_Time!=Sun_time_start & !set_sun_start){
				//SUNMASTER.Current_Time=Sun_time_start;
			set_sun_start=true;
		}

			if(SUNMASTER.Season == 1){
				if(!Butterflies.activeInHierarchy){
					Butterflies.SetActive(true);
				}
			}else{
				if(Butterflies.activeInHierarchy){
					Butterflies.SetActive(false);
				}
			} 

			GI_controls_on = false;
		if(GI_controls_on){
				//let GI controller take over GUI
		}
		else{
					
				if(!enable_controls){
					if (GUI.Button(new Rect(1*BOX_WIDTH+10, BOX_HEIGHT+60+30+30, BOX_WIDTH, 30), "Cycle Seasons")){
						if(!Auto_Season_Cycle){
							Auto_Season_Cycle = true;
							SUNMASTER.SPEED = 2431.818f;
							SUNMASTER.Seasonal_change_auto = true;
						}else{
							Auto_Season_Cycle = false;
							SUNMASTER.SPEED = 35;
							SUNMASTER.Seasonal_change_auto = false;
						}
					}
				}
				if(Auto_Season_Cycle){
					Cycle_speed = GUI.HorizontalSlider(new Rect(5*BOX_WIDTH+10, BOX_HEIGHT+80+30, BOX_WIDTH, 30),Cycle_speed,50,2431.818f);
					SUNMASTER.SPEED = Cycle_speed;
				}

				//Auto_Season_Cycle=false;
			if(!Auto_Season_Cycle){

					if(!enable_controls){
						if (GUI.Button(new Rect(1*BOX_WIDTH+10, BOX_HEIGHT+60+30, BOX_WIDTH, 30), "Sunny")){
							SUNMASTER.Weather = SkyMasterManager.Weather_types.Sunny;
							SUNMASTER.On_demand = true;
						}
					}
		
					if (GUI.Button(new Rect(2*BOX_WIDTH+10, BOX_HEIGHT-0, BOX_WIDTH, 30), "Cloudy")){
						SUNMASTER.Weather = SkyMasterManager.Weather_types.Cloudy;
						SUNMASTER.On_demand = true;
					}

					if (GUI.Button(new Rect(2*BOX_WIDTH+10, BOX_HEIGHT+30, BOX_WIDTH, 30), "Heavy Rain")){
						SUNMASTER.Weather = SkyMasterManager.Weather_types.FlatClouds;
						SUNMASTER.On_demand = true;
					}

		string Season_current= "Spring";
			if(SUNMASTER.Season == 2){
				Season_current= "Summer";
			}
			if(SUNMASTER.Season == 3){
				Season_current= "Autumn";
			}
			if(SUNMASTER.Season == 4){
				Season_current= "Winter";
			}
		GUI.TextArea(new Rect(2*BOX_WIDTH+10-(0/2), BOX_HEIGHT+60+30, BOX_WIDTH/1, 25),Season_current);
		if (GUI.Button(new Rect(2*BOX_WIDTH+10, BOX_HEIGHT+60, BOX_WIDTH, 30), "Cycle Season")){

				if(SUNMASTER.Season ==0){
					SUNMASTER.Season = 2;
				}else{
					SUNMASTER.Season = SUNMASTER.Season+1;
				}
				if(SUNMASTER.Season > 4){
					SUNMASTER.Season = 1;
				}
		}
					if (GUI.Button(new Rect(2*BOX_WIDTH+10, BOX_HEIGHT+60+30+30, BOX_WIDTH, 30), "Foggy")){
						SUNMASTER.Weather = SkyMasterManager.Weather_types.HeavyFog;
						SUNMASTER.On_demand = true;
					}

					if (GUI.Button(new Rect(3*BOX_WIDTH+10, BOX_HEIGHT-0, BOX_WIDTH, 30), "Snow storm")){
						SUNMASTER.Weather = SkyMasterManager.Weather_types.FreezeStorm;
						SUNMASTER.On_demand = true;
					}

					if (GUI.Button(new Rect(2*BOX_WIDTH+10, BOX_HEIGHT-30, BOX_WIDTH, 30), "Heavy Storm")){
						SUNMASTER.Weather = SkyMasterManager.Weather_types.HeavyStorm;
						SUNMASTER.On_demand = true;
					}

					if (GUI.Button(new Rect(3*BOX_WIDTH+10, BOX_HEIGHT-30, BOX_WIDTH, 30), "Dark Storm")){
						SUNMASTER.Weather = SkyMasterManager.Weather_types.HeavyStormDark;
						SUNMASTER.On_demand = true;
					}

					if (GUI.Button(new Rect(4*BOX_WIDTH+10, BOX_HEIGHT-30, BOX_WIDTH, 30), "Lightning")){
						SUNMASTER.Weather = SkyMasterManager.Weather_types.LightningStorm;
						SUNMASTER.On_demand = true;
					}

					if(!enable_controls){

						if (GUI.Button(new Rect(1*BOX_WIDTH+10, BOX_HEIGHT-30, BOX_WIDTH, 30), "Volume Fog")){
							SUNMASTER.Weather = SkyMasterManager.Weather_types.RollingFog;
							SUNMASTER.On_demand = true;
						}

						if (GUI.Button(new Rect(1*BOX_WIDTH+10, BOX_HEIGHT+0, BOX_WIDTH+0, 30), "Tornado")){
							SUNMASTER.Weather = SkyMasterManager.Weather_types.Tornado;
							SUNMASTER.On_demand = true;
						}

					}

//					if (GUI.Button(new Rect(6*BOX_WIDTH+10, BOX_HEIGHT+0, BOX_WIDTH+10, 30), "Volcano Erupt")){
//						SUNMASTER.Weather = SkyMasterManager.Weather_types.VolcanoErupt;
//						SUNMASTER.On_demand = true;
//					}
	
			//DOME CONTROL
					if(!enable_controls){
			GUI.TextArea( new Rect(1*BOX_WIDTH+10, BOX_HEIGHT+30, BOX_WIDTH+0, 25),"SkyDome rot");
			Dome_rot = GUI.HorizontalSlider(new Rect(1*BOX_WIDTH+10, BOX_HEIGHT+30+30, BOX_WIDTH+0, 30),Dome_rot,0,10);
			SUNMASTER.Horizontal_factor = Dome_rot;
					}

					if (GUI.Button(new Rect(5, 0*BOX_HEIGHT, BOX_WIDTH+5, 30), "Enable Controls")){

			if(enable_controls){
				enable_controls = false;
				SUNMASTER.Auto_Cycle_Sky = true;
			}else{
				enable_controls = true;
				SUNMASTER.Auto_Cycle_Sky = false;
			}
			
		}

		BOX_HEIGHT = BOX_HEIGHT+20;		
		float BOX_offset = 50;

		GUI.TextArea( new Rect(5, 5*BOX_HEIGHT+BOX_offset, 100, 20),"Sun Speed");
		SPEED = GUI.HorizontalSlider(new Rect(10, 5*BOX_HEIGHT+BOX_offset+30, 100, 30),SPEED,0.01f,70f);
		SUNMASTER.SPEED = SPEED;

		if(enable_controls){

			GUI.TextArea( new Rect(5, 0*BOX_HEIGHT+BOX_offset, 150, 20),"Increase HDR brightness");
			HDR = GUI.HorizontalSlider(new Rect(10, 0*BOX_HEIGHT+BOX_offset+30, 150, 30),HDR,0.05f,3f);
			SUNMASTER.m_fExposure = HDR;

			GUI.TextArea( new Rect(5, 1*BOX_HEIGHT+BOX_offset, 150, 20),"Esun");
			Esun = GUI.HorizontalSlider(new Rect(10, 1*BOX_HEIGHT+BOX_offset+30, 150, 30),Esun,0.9f,80f);
			SUNMASTER.m_ESun = Esun;

			GUI.TextArea( new Rect(5, 2*BOX_HEIGHT+BOX_offset, 150, 20),"Kr - White to Red factor");
			Kr = GUI.HorizontalSlider(new Rect(10, 2*BOX_HEIGHT+BOX_offset+30, 150, 30),Kr,0.0001f,0.014f);
			SUNMASTER.m_Kr = Kr;

			GUI.TextArea( new Rect(5, 3*BOX_HEIGHT+BOX_offset, 150, 20),"Km - Vertical effect factor");
			Km = GUI.HorizontalSlider(new Rect(10, 3*BOX_HEIGHT+BOX_offset+30, 150, 30),Km,0.0003f,0.1195f);
			SUNMASTER.m_Km = Km;

			GUI.TextArea( new Rect(5, 4*BOX_HEIGHT+BOX_offset, 150, 20),"G - Focus factor");
			GE = GUI.HorizontalSlider(new Rect(10, 4*BOX_HEIGHT+BOX_offset+30, 150, 30),GE,-0.69f,-0.9999f);
			SUNMASTER.m_g = GE;

			GUI.TextArea( new Rect(5, 6*BOX_HEIGHT+BOX_offset, 100, 20),"Sun Ring factor");
			Ring_factor = GUI.HorizontalSlider(new Rect(10, 6*BOX_HEIGHT+BOX_offset+30, 100, 30),Ring_factor,0f,0.15f);
			SUNMASTER.Sun_ring_factor = Ring_factor;

			GUI.TextArea( new Rect(5, 7*BOX_HEIGHT+BOX_offset, 150, 20),"Samples");
			fSamples = GUI.HorizontalSlider(new Rect(10, 7*BOX_HEIGHT+BOX_offset+30, 150, 30),fSamples,1,4);
			SUNMASTER.m_fSamples = fSamples;

			GUI.TextArea( new Rect(5, 8*BOX_HEIGHT+BOX_offset, 150, 20),"Scale depth");
			fScaleDepth = GUI.HorizontalSlider(new Rect(10, 8*BOX_HEIGHT+BOX_offset+30, 150, 30),fScaleDepth,0.1f,2);
			SUNMASTER.m_fRayleighScaleDepth = fScaleDepth;

			///////////// COLORS	
			
			GUI.TextArea( new Rect(BASE_X+130, BASE1+(3.5f*40)-20+70, 180, 20),"Tint: "+CURRENT_Force_color);
			SUNMASTER.m_fWaveLength.x = GUI.HorizontalSlider(new Rect(BASE_X+130, BASE1+(3.5f*40)+70, 100, 30),SUNMASTER.m_fWaveLength.x,0,1);
			CURRENT_Force_color.x = SUNMASTER.m_fWaveLength.x;
			SUNMASTER.m_fWaveLength.y = GUI.HorizontalSlider(new Rect(BASE_X+130, BASE1+(4.3f*40)+70, 100, 30),SUNMASTER.m_fWaveLength.y,0,1);
			CURRENT_Force_color.y = SUNMASTER.m_fWaveLength.y;
			SUNMASTER.m_fWaveLength.z = GUI.HorizontalSlider(new Rect(BASE_X+130, BASE1+(5.1f*40)+70, 100, 30),SUNMASTER.m_fWaveLength.z,0,1);
			CURRENT_Force_color.z = SUNMASTER.m_fWaveLength.z;

			BASE_X = 50;
			BASE1= 50+100;
			GUI.TextArea( new Rect(BASE_X+130, BASE1+(3.5f*40)-20+70, 180, 20),"Global tint: "+TintColor);
			SUNMASTER.m_TintColor.r = GUI.HorizontalSlider(new Rect(BASE_X+130, BASE1+(3.5f*40)+70, 100, 30),SUNMASTER.m_TintColor.r,0,1);
			TintColor.x = SUNMASTER.m_TintColor.r;
			SUNMASTER.m_TintColor.g = GUI.HorizontalSlider(new Rect(BASE_X+130, BASE1+(4.3f*40)+70, 100, 30),SUNMASTER.m_TintColor.g,0,1);
			TintColor.y = SUNMASTER.m_TintColor.g;
			SUNMASTER.m_TintColor.b = GUI.HorizontalSlider(new Rect(BASE_X+130, BASE1+(5.1f*40)+70, 100, 30),SUNMASTER.m_TintColor.b,0,1);
			TintColor.z = SUNMASTER.m_TintColor.b;

//			GUI.TextArea(new Rect(BASE_X+130, BASE1+(5.1f*40)+70+30, 100, 30),"Coloration");
//			Coloration = GUI.HorizontalSlider(new Rect(BASE_X+130, BASE1+(5.1f*40)+70+30+30, 100, 30),Coloration,0.1f,2);
//			SUNMASTER.m_Coloration = Coloration;
		}	
	}else{ // IF AUTO SEASON CYCLE
			string Season_current1 = "Spring";
			if(SUNMASTER.Season == 2){
				Season_current1= "Summer";
			}
			if(SUNMASTER.Season == 3){
				Season_current1= "Autumn";
			}
			if(SUNMASTER.Season == 4){
				Season_current1= "Winter";
			}
				GUI.TextArea(new Rect(5*BOX_WIDTH+10, BOX_HEIGHT+30+30, BOX_WIDTH, 30),Season_current1);
	  

	}
			}//END GI CHECK
  }// END OnGUI	
}
}