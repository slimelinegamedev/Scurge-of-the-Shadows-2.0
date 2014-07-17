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
using Rotorz.ReorderableList;

namespace BSGTools {
	namespace Components {
		namespace Editors {
			[CustomEditor(typeof(ParallaxDisplay))]
			public class ParallaxDisplayEditor : Editor {
				private SerializedProperty hScrollMultiplier;
				private SerializedProperty scaleMultiplier;
				private SerializedProperty quadShader;
				private SerializedProperty textures;

				// Use this for initialization
				private void OnEnable() {
					textures = serializedObject.FindProperty("textures");
					scaleMultiplier = serializedObject.FindProperty("scaleMultiplier");
					quadShader = serializedObject.FindProperty("quadShader");
					hScrollMultiplier = serializedObject.FindProperty("hScrollMultiplier");
				}

				// Update is called once per frame
				public override void OnInspectorGUI() {
					hScrollMultiplier.floatValue = EditorGUILayout.FloatField("H Scroll Multiplier", hScrollMultiplier.floatValue);
					scaleMultiplier.floatValue = EditorGUILayout.FloatField("Quad Scale Multiplier", scaleMultiplier.floatValue);
					quadShader.objectReferenceValue = EditorGUILayout.ObjectField("Quad Shader", quadShader.objectReferenceValue, typeof(Shader));

					ReorderableListGUI.Title("Layers");
					ReorderableListGUI.ListField(textures);

					var lastWidth = 0f;
					var lastHeight = 0f;
					var texList = (target as ParallaxDisplay).textures;
					for(int i = 0; i < texList.Count; i++) {
						var tex = texList[i];
						if(tex == null)
							continue;
						if(lastWidth == 0f || lastHeight == 0f) {
							lastWidth = tex.width;
							lastHeight = tex.height;
						}

						if(tex.width != lastWidth || tex.height != lastHeight) {
							ShowTexSizeError(tex);
							textures.DeleteArrayElementAtIndex(i);
							texList.RemoveAt(i);
							break;
						}

						lastWidth = tex.width;
						lastHeight = tex.height;
					}


					if(EditorGUILayout.ToggleLeft("Reset Settings", false))
						(target as ParallaxDisplay).ResetToDefaults();

					serializedObject.ApplyModifiedProperties();
					serializedObject.Update();
				}

				private void ShowTexSizeError(Texture2D badTex) {
					EditorUtility.DisplayDialog("Texture Size Discrepancy", string.Format("All textures must have identical dimensions! The following texture is not equal to the others: {0}", badTex.name), "Close");
				}
			}
		}
	}
}