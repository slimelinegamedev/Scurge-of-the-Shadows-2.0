using UnityEngine;
using System.Collections;

public class PixelShader : MonoBehaviour {

	[AddComponentMenu ("Indie Effects/Tiles Effect")]
		
	public IndieEffects IndieEffects;
	public Color edgeColor;
	public float numTiles = 8.0f;
	public float threshhold = 1.0f;
	public Material blurMat;
	public Shader tilesShader;
	
	void Start() {	
		IndieEffects = transform.GetComponent<IndieEffects>();
		blurMat = new Material(tilesShader);
	}
	
	void OnPostRender() {
		blurMat.SetTexture("_MainTex", IndieEffects.renderTexture);
		blurMat.SetColor("_EdgeColor", edgeColor);
		blurMat.SetFloat("_NumTiles", numTiles);
		blurMat.SetFloat("_Threshhold", threshhold);
		IndieEffects.FullScreenQuad(blurMat);
	}
}
