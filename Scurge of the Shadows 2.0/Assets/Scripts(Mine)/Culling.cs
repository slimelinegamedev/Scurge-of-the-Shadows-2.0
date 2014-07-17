using UnityEngine;
using System.Collections;

namespace Scurge.Util {
	public class Culling : MonoBehaviour {
		void Update() {
			if(!renderer.isVisible) {
				gameObject.GetComponent<Renderer>().enabled = false;
			}
			else {
				gameObject.GetComponent<Renderer>().enabled = true;
			}
		}
	}
}