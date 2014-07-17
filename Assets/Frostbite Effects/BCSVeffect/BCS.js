#pragma strict

@script RequireComponent (IndieEffects)
@script AddComponentMenu ("Indie Effects/BrightnessSaturationContrast")
 
private var Mat : Material;
var matShader : Shader;
@range(0,2)
var Brightness : float = 1;
@range(0,2)
var Contrast : float = 1;
@range(0,2)
var Saturation : float = 1;

var newTex : Texture2D;
 
function Start () {
	newTex = new Texture2D(camera.pixelWidth, camera.pixelHeight, TextureFormat.RGB24, false);
	Mat = new Material(matShader);
}

function Update () {
	//Mat.SetTexture("_MainTex", renderTexture);
	Mat.SetFloat("_SaturationAmount", Saturation);
	Mat.SetFloat("_BrightnessAmount", Brightness);
	Mat.SetFloat("_ContrastAmount", Contrast);
}
 
function OnPostRender () {
	newTex.Resize(camera.pixelWidth,camera.pixelHeight,TextureFormat.RGB24,false);
	newTex.ReadPixels(Rect(camera.pixelRect.x, camera.pixelRect.y,camera.pixelWidth, camera.pixelHeight), 0,0);
	newTex.Apply();
	Mat.SetTexture("_MainTex", newTex);
	FullScreenQuad(Mat);
}