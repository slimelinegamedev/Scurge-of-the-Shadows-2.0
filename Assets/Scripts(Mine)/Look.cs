using UnityEngine;
using System.Collections;

namespace Scurge.Util {
	public class Look : MonoBehaviour {

		public Camera camera;
		private float initialRotation;
	 
		void Start() {
			initialRotation = transform.rotation.z;
		}
	 
		void Update() {
			transform.rotation = camera.transform.rotation;
			transform.Rotate(Vector3.up * initialRotation * 180); 
		}
	}
}