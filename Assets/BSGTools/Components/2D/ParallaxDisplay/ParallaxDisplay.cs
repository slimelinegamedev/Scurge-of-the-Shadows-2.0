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
using System.Collections.Generic;

namespace BSGTools {
	namespace Components {
		public class ParallaxDisplay : MonoBehaviour {
			public float scaleMultiplier = 1f;
			public Shader quadShader = null;
			public float hScrollMultiplier;
			public List<Texture2D> textures;

			ParallaxScroller[] scrollers;
			Vector3 baseScale = Vector3.one;

			void Start() {
				CreateScrollers();
			}

			// Update is called once per frame
			void Update() {
				var tex = textures[0]; //Get sample texture for dimensions.
				float scale = (Screen.height / 2.0f) / Camera.main.orthographicSize;
				float scaleX = Screen.width / scale;
				float scaleY = scaleX / (tex.width * 1.0f / tex.height);
				Vector3 baseScale = new Vector3(1f, 1f, 1f);
				baseScale.x = scaleX;
				baseScale.y = scaleY;
				this.baseScale = baseScale;

				foreach(var s in scrollers) {
					s.hSpeed = s.baseHSpeed * hScrollMultiplier;
					s.transform.localScale = baseScale * scaleMultiplier;
				}
			}

			private void CreateScrollers() {
				scrollers = new ParallaxScroller[textures.Count];

				float baseSpeed = 1f;
				for(int i = 0; i < textures.Count; i++) {
					var tex = textures[i];
					int inverseLayer = 0 - i;
					Material material = new Material(quadShader);
					material.SetTexture("_MainTex", tex);
					GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
					quad.name = string.Format("[{0}] {1}", inverseLayer, tex.name);
					quad.renderer.material = material;
					quad.transform.parent = transform;
					quad.transform.localPosition = new Vector3(0f, 0f, i);

					//Compute Scale
					float scale = (Screen.height / 2.0f) / Camera.main.orthographicSize;
					float scaleX = Screen.width / scale;
					float scaleY = scaleX / (tex.width * 1.0f / tex.height);
					Vector3 baseScale = new Vector3(1f, 1f, 1f);
					baseScale.x = scaleX;
					baseScale.y = scaleY;
					this.baseScale = baseScale;
					quad.transform.localScale = baseScale;

					var scroller = quad.AddComponent<ParallaxScroller>();
					scroller.baseHSpeed = baseSpeed;
					baseSpeed /= 2f;
					scrollers[i] = scroller;
				}
			}

			public void ResetToDefaults() {
				scaleMultiplier = 1f;
				quadShader = Shader.Find("Unlit/Transparent Cutout");
				hScrollMultiplier = 0f;
			}
		}
	}
}