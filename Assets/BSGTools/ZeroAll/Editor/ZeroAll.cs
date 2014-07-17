/**
Copyright (c) 2014, Michael Notarnicola
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using UnityEngine;
using System.Collections;
using UnityEditor;

namespace BSGTools {
	namespace EditorUtilities {
		public class ZeroAll : Editor {
			[MenuItem("GameObject/BSG Tools/Zero All %&a")]
			static void ResetAll() {
				GameObject[] selection = Selection.gameObjects;
				if(selection.Length < 1)
					return;

				Undo.RecordObjects(selection, "Zero All");
				foreach(GameObject go in selection) {
					InternalZeroPosition(go);
					InternalZeroRotation(go);
					InternalZeroScale(go);
				}

				ShowNotification(string.Format("Zero-All on {0} objects", Selection.gameObjects.Length));
			}

			[MenuItem("GameObject/BSG Tools/Zero Position %&z")]
			static void ZeroPosition() {
				GameObject[] selection = Selection.gameObjects;
				if(selection.Length < 1)
					return;

				Undo.RecordObjects(selection, "Zero Position");
				foreach(GameObject go in selection)
					InternalZeroPosition(go);
				ShowNotification(string.Format("Zero-Position on {0} objects", Selection.gameObjects.Length));
			}

			[MenuItem("GameObject/BSG Tools/Zero Rotation %&r")]
			static void ZeroRotation() {
				GameObject[] selection = Selection.gameObjects;
				if(selection.Length < 1)
					return;

				Undo.RecordObjects(selection, "Zero Rotation");
				foreach(GameObject go in selection)
					InternalZeroRotation(go);
				ShowNotification(string.Format("Zero-Rotation on {0} objects", Selection.gameObjects.Length));
			}

			[MenuItem("GameObject/BSG Tools/Zero Scale %&s")]
			static void ZeroScale() {
				GameObject[] selection = Selection.gameObjects;
				if(selection.Length < 1)
					return;

				Undo.RecordObjects(selection, "Zero Position");
				foreach(GameObject go in selection)
					InternalZeroScale(go);

				ShowNotification(string.Format("Zero-Scale on {0} objects", Selection.gameObjects.Length));
			}

			private static void InternalZeroPosition(GameObject go) {
				go.transform.localPosition = Vector3.zero;
			}
			private static void InternalZeroRotation(GameObject go) {
				go.transform.localRotation = Quaternion.Euler(Vector3.zero);
			}
			private static void InternalZeroScale(GameObject go) {
				go.transform.localScale = Vector3.one;
			}

			static void ShowNotification(string message) {
				if(SceneView.currentDrawingSceneView != null)
					SceneView.currentDrawingSceneView.ShowNotification(new GUIContent(message));
				Debug.Log(message);
			}
		}
	}
}