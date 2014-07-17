/**
Copyright (c) 2014, Michael Notarnicola
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BSGTools {
	namespace Events {
		public abstract class MonoEvent : MonoBehaviour {
			#region Events & Delegates
			/// <summary>
			/// Called before ANY event starts.
			/// </summary>
			public delegate void OnAnyEventStarted();
			public static event OnAnyEventStarted AnyEventStarted;

			/// <summary>
			/// Called before ANY event ends.
			/// </summary>
			public delegate void OnAnyEventEnded();
			public static event OnAnyEventEnded AnyEventEnded;

			/// <summary>
			/// Called before event starts.
			/// </summary>
			public delegate void OnEventStarted();
			public event OnEventStarted EventStarted;

			/// <summary>
			/// Called before event ends.
			/// </summary>
			public delegate void OnEventEnded();
			public event OnEventEnded EventEnded;
			#endregion

			public EventStatus Status { get; private set; }

			[SerializeField]
			internal bool startOnAwake;
			[SerializeField]
			internal bool loop;

			internal bool DoingTask { get; private set; }

			private List<IEnumerator> coroutines;

			IEnumerator UpdateMonoEvent() {
				int count = 0;
				while(Status.Equals(EventStatus.Active) && count < coroutines.Count && !DoingTask) {
					if(DoingTask || Status.Equals(EventStatus.Inactive))
						yield return null;
					else {
						IEnumerator coroutine = coroutines[count];
						StartCoroutine(RunTask(coroutine));
						count++;
						yield return null;
					}
				}
				StopEvent();
			}

			void Update() {
				if(Status == EventStatus.Active)
					return;

				bool canTrigger = CanExecute();
				if(canTrigger)
					StartEvent();
			}

			internal virtual bool CanExecute() {
				return false;
			}

			#region Controls
			public void StartEvent() {
				if(Status != EventStatus.Inactive)
					return;
				StopAllCoroutines();
				Status = EventStatus.Active;
				DoingTask = false;

				if(EventStarted != null)
					EventStarted();
				if(AnyEventStarted != null)
					AnyEventStarted();
				StartCoroutine(UpdateMonoEvent());
			}

			public void StopEvent() {
				if(Status != EventStatus.Active)
					return;

				StopAllCoroutines();
				Status = EventStatus.Inactive;
				DoingTask = false;

				if(EventEnded != null)
					EventEnded();
				if(AnyEventEnded != null)
					AnyEventEnded();

				if(loop)
					StartEvent();
			}
			#endregion

			void Awake() {
				coroutines = new List<IEnumerator>(InitEvent());

				if(startOnAwake)
					StartEvent();
			}

			/// <summary>
			/// Passes all of the coroutines to the base class for execution.
			/// </summary>
			/// <returns></returns>
			internal abstract IEnumerator[] InitEvent();

			/// <summary>
			/// Provided for easy delays between events.
			/// </summary>
			/// <param name="time"></param>
			/// <returns></returns>
			internal IEnumerator Delay(float time) {
				yield return new WaitForSeconds(time);
			}

			private IEnumerator RunTask(IEnumerator task) {
				DoingTask = true;
				yield return StartCoroutine(task);
				DoingTask = false;
			}

			public enum EventStatus {
				Inactive,
				Active
			}
		}
	}
}
