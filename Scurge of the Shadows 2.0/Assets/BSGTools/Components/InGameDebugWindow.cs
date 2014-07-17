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
using System;

namespace BSGTools {
	namespace Components {
		public class InGameDebugWindow : MonoBehaviour {
			[SerializeField]
			private KeyCode activationKey = KeyCode.BackQuote;
			[SerializeField]
			private bool dontDestroyOnLoad = true;
			[SerializeField]
			private int maxLogs = 150;

			[SerializeField]
			private Color logColor = Color.white;
			[SerializeField]
			private Color warningColor = new Color(1.0f, 0.5f, 0f);
			[SerializeField]
			private Color errorColor = Color.red;
			[SerializeField]
			private Color exceptionColor = Color.red;
			[SerializeField]
			private Color assertColor = Color.green;

			Vector2 scrollPosition = Vector2.zero;
			Queue<Log> logQueue = new Queue<Log>();
			bool visible = false;
			bool scrollLock = false;

			void Start() {
				if(dontDestroyOnLoad)
					DontDestroyOnLoad(this);
				Application.RegisterLogCallback(LogWatcher);
			}

			void OnDestroy() {
				Application.RegisterLogCallback(null);
			}

			private void LogWatcher(string condition, string stackTrace, LogType type) {
				logQueue.Enqueue(new Log(condition, stackTrace, type, DateTime.Now));
				while(logQueue.Count > maxLogs)
					logQueue.Dequeue();
			}

			void Update() {
				if(Input.GetKeyDown(activationKey))
					visible = !visible;
			}

			void OnGUI() {
				if(!visible)
					return;

				Color oldBackground = GUI.backgroundColor;
				Color oldContent = GUI.contentColor;

				Rect areaRect = new Rect(0f, 0f, Screen.width, Screen.height / 2f + 30);
				Rect boxRect = new Rect(0f, 0f, Screen.width, Screen.height / 2f);

				GUILayout.BeginArea(areaRect);
				GUI.Box(boxRect, "");

				GUIStyle style = new GUIStyle();
				style.contentOffset = Vector2.zero;
				float newScroll = 0f;
				foreach(Log l in logQueue) {
					newScroll += style.CalcHeight(new GUIContent(l.Condition + Environment.NewLine + l.StackTrace), 10f);
				}
				if(!scrollLock)
					scrollPosition = new Vector2(0f, newScroll);

				scrollPosition = GUILayout.BeginScrollView(scrollPosition);
				foreach(Log l in logQueue) {
					style.normal.textColor = ColorFromType(l.LType);
					GUILayout.BeginVertical();
					GUILayout.Label(new GUIContent(string.Format("[{0}]: {1}", l.TimeStamp.ToString(), l.Condition)), style);
					GUILayout.Label(new GUIContent(l.StackTrace), style);
					GUILayout.EndVertical();
				}

				GUILayout.EndScrollView();
				GUILayout.BeginHorizontal(GUILayout.MaxWidth(200f));
				scrollLock = GUILayout.Toggle(scrollLock, "Scroll Lock");
				if(GUILayout.Button("Clear"))
					logQueue.Clear();
				GUILayout.EndHorizontal();

				GUILayout.EndArea();

				GUI.backgroundColor = oldBackground;
				GUI.contentColor = oldContent;
			}

			private Color ColorFromType(LogType logType) {
				switch(logType) {
					case LogType.Assert: {
							return assertColor;
						}
					case LogType.Error: {
							return errorColor;
						}
					case LogType.Exception: {
							return exceptionColor;
						}
					case LogType.Log: {
							return logColor;
						}
					case LogType.Warning: {
							return warningColor;
						}
					default: {
							return GUI.contentColor;
						}
				}
			}

			public class Log {
				public string Condition { get; private set; }
				public string StackTrace { get; private set; }
				public LogType LType { get; private set; }
				public DateTime TimeStamp { get; private set; }

				public Log(string condition, string stackTrace, LogType type, DateTime timeStamp) {
					this.Condition = condition;
					this.StackTrace = stackTrace;
					this.LType = type;
					this.TimeStamp = timeStamp;
				}
			}
		}
	}
}