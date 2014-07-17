#pragma strict

@script RequireComponent (IndieEffects)
@script AddComponentMenu ("Indie Effects/Refraction")
 
var Mat : Material;
var matShader : Shader;
var newTex : Texture2D;
 
function Start () {
	newTex = new Texture2D(camera.pixelWidth, camera.pixelHeight, TextureFormat.RGB24, false);
}

function Update () {

}
 
function OnPostRender () {
	newTex.Resize(camera.pixelWidth,camera.pixelHeight,TextureFormat.RGB24,false);
	newTex.ReadPixels(Rect(camera.pixelRect.x, camera.pixelRect.y,camera.pixelWidth, camera.pixelHeight), 0,0);
	newTex.Apply();
	Mat.SetTexture("_MainTex", newTex);
	FullScreenQuad(Mat);
}