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
using System.IO;
using System.Runtime.InteropServices;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace BSGTools {
	namespace Components {
		public abstract class ExternalConsole : MonoBehaviour {
			#region Vars
			[SerializeField]
			private string consoleName = "Unity Console";

			[SerializeField]
			[Range(1024, UInt16.MaxValue)]
			private int port = 7253;

			[SerializeField]
			[Range(1f, 5f)]
			private float connectDelay = 1f;

			/// <summary>
			/// Destroys this external console when the game exits. 
			/// </summary>
			[SerializeField]
			private bool destroyOnExit = true;

			/// <summary>
			/// Feel free to change this depending on your Project layout.
			/// </summary>
			private string assetPathToExecutable = "/BSG Tools/Components/UnityExtConsole.exe";

			private string finalExecutablePath;

			private Process consoleProc;
			#endregion

			// Use this for initialization
			void Start() {
				finalExecutablePath = Application.dataPath + assetPathToExecutable;
				if(!File.Exists(finalExecutablePath))
					UnityEngine.Debug.LogError("COULD NOT FIND EXTCONSOLE EXECUTABLE. CHECK 'assetPathToExecutable' in 'ExternalConsole.cs'!");
				else
					StartCoroutine(_UpdateStream());
			}

			void OnApplicationQuit() {
				if(destroyOnExit)
					consoleProc.Kill();
			}

			private IEnumerator _UpdateStream() {
				StartConsole(port);
				yield return new WaitForSeconds(connectDelay);

				using(TcpClient tcpOut = new TcpClient("127.0.0.1", port)) {
					using(StreamWriter stream = new StreamWriter(tcpOut.GetStream())) {
						StringBuilder sb = new StringBuilder();
						while(true) {
							try {
								string data = UpdateConsole(sb).Trim();
								data = Convert.ToBase64String(stream.Encoding.GetBytes(data));
								stream.WriteLine(data);
								stream.Flush();
							}
							catch(Exception e) {
								UnityEngine.Debug.LogError(e.Message);
								this.enabled = false;
								StopAllCoroutines();
								break;
							}
							sb.Length = 0;
							yield return null;
						}
					}
				}
			}

			private void StartConsole(int port) {
				ProcessStartInfo info = new ProcessStartInfo();
				info.FileName = Application.dataPath + assetPathToExecutable;
				info.Arguments = string.Format(@"""{0}"" {1} {2}", consoleName, port, Process.GetCurrentProcess().Id);
				consoleProc = Process.Start(info);
			}

			public abstract string UpdateConsole(StringBuilder sb);
		}
	}
}