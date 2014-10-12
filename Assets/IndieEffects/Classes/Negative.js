#pragma strict
import IndieEffectsJS;
/*

				----------Negative----------
When i was playing around with the indie effects motion blur shader, i got 
this effect by accident. enjoy!
*/
@script RequireComponent (IndieEffectsJS)
@script AddComponentMenu ("Indie Effects/Negative")
var fxRes : IndieEffectsJS;

private var ThermoMat : Material;
var shader : Shader;
var noise : float;

function Start () {
	fxRes = GetComponent(IndieEffectsJS);
	ThermoMat = new Material(shader);
}

function Update () {
	ThermoMat.SetTexture("_MainTex", fxRes.RT);
}

function OnPostRender () {
	FullScreenQuad(ThermoMat);
}