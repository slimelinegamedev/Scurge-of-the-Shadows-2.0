using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scurge.Player;
using Scurge.Util;
using Scurge.Enemy;
using Scurge.Audio;

namespace Scurge.Environment {
	public class Tile : MonoBehaviour {
		public List<GameObject> Doorways;
		public List<Vector3> DoorwayRotations;
		public TileType Type;
	}
}