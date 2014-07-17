#pragma strict

@script RequireComponent (IndieEffects)
@script AddComponentMenu ("Indie Effects/FilmicTonemap")
 
private var Mat : Material;
var matShader : Shader;
@range(0,1)
var ShoulderStrength : float = 0.15;
@range(0,1)
var LinearStrength : float = 0.55;
@range(0,1)
var LinearAngle : float = 0.1;
@range(0,1)
var ToeStrength : float = 0.2;
@range(0,1)
var ToeNumerator : float = 0.02;
@range(0,1)
var ToeDenominator : float = 0.7;
@range(0,20)
var Weight : float = 10.2;
//var IndieEffects : MonoBehaviour;


var newTex : Texture2D;
 
function Start () {
	newTex = new Texture2D(camera.pixelWidth, camera.pixelHeight, TextureFormat.RGB24, false);
	Mat = new Material(matShader);
}

function Update () {
	Mat.SetFloat("_A", ShoulderStrength);
	Mat.SetFloat("_B", LinearStrength);
	Mat.SetFloat("_C", LinearAngle);
	Mat.SetFloat("_D", ToeStrength);
	Mat.SetFloat("_E", ToeNumerator);
	Mat.SetFloat("_F", ToeDenominator);
	Mat.SetFloat("_W", Weight);
}

function OnPostRender () {
	newTex.Resize(camera.pixelWidth,camera.pixelHeight,TextureFormat.RGB24,false);
	newTex.ReadPixels(Rect(camera.pixelRect.x, camera.pixelRect.y,camera.pixelWidth, camera.pixelHeight), 0,0);
	newTex.Apply();
	Mat.SetTexture("_MainTex", newTex);
	FullScreenQuad(Mat);
}