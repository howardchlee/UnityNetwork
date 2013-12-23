using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public GameObject playerObject = null;
	private bool inPlay = false;

	// Use this for initialization
	void Start () {

	}


	public string s = "";

	// Update is called once per frame
	void Update () {
		if(!this.inPlay)
			return;
		if(!playerObject.networkView.isMine)
			return;
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			playerObject.transform.Translate(-20f*Time.deltaTime, 0, 0);
		}

		if(Input.GetKey(KeyCode.RightArrow))
		{
			playerObject.transform.Translate (20f*Time.deltaTime, 0,0);
		}

		if(Input.GetKey(KeyCode.UpArrow))
		{
			playerObject.transform.Translate (0, 20f*Time.deltaTime, 0);
		}

		if(Input.GetKey(KeyCode.DownArrow))
		{
			playerObject.transform.Translate(0, -20f*Time.deltaTime, 0);
		}

		if(playerObject.transform.position.x < -11)
		{
			playerObject.transform.position = new Vector3(-11, playerObject.transform.position.y, -0.5f);
		}
		else if(playerObject.transform.position.x > 11)
		{
			playerObject.transform.position = new Vector3(11, playerObject.transform.position.y, -0.5f);
		}

		if(playerObject.transform.position.y < -11)
		{
			playerObject.transform.position = new Vector3(playerObject.transform.position.x, -11, -0.5f);
		}
		else if(playerObject.transform.position.y > 11f)
		{
			playerObject.transform.position = new Vector3(playerObject.transform.position.x, 11f, -0.5f);;
		}

		if(Input.GetKey (KeyCode.F))
		{
			Quaternion c = new Quaternion(0.0f, 1.0f, 0.0f, 1.0f);
			this.networkView.RPC ("setGameObjectColorToRPC", RPCMode.AllBuffered, c);
		}

	}

	// RPC callers
	public void setGameObjectTo(NetworkViewID viewId)
	{
		this.networkView.RPC ("setGameObjectToRPC", RPCMode.AllBuffered, viewId);
	}


	// RPC functions
	[RPC]
	public void setGameObjectToRPC(NetworkViewID viewId)
	{
		this.playerObject = NetworkView.Find(viewId).gameObject;
		this.inPlay = true;
	}

	[RPC]
	public void setGameObjectColorToRPC(Quaternion c)
	{
		this.playerObject.renderer.material.color = new Color(c.x, c.y, c.z, c.w);
	}
}


