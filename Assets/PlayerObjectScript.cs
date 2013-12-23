using UnityEngine;
using System.Collections;

public class PlayerObjectScript : MonoBehaviour {

	public PlayerScript player = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 	{
	
	}

	public void AssignPlayer(NetworkViewID vid)
	{
		this.networkView.RPC ("AssignPlayerRPC", RPCMode.AllBuffered, vid);
	}

	[RPC]
	public void AssignPlayerRPC(NetworkViewID vid)
	{
		this.player = NetworkView.Find (vid).gameObject.GetComponent<PlayerScript>();
	}
}
