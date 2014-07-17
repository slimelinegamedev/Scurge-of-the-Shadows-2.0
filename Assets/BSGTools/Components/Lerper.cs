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
using System.Linq;

namespace BSGTools {
	namespace Components {
		public static class Lerper {
			private static GameObject _master;

			public static LerperTask CreateLerper(float lerpDuration) {
				if(_master == null) {
					_master = new GameObject("Lerper");
					_master.hideFlags = HideFlags.HideAndDontSave;
				}
				var lerpTask = _master.AddComponent<LerperTask>();
				lerpTask.lerpDuration = lerpDuration;
				return lerpTask;
			}
		}

		public class LerperTask : MonoBehaviour {
			public delegate void OnTick(float t);
			public event OnTick Tick;

			public delegate void OnComplete();
			public event OnComplete Complete;

			public bool useRealTime = true;
			private float startTime = 0f;
			internal float lerpDuration;
			internal bool started = false;

			public void StartLerp() {
				startTime = useRealTime ? Time.realtimeSinceStartup : Time.time;
				started = true;
			}

			void Update() {
				if(!started)
					return;
				float t = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / lerpDuration);
				if(Tick != null)
					Tick(t);

				if(t == 1f) {
					if(Complete != null)
						Complete();
					Destroy(this);
				}
			}
		}
	}
}