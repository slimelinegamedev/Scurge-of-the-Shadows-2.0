/*********************************************************************NVMH3****
*******************************************************************************
$Revision: #4 $
 
Copyright NVIDIA Corporation 2008
TO THE MAXIMUM EXTENT PERMITTED BY APPLICABLE LAW, THIS SOFTWARE IS PROVIDED
*AS IS* AND NVIDIA AND ITS SUPPLIERS DISCLAIM ALL WARRANTIES, EITHER EXPRESS
OR IMPLIED, INCLUDING, BUT NOT LIMITED TO, IMPLIED WARRANTIES OF MERCHANTABILITY
AND FITNESS FOR A PARTICULAR PURPOSE.  IN NO EVENT SHALL NVIDIA OR ITS SUPPLIERS
BE LIABLE FOR ANY SPECIAL, INCIDENTAL, INDIRECT, OR CONSEQUENTIAL DAMAGES
WHATSOEVER (INCLUDING, WITHOUT LIMITATION, DAMAGES FOR LOSS OF BUSINESS PROFITS,
BUSINESS INTERRUPTION, LOSS OF BUSINESS INFORMATION, OR ANY OTHER PECUNIARY
LOSS) ARISING OUT OF THE USE OF OR INABILITY TO USE THIS SOFTWARE, EVEN IF
NVIDIA HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES.
 
% Degrade image into a set of mock "3D-looking" tiles
 
 
 
To learn more about shading, shaders, and to bounce ideas off other shader
  authors and users, visit the NVIDIA Shader Library Forums at:
 
  http://developer.nvidia.com/forums/
 
Shader "Custom/Tiles Effect" {
Properties {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _EdgeColor ("EdgeColor", Color) = (0.5,0.5,0.5,1)
    _NumTiles ("Number of Tiles", float) = 8.0
    _Threshhold ("Threshhold", float) = 1.0
}
 
SubShader {
    Pass {
        ZTest Always Cull Off ZWrite Off
        Fog { Mode off }
        //Blend DstColor SrcColor
 
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#include "UnityCG.cginc"
 
sampler2D _MainTex;
half4  _EdgeColor;
float4 _MainTex_TexelSize;
float _NumTiles;
float _Threshhold;
 
struct v2f {
    float4 pos : POSITION;
    float2 UV : TEXCOORD0;
};
 
v2f vert( appdata_img v ) {
    v2f o;
    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
    float2 uv = MultiplyUV( UNITY_MATRIX_TEXTURE0, v.texcoord );
    o.UV = uv;
    return o;
}
 
 
half4 frag (v2f IN) : COLOR {
    half4 original = tex2D(_MainTex, IN.UV);
 
    half size = 1.0/_NumTiles;
    half2 Pbase = IN.UV - fmod(IN.UV,size.xx);
    half2 PCenter = Pbase + (size/2.0).xx;
    half2 st = (IN.UV - Pbase)/size;
    half4 c1 = (half4)0;
    half4 c2 = (half4)0;
    half4 invOff = (1-_EdgeColor);
    if (st.x > st.y) { c1 = invOff; }
    half threshholdB =  1.0 - _Threshhold;
    if (st.x > threshholdB) { c2 = c1; }
    if (st.y > threshholdB) { c2 = c1; }
    half4 cBottom = c2;
    c1 = (half4)0;
    c2 = (half4)0;
    if (st.x > st.y) { c1 = invOff; }
    if (st.x < _Threshhold) { c2 = c1; }
    if (st.y < _Threshhold) { c2 = c1; }
    half4 cTop = c2;
    //half4 tileColor = tex2D(SceneSampler,PCenter);
    half4 tileColor = tex2D(_MainTex,PCenter);
    half4 result = tileColor + cTop - cBottom;
    return result;
 
}
ENDCG
    }
}
 
Fallback off
 
