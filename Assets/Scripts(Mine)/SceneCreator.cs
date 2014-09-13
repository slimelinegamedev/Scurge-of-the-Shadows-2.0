using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scurge;
using Scurge.Environment;
using Scurge.Player;
using Scurge.Util;
using Scurge.Enemy;
using Scurge.Audio;
using Scurge.AI;
using TeamUtility.IO;

namespace Scurge.Util {
	public enum SceneObjectType {
		Environment,
		Enemy
	}
	[System.Serializable]
	public class SceneObject {
		public string title;
		public SceneObjectType type;
		public GameObject obj;
	}
	public class SceneCreator : MonoBehaviour {
		public List<SceneObject> objects;
	}
}