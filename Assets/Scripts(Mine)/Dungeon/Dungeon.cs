using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scurge;
using Scurge.AI;
using Scurge.Audio;
using Scurge.Enemy;
using Scurge.Environment;
using Scurge.Networking;
using Scurge.Player;
using Scurge.Scoreboard;
using Scurge.UI;
using Scurge.Util;

namespace Scurge.Environment {

	#region Enums
	public enum PropSize {
		//X Y Z
		OneOneOne,
		TwoTwoTwo,
		TwoOneOne,
		OneTwoOne,
		OneOneTwo,
		TwoTwoOne,
		OneTwoTwo
	}
	#endregion

	//TODO: Decrease boss tile spawn rates before release

	public class Dungeon : MonoBehaviour {

		public Objects Objects;
		public Spawner Spawner;
		public List<GameObject> Tiles;
		public List<GameObject> BossTiles;
		public List<GameObject> WaysDown;
		public List<GameObject> SpawnedTiles;
		public List<DungeonProp> Props;
		public GameObject Wall;
		public GameObject Holder;
		public GameObject meshHolder;
		public float WaitTime;
		public GUISkin Skin;
		private bool FirstTime = true;
		public string GenerationText = "Generating New\nDungeon";
		public int Floor = 0;
		public int TileSize = 16;
		public int SizeX = 3;
		public int SizeZ = 3;
		public int PieceNumber;
		public GameObject CurrentPiece;
		public bool Generating = false;
		public bool entered = false;
		public bool firstTime = true;

		void Awake() {

		}

		void Start() {
			if(TileSize > 0) {
				if(FirstTime) {
					StartCoroutine(BeginGeneration());
					FirstTime = false;
				}
			} else {
				Debug.LogError("Tile Size must not be 0");
			}
		}

		void Update() {
			if(firstTime) {
				Floor += 1;
				firstTime = false;
			}
		}

		public void Generate() {
			int Type = Random.Range(0, 1000);
			if(SpawnedTiles.Count > 0) {
				foreach(GameObject curTileDestroying in SpawnedTiles) {
					Destroy(curTileDestroying);
				}
			}
			foreach(Transform child in meshHolder.transform) {
				if(child != null) {
					Destroy(child.gameObject);
				}
			}
			SpawnedTiles = new List<GameObject>(0);
			for(int x = 0; x < SizeX * TileSize; x += TileSize) {
				for(int y = 0; y < SizeZ * TileSize; y += TileSize) {
					if(y > 0 || x > 0) {
						Type = Random.Range(0, 50);
						if(Type < 49) {
							PieceNumber = Random.Range(0, Tiles.Count);
							CurrentPiece = Tiles [PieceNumber];
							var CurTile = (GameObject)Instantiate(CurrentPiece, new Vector3(x, -20, y), Quaternion.identity);
							CurTile.transform.parent = Holder.transform;
							SpawnedTiles.Add(CurTile);
						} else {	
							PieceNumber = Random.Range(0, BossTiles.Count);
							CurrentPiece = BossTiles [PieceNumber];
							var CurTile = (GameObject)Instantiate(CurrentPiece, new Vector3(x, -20, y), Quaternion.identity);
							CurTile.transform.parent = Holder.transform;
							SpawnedTiles.Add(CurTile);
						}
					} else {
						PieceNumber = Random.Range(0, WaysDown.Count);
						CurrentPiece = WaysDown [PieceNumber];
						var CurTile = (GameObject)Instantiate(CurrentPiece, new Vector3(x, -20, y), Quaternion.identity);
						CurTile.transform.parent = Holder.transform;
						SpawnedTiles.Add(CurTile);
					}
				}
			}
			Walls();
			Spawner.Spawn();
			Teleport(new Vector3(7.499434f, -5.875009f, -7.074736f));
		}

		public void Walls() {

			int posX = -5;
			int posZ = -20;

			for(int allWalls = 0; allWalls < 5; allWalls++) {
				var lastWall = (GameObject)Instantiate(Wall, new Vector3(posX, -20.011f, -30), transform.rotation);
				lastWall.transform.rotation = Quaternion.Euler(0, 180, 0);
				posX += 16;
				lastWall.transform.parent = Holder.transform;
				SpawnedTiles.Add(lastWall);
			}
			posX = -5;
			for(int allWalls = 0; allWalls < 5; allWalls++) {
				var lastWall = (GameObject)Instantiate(Wall, new Vector3(posX, -20.011f, 49), transform.rotation);
				lastWall.transform.rotation = Quaternion.Euler(0, 180, 0);
				posX += 16;
				lastWall.transform.parent = Holder.transform;
				SpawnedTiles.Add(lastWall);
			}
			for(int allWalls = 0; allWalls < 5; allWalls++) {
				var lastWall = (GameObject)Instantiate(Wall, new Vector3(15, -20.011f, posZ), transform.rotation);
				lastWall.transform.rotation = Quaternion.Euler(0, 90, 0);
				posZ += 16;
				posX += 16;
				lastWall.transform.parent = Holder.transform;
				SpawnedTiles.Add(lastWall);
			}
			posZ = -20;
			for(int allWalls = 0; allWalls < 5; allWalls++) {
				var lastWall = (GameObject)Instantiate(Wall, new Vector3(94, -20.011f, posZ), transform.rotation);
				lastWall.transform.rotation = Quaternion.Euler(0, 90, 0);
				posZ += 16;
				posX += 16;
				lastWall.transform.parent = Holder.transform;
				SpawnedTiles.Add(lastWall);
			}
		}

		void OnGUI() {
			GUI.depth = 0;
			GUI.skin = Skin;
			if(Generating) {
				GUI.Box(new Rect(0, 0, 1280, 720), GenerationText, "Gen BG");
			}
		}

		public IEnumerator BeginGeneration() {
			GenerationText = "Floor " + Floor + "\n\n<size=20>Loading...</size>";
			Generating = true;
			yield return new WaitForSeconds(1);
			Generate();
			yield return new WaitForSeconds(WaitTime);
			Generating = false;
		}

		public IEnumerator Generation(float waiter) {
			print("Generation");
			GenerationText = "Floor " + Floor + "\n\n<size=20>Loading...</size>";
			Generating = true;
			yield return new WaitForSeconds(waiter);
			Generate();
			yield return new WaitForSeconds(waiter * 4);
			Generating = false;
			yield return new WaitForSeconds(1);
			entered = false;
		}

		public void Teleport(Vector3 position) {
			Objects.Player.transform.position = position;
		}

		public void OutterGeneration() {
			print("OutterGeneration");
			if(!entered && !firstTime) {
				StartCoroutine(Generation(WaitTime));
				Floor += 1;
				entered = true;
			}
		}
	}
}