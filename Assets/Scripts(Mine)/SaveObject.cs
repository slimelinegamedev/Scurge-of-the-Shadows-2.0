using UnityEngine;
using System.Collections;

namespace Scurge.Util {
	public class SaveObject : MonoBehaviour {

		void Update() {
			if(gameObject.name.Contains("(Clone)")) {
				gameObject.AddComponent<StoreInformation>();
			}
		}
	}
}