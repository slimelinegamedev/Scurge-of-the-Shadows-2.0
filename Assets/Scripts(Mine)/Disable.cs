using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scurge.Player;
using Scurge.Util;
using Scurge.Enemy;

namespace Scurge.Util {
	public class Disable : MonoBehaviour {

		public Objects Objects;

		public void DisableObj(bool inventoryDisab, bool DisablePlayer) {
			foreach (GameObject CurObj in Objects.Disables) {
				CurObj.SetActive(false);
			}
			foreach (HeadBob Curbob in Objects.StopTheBobs) {
				Curbob.enabled = false;
			}
			Objects.MouseX.enabled = false;
			Objects.MouseY.enabled = false;
			Objects.Controller.enabled = false;
			if(DisablePlayer) {
				Objects.Player.SetActive(false);
			}
			if(inventoryDisab) {
				Objects.Inventory.enabled = false;
			}
		}
		public void EnableObj(bool inventoryEnab, bool EnablePlayer) {
			foreach (GameObject CurObj in Objects.Disables) {
				CurObj.SetActive(true);
			}
			foreach (HeadBob Curbob in Objects.StopTheBobs) {
				Curbob.enabled = true;
			}
			Objects.MouseX.enabled = true;
			Objects.MouseY.enabled = true;
			Objects.Controller.enabled = true;
			if(EnablePlayer) {
				Objects.Player.SetActive(true);
			}
			if(inventoryEnab) {
				Objects.Inventory.enabled = true;
			}
		}
	}
}