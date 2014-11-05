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

	public enum TileType {
		Regular,
		Boss,
		WayDown,
		FromAbove,
		DownAndAbove,
		Chest
	}
	[System.Serializable]
	public class Tile {
		public string name;
		public GameObject tileObject;
		public TileType tileType;
		[Range(1, 1000)]
		public int spawnRate = 1;
	}

	public class Dungeon : MonoBehaviour {

		public Objects Objects;
		public Spawner Spawner;
		public Disable Disable;
		public Pause Pause;
		public List<Tile> Tiles;
		public List<GameObject> SpawnedTiles;
		public List<DungeonProp> Props;
		public GameObject Wall;
		public GameObject Holder;
		public GameObject meshHolder;
		public float WaitTime;
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
		public TextMesh loadingTextMesh;
		public GameObject loadingGameObject;

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
			if(Generating) {
				loadingTextMesh.text = GenerationText;
				loadingGameObject.SetActive(true);
				Disable.DisableObj(false, true);
			}
			else if(loadingGameObject.activeInHierarchy && !Generating) {
				Disable.EnableObj(true, true);
				loadingGameObject.SetActive(false);
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
			bool spawnedFirstTile = false;
			while(!spawnedFirstTile) {
				PieceNumber = Random.Range(0, Tiles.Count);
				int rarityChosen = Random.Range(1, 1000);
				if(Tiles[PieceNumber].spawnRate < rarityChosen && Tiles[PieceNumber].tileType == TileType.DownAndAbove) {
					CurrentPiece = Tiles [PieceNumber].tileObject;
					var CurTile = (GameObject)Instantiate(CurrentPiece, new Vector3(0, -20, 0), Quaternion.identity);
					CurTile.transform.parent = Holder.transform;
					SpawnedTiles.Add(CurTile);
					spawnedFirstTile = true;
				}
				else {
					spawnedFirstTile = false;
				}
			}
			for(int x = 0; x < SizeX * TileSize; x += TileSize) {
				for(int y = 0; y < SizeZ * TileSize; y += TileSize) {
					if(y > 0 || x > 0) {
						bool chosen = false;

						while(!chosen) {
							PieceNumber = Random.Range(0, Tiles.Count);
							int rarityChosen = Random.Range(1, 1000);
							if(Tiles[PieceNumber].spawnRate < rarityChosen && Tiles[PieceNumber].tileType != TileType.DownAndAbove && Tiles[PieceNumber].tileType != TileType.FromAbove) {
								CurrentPiece = Tiles [PieceNumber].tileObject;
								var CurTile = (GameObject)Instantiate(CurrentPiece, new Vector3(x, -20, y), Quaternion.identity);
								CurTile.transform.parent = Holder.transform;
								SpawnedTiles.Add(CurTile);
								chosen = true;
							}
							else {
								chosen = false;
							}
						}	
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

		public IEnumerator BeginGeneration() {
			Objects.HUD.SetActive(false);
			GenerationText = "Floor " + Floor;
			Generating = true;
			yield return new WaitForSeconds(1);
			Generate();
			yield return new WaitForSeconds(WaitTime);
			Objects.HUD.SetActive(true);
			Generating = false;
		}

		public IEnumerator Generation(float waiter) {
			Objects.HUD.SetActive(false);
			print("Generation");
			GenerationText = "Floor " + Floor;
			Generating = true;
			yield return new WaitForSeconds(waiter);
			Generate();
			yield return new WaitForSeconds(waiter * 4);
			Objects.HUD.SetActive(true);
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