#pragma strict
 
@script RequireComponent (IndieEffects)
@script AddComponentMenu ("Indie Effects/Blur_CurveLens")
 
private var bloomMat : Material;
var bloomShaderCurve : Shader;
var bloomColor : Color;
var dirtyLensTex : Texture2D;
@range(0,10)
var intensity : float;
@range(0,1)
var threshold : float;
@range(0,0.02)
var radius : float;
@Range(0, 2)
var downsample : int = 1;
@range(0,1)
var lensIntensity : float;
var newTex : Texture2D;
 
function Start () {
newTex = new Texture2D(camera.pixelWidth, camera.pixelHeight, TextureFormat.RGB24, false);
bloomMat = new Material(bloomShaderCurve);
}
 
function Update () {
bloomMat.SetTexture("_MainTex", renderTexture);
bloomMat.SetTexture("_LensTex", dirtyLensTex);
bloomMat.SetTexture("_BlurTex", renderTexture);
bloomMat.SetColor("_BloomColor", bloomColor);
bloomMat.SetFloat("_Amount", intensity);
bloomMat.SetFloat("_Threshold", threshold);
bloomMat.SetFloat("_Radius", radius);
}
 
function OnPostRender () {

	bloomMat.SetTexture("_BlurTex", renderTexture);
	FullScreenQuadPass(bloomMat, 1);
	newTex.Resize(camera.pixelWidth,camera.pixelHeight,TextureFormat.RGB24,false);
	newTex.ReadPixels(Rect(camera.pixelRect.x, camera.pixelRect.y,camera.pixelWidth, camera.pixelHeight), 0,0);
	bloomMat.SetTexture("_BlurTex", newTex);
	newTex.Apply();
	FullScreenQuadPass(bloomMat, 2);
	bloomMat.SetTexture("_MainTex", renderTexture);
	bloomMat.SetTexture("_LensTex", dirtyLensTex);
	bloomMat.SetFloat("_LensAmount", lensIntensity);
	newTex.ReadPixels(Rect(camera.pixelRect.x, camera.pixelRect.y,camera.pixelWidth, camera.pixelHeight), 0,0);
	newTex.Apply();
	FullScreenQuadPass(bloomMat, 3);
}