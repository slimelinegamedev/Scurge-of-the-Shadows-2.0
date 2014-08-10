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
using Scurge.Scoreboard;

namespace Scurge.Networking {
	public class Server : MonoBehaviour {

		public string IP;
		public int Port;
		public int MaxPlayers;

		public string ConnectionIP;
		public string ConnectionPort;

		public GameObject TestObject;

		//Unity
		void OnGUI() {
			ConnectionIP = GUILayout.TextField(ConnectionIP);
			ConnectionPort = GUILayout.TextField(ConnectionPort);
			if(GUILayout.Button("Connect")) {
				Join(ConnectionIP, int.Parse(ConnectionPort));
			}
			if(GUILayout.Button("Create")) {
				Create(MaxPlayers, Port, IP);
			}
			if(GUILayout.Button("Create Cube")) {
				var LastTorch = (GameObject)Network.Instantiate(TestObject, new Vector3(0, 2, 0), transform.rotation, 0);
			}
		}
		//Networking
		void OnConnectedToServer() {
			print("Connected!");
		}
		void OnFailedToConnect(NetworkConnectionError error) {
			print("Could not connect to server: " + error);
		}
		//Custom
		public void Create(int maxConnections, int port, string address) {
			print("Starting server on " + address + ":" + port);
			bool useNat = !Network.HavePublicAddress();
        			Network.InitializeServer(maxConnections, port, useNat);			
		}
		public void Join(string ip, int port) {
			print("Connecting... ");
			Network.Connect(ip, port);
		}
	}
}