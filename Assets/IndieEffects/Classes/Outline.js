#pragma strict
import IndieEffectsJS;

@script RequireComponent (IndieEffectsJS)
@script AddComponentMenu ("Indie Effects/Outline")

var fxRes : IndieEffectsJS;

var threshold : float;
private var blurMat : Material;
var outlineShader : Shader; 

function Start () {
    fxRes = GetComponent("IndieEffectsJS");
    blurMat = new Material(outlineShader);
}
function Update () {
    blurMat.SetTexture("_MainTex", fxRes.RT);
    blurMat.SetFloat("_Treshold", threshold);
}
function OnPostRender () {
    FullScreenQuad(blurMat);
}