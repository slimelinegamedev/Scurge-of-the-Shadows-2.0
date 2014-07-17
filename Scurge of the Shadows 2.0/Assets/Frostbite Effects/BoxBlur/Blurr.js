@script RequireComponent (IndieEffects)
import IndieEffects;

private var mat : Material;
var shader : Shader;
var Passes : float = 1;

function Start () {
	mat = new Material(shader);
	Passes = Mathf.Clamp(Passes, 1, mat.passCount);
}

function Update () {
	mat.SetTexture("_MainTex", renderTexture);
}

function OnPostRender () {
	
	GL.PushMatrix();
	for (var i = 0; i < Passes; ++i) {
		
		mat.SetPass(i);
		GL.LoadOrtho();
		GL.Begin(GL.QUADS);
		GL.Color(Color(1,1,1,1));
		GL.MultiTexCoord(0,Vector3(0,0,0));
		GL.Vertex3(0,0,0);
		GL.MultiTexCoord(0,Vector3(0,1,0));
		GL.Vertex3(0,1,0);
		GL.MultiTexCoord(0,Vector3(1,1,0));
		GL.Vertex3(1,1,0);
		GL.MultiTexCoord(0,Vector3(1,0,0));
		GL.Vertex3(1,0,0);
		GL.End();
		
		if (i < mat.passCount - 1) {
			renderTexture.ReadPixels(Rect(0,0,Screen.width,Screen.height), 0, 0);
			renderTexture.Apply();
		}
	}
	GL.PopMatrix();
	
}