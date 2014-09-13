using UnityEngine;
using System.IO;
using System.Collections;
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

public class TexturePack : MonoBehaviour {

	public static TexturePack _TexturePack;
	public bool Enabled = false;
	public string TexturePackName;
	public string directory;

	public List<TexturePackTexture> TexturePackTextures;

	void Awake() {
		Load();
		LoadPaths();
	}
	public void Load() {
		if(Enabled) {
			print("Texture Packs Enabled!");
			_TexturePack = gameObject.GetComponent<TexturePack>();
			foreach (TexturePackTexture curTexPackTex in TexturePackTextures) {
				StartCoroutine(curTexPackTex.Load());
			}
		}
		else {
			print("Texture Packs Disabled...");
		}
	}
	public void LoadPaths() {
		#if UNITY_EDITOR
			Debug.LogError("Texture Packs CANNOT Be Loaded In The Editor.");
		#endif
		#if UNITY_STANDALONE_OSX
			//OSX
			string backOne = System.IO.Directory.GetParent(Application.dataPath).FullName;
			directory = System.IO.Directory.GetParent(backOne).FullName;
			directory = directory + "/Texture Packs/" + TexturePackName + "/";
		#endif
		#if UNITY_STANDALONE_WIN
			//Windows
		#endif
		#if UNITY_STANDALONE_LINUX
			//Linux
		#endif
	}

	void OnGUI() {
		
	}
}
