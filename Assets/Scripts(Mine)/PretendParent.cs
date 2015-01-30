using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Scurge.Utils {
	/// <summary>
	/// makes an object pretend to be a child, and follows its parents
	/// </summary>
	public class PretendParent : MonoBehaviour {
		
		#region Variables

		public GameObject parent;

		public bool followX;
		public bool followY;
		public bool followZ;

		public int addX;
		public int addY;
		public int addZ;

		#endregion

		#region Functions

		void Update() {
			if(followX) {
				transform.position = new Vector3(parent.transform.position.x, transform.position.y, transform.position.z);
			}
			if(followY) {
				transform.position = new Vector3(transform.position.x, parent.transform.position.y, transform.position.z);
			}
			if(followZ) {
				transform.position = new Vector3(transform.position.x, transform.position.y, parent.transform.position.z);
			}
			transform.position = new Vector3(parent.transform.position.x + addX, parent.transform.position.y + addY, parent.transform.position.z + addZ);
		}

		#endregion

		#region Methods



		#endregion
	}
}