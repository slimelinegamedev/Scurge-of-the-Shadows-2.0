using UnityEngine;
using System.Collections;
using Scurge.Player;
using Scurge.Util;

namespace Scurge.Enemy {
	public class AI : MonoBehaviour {

		public CharacterController controller;
		public Vector3 direction;
		public Animator anim;
		public float gravity;
		public float jumpHeight;
		public float MinWander;
		public float MaxWander;
		public float followSpeed;

		public bool FoundPlayer = false;
		public GameObject Player;

		void Start() {
			InvokeRepeating("Wander", 1, 1);
		}

		void Update() {
			if(!controller.isGrounded) {
				direction.y -= gravity * Time.deltaTime;
			}
			controller.Move(direction);
			if(Input.GetKeyDown(KeyCode.P)) {
				Wander();
			}
			if(FoundPlayer) {
				Follow(Player);
			}
		}
		void OnTriggerEnter(Collider collider) {
			if(collider.gameObject.tag == "Player") {
				Follow(collider.gameObject);
				Player = collider.gameObject;
			}
		}
		public void Wander() {
			if(!FoundPlayer) {
				direction.x = Random.Range(MinWander, MaxWander);
				direction.z = Random.Range(MinWander, MaxWander);
			}
		}
		public void Follow(GameObject ObjectToFollow) {
			FoundPlayer = true;
			if(ObjectToFollow.transform.position == transform.position) {
				return;
			}
			Vector3 movementDifference = ObjectToFollow.transform.position - transform.position;
		 	direction = movementDifference.normalized * followSpeed * Time.deltaTime;
		}
		public void Jump(float amount) {
			if(controller.isGrounded) {
				direction.y = amount;
			}
		}
	}
}