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
using BSGTools.Utilities;
using System.Linq;

namespace BSGTools {
	namespace Components {
		public class AudioBank : MonoBehaviour {
			[SerializeField]
			private AudioClip[] clips;

			[SerializeField]
			private string _bankName;
			public string BankName {
				get {
					return _bankName;
				}
			}

			private AudioClip lastPlayed;

			private AudioSource _source;
			public AudioSource Source {
				get {
					if(_source == null)
						_source = gameObject.AddComponent<AudioSource>();
					return _source;
				}
			}

			#region PlayRandom
			public bool PlayRandom() {
				return PlayRandom(false);
			}

			public bool PlayRandom(bool playUnique) {
				return PlayRandom(playUnique, 0f, false);
			}

			public bool PlayRandom(bool playUnique, float delay) {
				return PlayRandom(playUnique, delay, false, false);
			}

			public bool PlayRandom(bool playUnique, float delay, bool checkNotPlaying) {
				return PlayRandom(playUnique, delay, checkNotPlaying, false);
			}

			public bool PlayRandom(bool playUnique, float delay, bool checkNotPlaying, bool stopAll) {
				if(checkNotPlaying && Source.isPlaying)
					return false;
				if(stopAll)
					Source.Stop();


				Source.clip = clips.Random();
				if(playUnique)
					while(Source.clip.Equals(lastPlayed))
						Source.clip = clips.Random();
				lastPlayed = Source.clip;
				Source.PlayDelayed(delay);
				return true;
			}
			#endregion

			#region PlayAtIndex
			public bool PlayAtIndex(int index) {
				return PlayAtIndex(index, false);
			}

			public bool PlayAtIndex(int index, float delay) {
				return PlayAtIndex(index, delay, false);
			}

			public bool PlayAtIndex(int index, bool checkNotPlaying) {
				return PlayAtIndex(index, 0f, checkNotPlaying);
			}

			public bool PlayAtIndex(int index, float delay, bool checkNotPlaying) {
				return PlayAtIndex(index, delay, checkNotPlaying, false);
			}

			public bool PlayAtIndex(int index, float delay, bool checkNotPlaying, bool stopAll) {
				if(checkNotPlaying && Source.isPlaying)
					return false;
				if(stopAll)
					Source.Stop();

				Source.clip = clips[index];
				lastPlayed = Source.clip;
				Source.PlayDelayed(delay);
				return true;
			}
			#endregion
		}

		public static class ABExtensions {
			public static AudioBank FindAB(this GameObject gameObject, string refName) {
				var banks = gameObject.GetComponents<AudioBank>();
				if(banks == null)
					return null;

				return banks.SingleOrDefault(ab => ab.BankName == refName);
			}
		}
	}
}