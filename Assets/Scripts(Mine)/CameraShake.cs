using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	public Camera camera;
	public float shake = 0;
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;
	public Vector3 originalPos;

	public void Shake(float Timer, float Amount, float DecreaseFactor) {
		shake = Timer;
		shakeAmount = Amount;
		decreaseFactor = DecreaseFactor;
	}

	void Update() {
		if(shake > 0) {
			camera.transform.localPosition = Random.insideUnitSphere * shakeAmount;
			shake -= Time.deltaTime * decreaseFactor;
		}
		else {
			shake = 0.0f;
			camera.transform.localPosition = originalPos;
		}
	}
}
