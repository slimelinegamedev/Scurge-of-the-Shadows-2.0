using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scurge.Player;
using Scurge.Util;
using Scurge.Enemy;

namespace Scurge.Util {
	public class Objects : MonoBehaviour {
		public GameObject Player;
		public GameObject Camera;
		public GameObject[] Disables;
		public MouseLook MouseX;
		public MouseLook MouseY;
		public FirstPersonDrifter Controller;
		public Inventory Inventory;
		public HeadBob[] StopTheBobs;
	}
}