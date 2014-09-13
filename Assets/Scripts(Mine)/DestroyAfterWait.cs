using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scurge;
using Scurge.AI;
using Scurge.Audio;
using Scurge.Enemy;
using Scurge.Environment;
using Scurge.Networking;
using Scurge.Player;
using Scurge.Scoreboard;
using Scurge.UI;
using Scurge.Util;

public class DestroyAfterWait : MonoBehaviour {

	public float wait;
	public bool OnActivate = false;

	void Start() {
		StartCoroutine(Destroy());
	}
	void OnEnable() {
		StartCoroutine(Destroy());
	}
	IEnumerator Destroy() {
		if(OnActivate) {
			yield return new WaitForSeconds(wait);
			Destroy(gameObject);
		}
		else if(!OnActivate) {
			yield return new WaitForSeconds(wait);
			Destroy(gameObject);
		}
	}
}