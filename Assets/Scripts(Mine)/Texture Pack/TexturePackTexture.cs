using UnityEngine;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections;
using System.Xml.Serialization; 
using System.Collections.Generic;
using Scurge;
using Scurge.AI;
using Scurge.Audio;
using Scurge.Enemy;
using Scurge.Environment;
using Scurge.Networking;
using Scurge.Player;
using Scurge.Scoreboard;
using Scurge.UI;
using Scurge.Util;

public class TexturePackTexture : MonoBehaviour {

	public string TextureName;
	public string TexturePackDecided;
	public Material texture;
	public Shader shader;
	public string Directory;
	public WWW download;

	public Material defaultTex;

	public Texture2D NormalMap(Texture2D source,float strength) {
		strength=Mathf.Clamp(strength,0.0F,10.0F);
		Texture2D result;
		float xLeft;
		float xRight;
		float yUp;
		float yDown;
		float yDelta;
		float xDelta;
		result = new Texture2D (source.width, source.height, TextureFormat.ARGB32, true);
		for (int by=0; by<result.height; by++) {
			for (int bx=0; bx<result.width; bx++) {
				xLeft = source.GetPixel(bx-1,by).grayscale*strength;
				xRight = source.GetPixel(bx+1,by).grayscale*strength;
				yUp = source.GetPixel(bx,by-1).grayscale*strength;
				yDown = source.GetPixel(bx,by+1).grayscale*strength;
				xDelta = ((xLeft-xRight)+1)*0.5f;
				yDelta = ((yUp-yDown)+1)*0.5f;
				result.SetPixel(bx,by,new Color(xDelta,yDelta,1.0f,yDelta));
			}
		}
		result.Apply();
		return result;
	}

	void Start() {
		TexturePackTexture reference = gameObject.GetComponent<TexturePackTexture>();
		TexturePack._TexturePack.TexturePackTextures.Add(reference);
	}
	void Update() {
		
	}
	public IEnumerator Load() {
		if(TexturePack._TexturePack.Enabled) {

			//Paths
			TexturePackDecided = TexturePack._TexturePack.TexturePackName;
			Directory = TexturePack._TexturePack.directory;

			//Main Texture
			download = new WWW("file:///" + TexturePack._TexturePack.directory + TextureName + ".png");
			yield return download;
			texture.mainTexture = download.texture;

			//Bump Map
			download = new WWW("file:///" + TexturePack._TexturePack.directory + TextureName + "_map.png");
			yield return download;
			texture.SetTexture("_BumpMap", NormalMapGen.CreateDOT3(download.texture, 0.5f, 0.01f));

			renderer.material = texture;

			Texture2D BumpMap = (Texture2D)texture.GetTexture("_BumpMap");
			Texture2D MainTex = (Texture2D)texture.GetTexture("_MainTex");
			BumpMap.filterMode = FilterMode.Point;
			MainTex.filterMode = FilterMode.Point;
			texture.SetTexture("_BumpMap", BumpMap);
			texture.SetTexture("_MainTex", MainTex);
		}
		else  {
			renderer.material = defaultTex;
		}
	}
}
