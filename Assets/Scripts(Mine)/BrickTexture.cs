using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scurge;
using Scurge.Player;
using Scurge.Util;
using Scurge.Enemy;
using Scurge.Audio;
using Scurge.AI;
using Scurge.Environment;

namespace Scurge.Environment {
	public class BrickTexture : MonoBehaviour {

		public List<Material> Textures;

		void Start() {
			int WhichTex = Random.Range(0, 300);

			if(WhichTex >= 150) {
				gameObject.GetComponent<Renderer>().material = Textures[0];
			}
			else if(WhichTex < 150 && WhichTex >= 250) {
				gameObject.GetComponent<Renderer>().material = Textures[1];
			}
			else if(WhichTex < 250) {
				gameObject.GetComponent<Renderer>().material = Textures[2];
			}
		} 
	}
}