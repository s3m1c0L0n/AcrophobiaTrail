// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "SkyMaster/ShaderVolumeClouds-VOLUME" {
    Properties {
        _SunColor ("_SunColor", Color) = (0.95,0.95,0.95,0.8)
        _ShadowColor ("_ShadowColor", Color) = (0.05,0.05,0.1,0.3)
        _ColorDiff ("_ColorDiff", Float ) = 0.5
        _CloudMap ("_CloudMap", 2D) = "white" {}
        _CloudMap1 ("_CloudMap1", 2D) = "white" {}
        _Density ("_Density", Float ) = -0.4
        _Coverage ("_Coverage", Float ) = 4250
        _Transparency ("_Transparency", Float ) = 1
        _Velocity1 ("_Velocity1", Vector ) = (1,23,0,0)
        _Velocity2 ("_Velocity2", Vector ) = (1,22,0,0)   
        _LightingControl ("_LightingControl", Vector) = (1,1,-1,0)       
        _HorizonFactor ("_HorizonFactor", Range(0, 10)) = 2    
        _EdgeFactors ("_EdgeFactor2", Vector) = (0,0.52,-1,0) 
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        //_Mode ("_Mode", Range(0,5)) = 0
        _CutHeight ("_CutHeight", Float) = 240
        _CutHeight2 ("_CutHeight2", Float) = 1285
        Thickness ("Thickness", Float) = 1
         _CoverageOffset ("_Coverage Offset", Float ) = -0.15
          _ColorDiffOffset ("_ColorDiff Offset", Float ) = -0.1

        //SCATTER
        _Control ("Control Color", COLOR) = (1,1,1)
        _Color ("Color", COLOR) = (1,1,1) 
        _FogColor ("Fog Color", COLOR) = (1,1,1) 
        _FogFactor ("Fog factor", float) = 1
        _FogUnity ("Fog on/off(1,0)", float) = 0
       // _PaintMap ("_CloudMap", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }       
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #include "UnityCG.cginc"

            //SCATTER
            #include "AutoLight.cginc"
            #include "Lighting.cginc"

            #pragma vertex vert
            #pragma fragment frag
            //#define UNITY_PASS_FORWARDBASE
            #pragma multi_compile_fog 
            #pragma multi_compile_fwdbase                      
            #pragma target 3.0
			#pragma multi_compile_fwdbase nolightmap //v4.1

			uniform sampler2D _CloudMap; 
            uniform float4 _CloudMap_ST;
            uniform sampler2D _CloudMap1; 
            uniform float4 _CloudMap1_ST;
            //float4 _LightColor0;
            uniform float4 _SunColor;
            uniform float4 _ShadowColor;
            uniform float _ColorDiff;
            uniform float _Density;
            uniform float _Coverage;
            uniform float _Transparency;         
            uniform float _HorizonFactor;
            uniform float4 _LightingControl;
            uniform float2 _EdgeFactors;
            uniform float2 _Velocity1;
            uniform float2 _Velocity2;
            // uniform int _Mode;
            uniform float _CutHeight;
             uniform float _CutHeight2;
            uniform float4 _FogColor ;
            uniform float _FogFactor;
            uniform float _FogUnity;
            uniform float Thickness;
            uniform float _CoverageOffset;
               uniform float  _ColorDiffOffset;

            //uniform sampler2D _PaintMap;

            //SCATTER			
			float3 _Color;
			float3 _Control;


            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;    
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 worldPos : TEXCOORD1;    
                   float3 ForwLight: TEXCOORD2;  
                   float3 camPos:TEXCOORD3;  
                     float3 normal : TEXCOORD4;                      
                LIGHTING_COORDS(5,6)                    
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {           
             	VertexOutput o;    
                o.uv0 = v.texcoord0;    
                o.pos = UnityObjectToClipPos(v.vertex );                         
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);

         //     o.worldPos.y =o.worldPos.y+100;
        // 		o.pos.y =o.pos.y-100;
                //SCATTER 
                o.ForwLight =ObjSpaceLightDir(v.vertex); //ObjSpaceLightDir(v.vertex);
                o.camPos = normalize(WorldSpaceViewDir(v.vertex));
                o.normal = v.normal;
                TRANSFER_VERTEX_TO_FRAGMENT(o);		


                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {

            	



				float4 SUM_COLOR = float4(0,0,0,0);

				float j =1;
				int count = 30;
				for(j=1;j<count;j=j+1){ //22

				float4 TimingF = 0.0012;

				i.worldPos.y = i.worldPos.y+1*j;
			//	i.pos.y = i.pos.y+100*j; 
		//	_Coverage = _Coverage+j*0.01;
		//	_Coverage = _Coverage+j*0.0004;
//	_Coverage = _Coverage-(j+1)*0.001;
			_Density = _Density+ (j+1)*0.00000009; //0.0000001

		//_Velocity1 = _Velocity1 - float3(j*2,0,0); 

				float change_h = _CutHeight;//240;
				float PosDiff = Thickness*0.0006*(i.worldPos.y-change_h)-0.7;

                float2 UVs = (_Density)*float2(i.worldPos.x,i.worldPos.z);


                float2 UVs1 = (_Velocity1)*TimingF*_Time.y + UVs ;

                float4 cloudTexture = tex2D(_CloudMap,UVs1+_CloudMap_ST);
                float4 cloudTexture1 = tex2D(_CloudMap1,UVs1+_CloudMap1_ST);

            //    i.worldPos.x = i.worldPos.x +1.5*(j-1)*cloudTexture1.b;//+0.2*i.worldPos.y;
             i.worldPos.x = i.worldPos.x -_Time.y*0.0003*j*cloudTexture1.b;
             i.worldPos.z = i.worldPos.z +_Time.y*0.0003*j*cloudTexture1.r;
                //_PaintMap
                // float4 paintTexture1 = tex2D(_PaintMap,UVs1+_CloudMap1_ST);

                float2 UVs2 = (_Velocity2*TimingF*_Time.y + float2(_EdgeFactors.x,_EdgeFactors.y) + UVs);

                float4 Texture1 = tex2D(_CloudMap,UVs2+_CloudMap_ST); 
                float4 Texture2 = tex2D(_CloudMap1,UVs2+_CloudMap1_ST); 

                float DER = i.worldPos.y*0.001;               
   //             float3 normalA = (((DER*( (_Coverage +_CoverageOffset) +((cloudTexture.rgb*2)-1)))-(1-(Texture1.rgb*2))));             /////// -0.25 coverage	(-0.35,5)
                  float3 normalA = (((DER*( (_Coverage +_CoverageOffset) +((cloudTexture.rgb*2)-1)))-(1-(Texture1.rgb*1.2))));    
             	float3 normalN = normalize(normalA); 

             	//SCATTER              
               	//fixed atten = LIGHT_ATTENUATION(i);     
				UNITY_LIGHT_ATTENUATION(atten, i, i.worldPos.xyz); //v4.1
              		
        //     	float DER1 = -(i.worldPos.y)*PosDiff-95;  //-95
        		float DER1 = (i.worldPos.y)*PosDiff-95;  //-95
             	float PosTDiff = i.worldPos.y;
             	if(i.worldPos.y > change_h){             		
             		DER1 = (1-cloudTexture1.a);
             	}

             	float shaper = (_Transparency+4.5) *( (DER1*saturate(( (_Coverage  +_CoverageOffset)-(0.8*PosDiff)+cloudTexture1.a))))   * (Texture2.a+_CoverageOffset); /////////////////// DIFERMCE			//////////////// * 30   _Transparency /////// -0.3 coverage	

                float3 lightDirect = normalize(_WorldSpaceLightPos0.xyz);
               	lightDirect.y = -lightDirect.y;
             //  	lightDirect.x = -lightDirect.x;
               
             //   float ColDiff =  (_ColorDiff+_ColorDiffOffset)+((1+(DER*_LightingControl.r*_LightingControl.g))*0.5); 
                float ColDiff =  (_ColorDiff+_ColorDiffOffset)+((0.2+(DER*_LightingControl.r*_LightingControl.g))*0.2); 

                float verticalFactor = dot(lightDirect, float3(0,1,0));
             	float Lerpfactor = (ColDiff+(_ShadowColor.a*(dot(lightDirect,normalN)-1)*ColDiff));

                float ColB = _SunColor.rgb;
           
	                change_h =_CutHeight2;   //10
	                PosDiff =  0.0004*(i.worldPos.y-change_h);  
	                PosTDiff = i.worldPos.y*PosDiff;          
	             	DER1 = -(i.worldPos.y)*PosDiff;

	             	if(i.worldPos.y > change_h){	             		
	             		DER1 = (1-cloudTexture1.a) *  PosTDiff ;
	             	}
	//             	ColB =1*_SunColor.a*(1-cloudTexture1.a)*_SunColor.rgb*DER1*(1-verticalFactor);
             	  	ColB =saturate(_SunColor.a+0.5)*(1-cloudTexture1.a)*_SunColor.rgb*DER1*(1-verticalFactor);

             	//SCATTER
         //    	float diff = saturate(dot((-i.camPos), normalize(i.ForwLight)))+0.7;
             	float diff = saturate(dot((normalN), normalize(i.ForwLight)))+0.7;
             
             	float diff2 = distance(_WorldSpaceCameraPos,i.worldPos)*distance(_WorldSpaceCameraPos,i.worldPos);//*(j+0.001)*100;           

	            float3 finalCol = diff*_LightColor0.rgb* atten;	

	            float3 endColor =( _Control.x*lerp(_ShadowColor.rgb,(0.8)*ColB, Lerpfactor) + _Control.z*float4(min(finalCol.rgb,1),Texture1.a)  )*_SunColor.rgb;//_Color;
	          
	   //         float4 Fcolor = float4(saturate(endColor + (_FogFactor/3)  *diff2*_FogColor*0.00000001 + 0),saturate(shaper - 0.01*(_HorizonFactor*0.00001*diff2)  )) ; //-8 _FogFactor
	   		    float4 Fcolor = float4(saturate(endColor + (_FogFactor/3)  *diff2*_FogColor*0.00000001*cloudTexture1.a + 0),saturate(shaper - 0.01*(_HorizonFactor*0.00001*diff2)  )) ; //-8 _FogFactor
	            SUM_COLOR += Fcolor/count;

	            }//END FOR LOOP

	            if(_FogUnity==1){
	               UNITY_APPLY_FOG(i.fogCoord, SUM_COLOR);
	            }
                return (float4((SUM_COLOR.r),(SUM_COLOR.g),(SUM_COLOR.b),saturate(SUM_COLOR.a)));

            }
            ENDCG
        }
























         Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
                   cull off    
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_fog 
            #pragma multi_compile_shadowcaster                    
            #pragma target 3.0
			#pragma multi_compile_fwdbase nolightmap //v4.1

            uniform sampler2D _CloudMap; 
            uniform float4 _CloudMap_ST;
            uniform sampler2D _CloudMap1; 
            uniform float4 _CloudMap1_ST;
            //uniform float4 _LightColor0;          
            uniform float _Density;
            uniform float _Coverage;
            uniform float _Transparency;         
            uniform float2 _EdgeFactors;
            uniform float2 _Velocity1;
            uniform float2 _Velocity2;
            uniform float _Cutoff;
            uniform float _CutHeight;

          
            uniform float Thickness;
            uniform float _CoverageOffset;

            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;    
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;               
                float2 uv0 : TEXCOORD1;
                float4 worldPos : TEXCOORD2;               
            };
            VertexOutput vert (VertexInput v) {           
             	VertexOutput o;    
                o.uv0 = v.texcoord0;    
                o.pos = UnityObjectToClipPos(v.vertex );                         
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                            
                 float2 UVs = _Density*float2(i.worldPos.x,i.worldPos.z);
                float4 TimingF = 0.0012;
                float2 UVs1 = _Velocity1*TimingF*_Time.y + UVs;
                float4 cloudTexture = tex2D(_CloudMap,UVs1+_CloudMap_ST);
                float4 cloudTexture1 = tex2D(_CloudMap1,UVs1+_CloudMap1_ST);
                float2 UVs2 = (_Velocity2*TimingF*_Time.y + float2(_EdgeFactors.x,_EdgeFactors.y) + UVs);
                float4 Texture1 = tex2D(_CloudMap,UVs2+_CloudMap_ST); 
                float4 Texture2 = tex2D(_CloudMap1,UVs2+_CloudMap1_ST); 

                float DER = i.worldPos.y*0.001;               
                float3 normalA = (((DER*((_Coverage  +_CoverageOffset)+((cloudTexture.rgb*2)-1)))-(1-(Texture1.rgb*2))));             	
             	float3 normalN = normalize(normalA); 

				float change_h =_CutHeight;
				float PosDiff = Thickness*0.0006*(i.worldPos.y-change_h);
             	float DER1 = -(i.worldPos.y+0)*PosDiff;
             	float PosTDiff = i.worldPos.y*PosDiff;
             	if(i.worldPos.y > change_h){             		
             		DER1 = (1-cloudTexture1.a) *  PosTDiff;
             		//DER1 =  PosTDiff;
             	}

             	float shaper = _Transparency*((DER1*saturate(((_Coverage  +_CoverageOffset)+cloudTexture1.a)))-Texture2.a);
              
                clip(shaper - _Cutoff+0.4);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }



       
    }
    FallBack "Diffuse"
}