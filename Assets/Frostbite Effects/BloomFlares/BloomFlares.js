#pragma strict
 
@script AddComponentMenu ("Indie Effects/Blur_CurveLens")
 
private var bloomFMat : Material;
var dirtyLensFX : boolean;
var bloomFlareShader : Shader;
var DirtyBloomFlareShader : Shader;
var bloomFlareColor : Color;
@range(0,10)
var intensity : float;
@range(0,1)
var threshold : float;
@range(0,1)
var width : float;
@range(0,1)
var height : float;
@Range(0, 2)
var downsample : int = 1;
var lens : Texture2D;
var newTex : Texture2D;
 
function Start () {
	newTex = new Texture2D(camera.pixelWidth, camera.pixelHeight, TextureFormat.RGB24, false);
	if(dirtyLensFX == false){
		bloomFMat = new Material(bloomFlareShader);
	}
	else {
		bloomFMat = new Material(DirtyBloomFlareShader);
	}
}

function Update () {
	bloomFMat.SetTexture("_MainTex", renderTexture);
	bloomFMat.SetTexture("_LensTex", lens);
	bloomFMat.SetTexture("_BlurTex", newTex);
	bloomFMat.SetColor("_BloomColor", bloomFlareColor);
	bloomFMat.SetFloat("_Amount", intensity);
	bloomFMat.SetFloat("_Threshold", threshold);
	bloomFMat.SetFloat("_RadiusW", width);
	bloomFMat.SetFloat("_RadiusH", height);
}
 
function OnPostRender () {
	newTex.Resize(camera.pixelWidth,camera.pixelHeight,TextureFormat.RGB24,false);
	newTex.ReadPixels(Rect(camera.pixelRect.x, camera.pixelRect.y,camera.pixelWidth, camera.pixelHeight), 0,0);
	bloomFMat.SetTexture("_BlurTex", newTex);
	bloomFMat.SetTexture("_MainTex", renderTexture);
	newTex.Apply();
	FullScreenQuadPass(bloomFMat, 1);
	newTex.Resize(camera.pixelWidth,camera.pixelHeight,TextureFormat.RGB24,false);
	newTex.ReadPixels(Rect(camera.pixelRect.x, camera.pixelRect.y,camera.pixelWidth, camera.pixelHeight), 0,0);
	bloomFMat.SetTexture("_BlurTex", newTex);
	bloomFMat.SetTexture("_MainTex", renderTexture);
	newTex.Apply();
	FullScreenQuadPass(bloomFMat, 2);
	bloomFMat.SetTexture("_MainTex", renderTexture);
	bloomFMat.SetTexture("_BlurTex", newTex);
	newTex.ReadPixels(Rect(camera.pixelRect.x, camera.pixelRect.y,camera.pixelWidth, camera.pixelHeight), 0,0);
	newTex.Apply();
	FullScreenQuadPass(bloomFMat, 3);
	newTex.Resize(camera.pixelWidth,camera.pixelHeight,TextureFormat.RGB24,false);
	newTex.ReadPixels(Rect(camera.pixelRect.x, camera.pixelRect.y,camera.pixelWidth, camera.pixelHeight), 0,0);
	bloomFMat.SetTexture("_BlurTex", newTex);
	bloomFMat.SetTexture("_MainTex", renderTexture);
	newTex.Apply();
}