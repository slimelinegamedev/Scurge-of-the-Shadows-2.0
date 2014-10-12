Shader "Relief_Correct_Silhouettes" {
	Properties {
		_MainTex ("Color Map", 		2D) = "white" {} 
		_ReliefMap ("ReliefMap Map", 	2D) = "white" {} 
		_Depth ("Depth", Float) = 0.1
		_Tile ("Tile", Float) = 4
		_Curvature("Curvature", Float) = 1
		_Ambient ("Ambient Color", Color) = (0.1, 0.1, 0.1, 1.0)
		_Diffuse ("Diffuse Color", Color) = (0.8, 0.8, 0.8, 1.0)
		_Specular ("Specular Color", Color) = (1, 1, 1, 1.0)
		_LightPosition ("Light Position", Vector) = (-10,10,-10,1)
		_LightColor ("Light Color", Color) = (1,1,1,1)
	}
	SubShader 
	{
        Pass     
        {
        	
CGPROGRAM //-----------
#pragma target 3.0
#pragma vertex 		vertex_shader
#pragma fragment 	main_relief
#pragma profileoption MaxTexIndirections=64
#include "UnityCG.cginc"

uniform sampler2D 	_MainTex;
uniform sampler2D 	_ReliefMap;
uniform float 		_Depth;
uniform float 		_Tile;
uniform float 		_Curvature;
uniform float4 		_Ambient;
uniform float4 		_Diffuse;
uniform float4 		_Specular;
uniform float3 		_LightPosition;
uniform float4 		_LightColor;

struct a2v 
{
	float4 vertex 		: POSITION;
	float4 tangent		: TANGENT; 
	float3 normal		: NORMAL; 
	float2 texCoord		: TEXCOORD0; 
};

struct v2f
{
	float4 hpos 		: POSITION;
	float3 vpos 		: TEXCOORD0;
	float2 texcoord 	: TEXCOORD1;
	float3 view 		: TEXCOORD2;
	float3 light 		: TEXCOORD3;
	float3 scale 		: TEXCOORD4;
	float2 curvature 	: TEXCOORD5;
};

struct f2s
{	
	float4 color : COLOR;
};

v2f vertex_shader(a2v In) 
{ 
	v2f Out;
	
	float4 pos = float4(In.vertex.xyz, 1.0);
	
	Out.hpos 		= mul(glstate.matrix.mvp, pos);
	Out.texcoord 	= In.texCoord.xy*_Tile;;	

	float3 In_binormal = cross( In.normal, In.tangent.xyz )*In.tangent.w;
	
	float3x3 tangentspace;
	tangentspace[0] = In.tangent.xyz; 
	tangentspace[1] = -In_binormal; 
	tangentspace[2] = In.normal; 
		
	float3 osEyeVec = _ObjectSpaceCameraPos - In.vertex.xyz;
	Out.view 		= mul(tangentspace, osEyeVec);
	
	float3 wsLPos 	= mul(In.vertex.xyz, (float3x3)_Object2World);
	float3 wsLVec 	= _LightPosition - wsLPos;	
	float3 osLVec 	= mul(wsLVec, (float3x3)_World2Object).xyz;
	Out.light 		= mul(tangentspace, osLVec);
	
	// scale and curvature
	float c = _Curvature;
	Out.curvature = float2(c,c);
	Out.scale=float3(In.tangent.w,1,_Depth)/_Tile;
								
	return Out; 
}

float3 ray_position(
						in float t,			// search parameters
						in float2 tx,		// original pixel texcoord
						in float3 v,		// view vector in texture space
						in float dataz)		// data constants
{
	float3 r=v*t;
	r.z-=t*t*dataz;
	r.xy+=tx;
	return r;
}

float ray_intersect_rm_curved(
								in sampler2D reliefmap,
								in float2 tx,
								in float3 v,
								in float tmax,
								in float dataz)
{

   const int linear_search_steps=40;

   float t=0.0;
   float size=(tmax+0.001)/linear_search_steps;

   // search front to back for first point inside object
   for( int i=0;i<linear_search_steps;i++ )
   {
		float3 p=ray_position(t,tx,v,dataz);
		float4 tex=tex2D(reliefmap,p.xy); 
		if (p.z<tex.w)
			t+=size;
   }

   const int binary_search_steps=8;

   // recurse around first point for closest match
   for( int i=0;i<binary_search_steps;i++ )
   {
	  size*=0.5;
	  float3 p=ray_position(t,tx,v,dataz);
	  float4 tex=tex2D(reliefmap,p.xy);
	  if (p.z<tex.w)
		 t+=2*size;
	  t-=size;
   }

   return t;
}

f2s main_relief(v2f IN)

{
	f2s OUT;

	// view vector in eye space
	float3 v=normalize(IN.view);

	// vector perpendicular to view closest to (0,0,1)
	float3 u=normalize(2*v*v.z-float3(0,0,2));

	// mapping scale from object to texture space
	float3 mapping=1.0/IN.scale;
	
	// quadric constants
	float dataz=IN.curvature.x*v.x*v.x+IN.curvature.y*v.z*v.z; 
	dataz=sign(dataz)*max(abs(dataz),0.001);

	// compute max distance for search min(t(z=0),t(z=1))
	float d=v.z*v.z-4*dataz*IN.scale.z;
	float tmax=50;
	if (d>0)		// t when z=1
		tmax=min(tmax,(-v.z+sqrt(d))/(-2*dataz));
	d=v.z/dataz;	// t when z=0
	if (d>0)
		tmax=min(tmax,d);
	
	// transform view and quadric data to texture space
	v*=mapping;
	dataz*=mapping.z;

	// ray intersect depth map
	float t=ray_intersect_rm_curved(_ReliefMap,IN.texcoord,v,tmax,dataz);
	float alpha=1;
	if (t>tmax)
		discard; // discard fragment
	
	// compute intersected position
	float3 p=ray_position(t,IN.texcoord,v,dataz);

	// get normal and color
	float4 n=tex2D(_ReliefMap,p.xy);	
	float4 c=tex2D(_MainTex,p.xy);

	// expand normal from normal map
	n.xyz=normalize(n.xyz-0.5);

	// restore view vector
	v=normalize(IN.view);

	float3 light=normalize(IN.light);

	// compute diffuse and specular terms
	float diff=saturate(dot(light,n.xyz));
	float spec=saturate(dot(normalize(light-v),n.xyz));

	// attenuation factor
	float att=1.0-max(0,light.z); att=1.0-att*att;

	// ambient term
	float4 finalcolor=_Ambient*c;

	// compute final color
	finalcolor.xyz += att*(c.xyz*_Diffuse.xyz*diff+_Specular.xyz*pow(spec,_Specular.w));
	finalcolor.w=alpha;
	OUT.color=finalcolor;
	
	return OUT;
}

ENDCG //----------        
        } // Pass  
	} // SubShader  
} // Shader