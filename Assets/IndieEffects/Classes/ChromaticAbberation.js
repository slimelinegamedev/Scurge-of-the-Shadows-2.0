#pragma strict

@script RequireComponent(IndieEffectsJS)
@script AddComponentMenu("Indie Effects/Chromatic Abberation")
import IndieEffectsJS;

var fxRes : IndieEffectsJS;
var shader : Shader;
private var chromMat : Material;
var vignette : Texture2D;

function Start () {
	fxRes = GetComponent(IndieEffectsJS);
	chromMat = new Material(shader);
}

function OnPostRender () {
	chromMat.SetTexture("_MainTex", fxRes.RT);
	chromMat.SetTexture("_Vignette", vignette);
	FullScreenQuad(chromMat);
}