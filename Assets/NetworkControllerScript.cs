using UnityEngine;
using System.Collections;

public class NetworkControllerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int MaxConnections = 1;
	public int Port = 5002;
	public string GameTypeName = "howardchlee_unique_gamestring_test";

	public GameObject prefab;
	public GameObject playerPrefab;

	private bool refreshing = false;

	private HostData[] AvailableGames = new HostData[0];

	private PlayerScript thisPlayer;
	private System.Random r = new System.Random();

	public void CreateGame()
	{
		int port = r.Next () %1000 + 5002;  // to prevent port collision if host spawns multiple games.
		Network.InitializeServer (this.MaxConnections, port, !Network.HavePublicAddress());
		Debug.Log("Server Created!");

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
		else
		{
			Debug.Log (mse.ToString());
		}
	}

	// message 
	public void OnServerInitialized()
	{
		int salt = r.Next() %1000;
		Debug.Log (salt);

		// randomize game name
		MasterServer.RegisterHost(GameTypeName, "game" + salt.ToString());
		InstantiatePlayerObject();
	}

	// message
	public void OnConnectedToServer()
	{
		Debug.Log (Network.connections.Length + " " + Network.maxConnections);
		if(Network.connections.Length > Network.maxConnections+1)
		{
			Network.Disconnect();
			return;
		}
		InstantiatePlayerObject();
		this.thisPlayer.toggleInPlay();
	}

	public void OnPlayerConnected()
	{
		this.thisPlayer.toggleInPlay();
	}

	void OnFailedToConnect(NetworkConnectionError error) {
		Debug.Log("Could not connect to server: " + error);
	}

	// message
	public void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		Application.Quit ();
	}

	// message
	public void OnPlayerDisconnected(NetworkPlayer player)
	{
		Debug.Log("Player " + player.ToString() + " has disconnected.");
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects (player);
	}
	

	public void InstantiatePlayerObject()
	{
		Vector3 spawnPosition = Vector3.zero;
		if(Network.isServer)
		{
			spawnPosition = new Vector3(0, 8f, -0.5f);
		}
		else
		{
			spawnPosition = new Vector3(0, -8f, -0.5f);
		}

		GameObject newPlayer = (GameObject) Network.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, 0);
		GameObject newPlayerObject = (GameObject) Network.Instantiate(prefab, spawnPosition, Quaternion.identity, 0);
		Debug.Log (newPlayer.ToString() + " " + newPlayerObject.ToString ());
		NetworkViewID objectVID = newPlayerObject.networkView.viewID;
		NetworkViewID playerVID = newPlayer.networkView.viewID;


		newPlayerObject.GetComponent<PlayerObjectScript>().AssignPlayer(playerVID);

		this.thisPlayer = newPlayer.GetComponent<PlayerScript>();
		thisPlayer.setGameObjectTo(objectVID);
	}

	public void OnGUI()
	{
		if(Network.isServer)
		{
			GUI.Box (new Rect(10, 10, 80, 40), Network.connections.Length.ToString());
		}

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
				HostData thisGame = this.AvailableGames[i];
				if(thisGame.connectedPlayers >= this.MaxConnections+1)
				{
					GUI.Button (new Rect(20, 110 + i*50, 150, 40), thisGame.gameName + "(FULL)");
				}
				else if(GUI.Button (new Rect(20, 110 + i*50, 150, 40), thisGame.gameName + "(" + thisGame.connectedPlayers + "/" + thisGame.playerLimit + ")"))
				{
					Network.Connect (thisGame);
				}
			}
		}
		else
		{
			GUI.Box(new Rect(10, 10, 120, 40), this.thisPlayer.s);
		}
	}
}
