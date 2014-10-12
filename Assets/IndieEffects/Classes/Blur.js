#pragma strict
import IndieEffectsJS;

@script RequireComponent (IndieEffectsJS)
@script AddComponentMenu ("Indie Effects/Blur")
var fxRes : IndieEffectsJS;

private var blurMat : Material;
var blurShader : Shader;
@range(0,5)
var blur : float;

function Start () {
	fxRes = GetComponent(IndieEffectsJS);
	blurMat = new Material(blurShader);
}

function Update () {
	blurMat.SetTexture("_MainTex", fxRes.RT);
	blurMat.SetFloat("_Amount", blur);
}

function OnPostRender () {
	FullScreenQuad(blurMat);
}