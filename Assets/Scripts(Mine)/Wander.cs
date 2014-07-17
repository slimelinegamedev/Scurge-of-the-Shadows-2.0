using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scurge.Player;
using Scurge.Util;
using Scurge.Enemy;

namespace Scurge.AI {

	public enum Orientation {
		Left = 0,
		Right = 1
	}

	public class Wander : MonoBehaviour {
		public float speed;
		public float gravity;
		public float jumpAmount;
		public Vector3 movePosition;
		public CharacterController controller;

		public GameObject texture;
		public float originalX = 414.658f;

		public void Look(Orientation direction) {
			if(direction == Orientation.Left) {
				texture.transform.localScale = new Vector3(-originalX, texture.transform.localScale.y, texture.transform.localScale.z);
			}
			else if(direction == Orientation.Right) {
				texture.transform.localScale = new Vector3(originalX, texture.transform.localScale.y, texture.transform.localScale.z);
			}
		}
	}
}