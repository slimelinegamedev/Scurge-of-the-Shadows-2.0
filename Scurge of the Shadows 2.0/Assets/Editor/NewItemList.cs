using UnityEngine;
using UnityEditor;
using System.Collections;
using Scurge;
using System.Collections.Generic;
using Scurge.Player;
using Scurge.Util;
using Scurge.Enemy;
using Scurge.Audio;
using Scurge.AI;

namespace Scurge.Editor {
	public class NewItemList : MonoBehaviour {
		[MenuItem("Assets/Create/Item List")]
		public static void Main() {
			print("Creating Item List Asset");

			ItemList asset = ScriptableObject.CreateInstance<ItemList>();

			AssetDatabase.CreateAsset(asset, "Assets/Scriptable Objects/Items.asset");
			AssetDatabase.SaveAssets();
		}	
	}
}