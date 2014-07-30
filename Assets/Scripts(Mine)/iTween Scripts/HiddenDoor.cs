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

namespace Scurge.Environment {
	public class HiddenDoor : MonoBehaviour {

		public GameObject Door;
		public Vector3 MoveAddAmount;
		public float Time;

		public bool HoverDoor = true;
		public bool Opened = false;

		void OnMouseOver() {
			if(InputManager.GetButtonDown("Interact")) {
				if(!Opened) {
					Open();
				}
				Opened = true;
			}
		}

		public void Open() {
			iTween.MoveAdd(Door, MoveAddAmount, Time);
			Opened = true;
		}
		public void Close() {
			iTween.MoveAdd(Door, new Vector3(MoveAddAmount.x, -MoveAddAmount.y, MoveAddAmount.z), Time);
			Opened = false;
		}
	}
}