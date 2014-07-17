using UnityEngine;
using System.Collections;
using Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;


public class TestSerialization : MonoBehaviour
{

	public int saveStateInt;
	public GUISkin Skin;
	
	void OnEnable()
	{
		//LevelSerializer.Progress += HandleLevelSerializerProgress;
	}
	
	void OnDisable()
	{
		//LevelSerializer.Progress -= HandleLevelSerializerProgress;
	}

	static void HandleLevelSerializerProgress (string section, float complete)
	{
		Debug.Log(string.Format("Progress on {0} = {1:0.00%}", section, complete));
	}
	
	
	void OnGUI()
	{

		GUI.skin = Skin;
		
		
		if(GUILayout.Button("Save"))
		{
			saveStateInt += 1;
			//Save the game with a prefix of Game
			var t = DateTime.Now;
			LevelSerializer.SaveGame("Game" + saveStateInt.ToString());
			Radical.CommitLog();
		}
		if(GUILayout.Button("Delete"))
		{
			saveStateInt = 0;
			//Save the game with a prefix of Game
			var t = DateTime.Now;
			LevelSerializer.SavedGames[LevelSerializer.PlayerName].Clear();
			Radical.CommitLog();
		}
		
		//Check to see if there is resume info
		if(LevelSerializer.CanResume)
		{
			if(GUILayout.Button("Resume"))
			{
				LevelSerializer.Resume();
			}
		}
		
		if(LevelSerializer.SavedGames.Count > 0)
		{
			GUILayout.Label("Available saved games");
			//Look for saved games under the given player name
			foreach(var g in LevelSerializer.SavedGames[LevelSerializer.PlayerName])
			{
				if(GUILayout.Button(g.Caption))
				{
					g.Load();
				}
					
			}
		}
	}
	
	// Update is called once per frame
	void Update()
	{

	}
}


