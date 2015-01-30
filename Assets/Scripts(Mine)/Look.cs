using UnityEngine;
using System.Collections;

namespace Scurge.Util {
	public class Look : MonoBehaviour {

		public Camera camera;
		private float initialRotation;
		private float initialRotationX;
	 
		void Start() {
			initialRotation = transform.rotation.z;
			initialRotationX = transform.rotation.x;
			if(camera == null) {
				print("I Have No Camera! Im " + gameObject.name + " And My Parent Is " + transform.parent + "! Help Me!!!");
			}
		}
	 
		void Update() {
			transform.rotation = camera.transform.rotation;
			transform.Rotate(Vector3.up * initialRotation * 180); 
			transform.eulerAngles = new Vector3(initialRotationX, transform.eulerAngles.y, transform.eulerAngles.z);
		}
	}
}