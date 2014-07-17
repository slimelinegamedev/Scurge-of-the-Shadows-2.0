using UnityEngine;
using System.Collections;

namespace Scurge.Audio {
	public class Music : MonoBehaviour {

		public void CrossFade(AudioSource lowerFade, AudioSource higherFade) {
			for(float lowerSound = 0.0f; lowerSound > 0; lowerSound -= 0.05f) {
				lowerFade.volume = lowerSound;
			}
			for(float higherSound = 0.0f; higherSound > 0; higherSound += 0.05f) {
				higherFade.volume = higherSound;
			}
		}
	}
}