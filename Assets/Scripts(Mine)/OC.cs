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
	public class OC : MonoBehaviour {

		public List<Renderer> Renderers;
		public List<Light> Lights;
		public bool DisableOnExit = true;

		void Start() {
			foreach(Renderer curRender in Renderers) {
				curRender.enabled = false;
			}
			foreach(Light curLight in Lights) {
				curLight.enabled = false;
			}
		}
		void OnTriggerEnter(Collider collider) {
			if(collider.gameObject.tag == "Player") {
				foreach(Renderer curRender in Renderers) {
					curRender.enabled = true;
				}
				foreach(Light curLight in Lights) {
					curLight.enabled = true;
				}
			}
		}
		void OnTriggerExit(Collider collider) {
			if(collider.gameObject.tag == "Player" && DisableOnExit) {
				foreach(Renderer curRender in Renderers) {
					curRender.enabled = false;
				}
				foreach(Light curLight in Lights) {
					curLight.enabled = false;
				}
			}
		}
	}
}