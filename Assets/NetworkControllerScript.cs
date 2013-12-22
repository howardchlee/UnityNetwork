﻿using UnityEngine;
using System.Collections;

public class NetworkControllerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int MaxConnections = 32;
	public int Port = 5001;
	public string GameTypeName = "howardchlee_unique_gamestring";

	public GameObject prefab;
	public GameObject playerPrefab;

	private bool refreshing = false;

	private bool objectCreated = false;

	private HostData[] AvailableGames = new HostData[0];

	public void CreateGame()
	{
		Network.InitializeServer (this.MaxConnections, this.Port, !Network.HavePublicAddress());
		Debug.Log("Server Created!");
		MasterServer.RegisterHost(GameTypeName, "game1", "testing!");
	}

	public void RefreshHostList()
	{
		Debug.Log ("Searching for Games...");
		MasterServer.RequestHostList(GameTypeName);
		this.refreshing = true;
	}

	// message
	public void OnMasterServerEvent(MasterServerEvent mse)
	{
		if(mse == MasterServerEvent.RegistrationSucceeded)
		{
			Debug.Log ("Registered Server!");
		}
	}

	// message 
	public void OnServerInitialized()
	{
		InstantiatePlayerObject();
		this.objectCreated = true;
	}

	public void InstantiatePlayerObject()
	{
		GameObject newPlayer = (GameObject) Network.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, 0);
		GameObject newPlayerObject = (GameObject) Network.Instantiate(prefab, prefab.transform.position, Quaternion.identity, 0);
		Debug.Log (newPlayer.ToString() + " " + newPlayerObject.ToString ());

		newPlayer.GetComponent<PlayerScript>().setPlayerObject(newPlayerObject);
	}

	public void OnGUI()
	{
		if(!Network.isClient && !Network.isServer)
		{
			if(GUI.Button(new Rect(10, 10, 120, 40), "New Game"))
			{
				CreateGame ();
			}
			if(GUI.Button (new Rect(10, 60, 120, 40), "Find Games"))
			{
				RefreshHostList ();
			}

			if(this.refreshing)
			{
				if(MasterServer.PollHostList().Length > 0)
				{
					refreshing = false;
					this.AvailableGames = MasterServer.PollHostList();
					Debug.Log ("Found Available Games.");
				}
			}

			for(int i = 0; i < this.AvailableGames.Length; i++)
			{
				if(GUI.Button (new Rect(20, 110 + i*50, 150, 40), this.AvailableGames[i].gameName))
				{
					Network.Connect (this.AvailableGames[i]);
				}
			}
		}

		if(Network.isClient && !this.objectCreated)
		{
			objectCreated = true;
			InstantiatePlayerObject();
		}
	}
}
