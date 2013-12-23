using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public GameObject playerObject = null;
	private bool inPlay = false;

	// Use this for initialization
	void Start () {

	}

	public int health = 100;
	public string s = "";

	public Texture2D progressBarBg;
	public Texture2D progressBarFull;

	void OnGUI()
	{
		Vector3 worldPos = new Vector3(this.playerObject.transform.position.x, this.playerObject.transform.position.y, -0.5f);
		Vector3 healthBarPos = Camera.current.WorldToScreenPoint(worldPos);
		Debug.Log (healthBarPos.ToString());
		GUI.Box (new Rect(healthBarPos.x - 20.0f, Screen.height - healthBarPos.y - 70.0f, 100.0f, 16.0f), progressBarBg);
		GUI.Box (new Rect(healthBarPos.x - 20.0f, Screen.height - healthBarPos.y - 70.0f, 100.0f * health / 100.0f, 16.0f), progressBarFull);
	}

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

		if(Network.isServer)
		{
			if(playerObject.transform.position.y < 2.5f)
			{
				playerObject.transform.position = new Vector3(playerObject.transform.position.x, 2.5f, -0.5f);
			}
			else if(playerObject.transform.position.y > 11f)
			{
				playerObject.transform.position = new Vector3(playerObject.transform.position.x, 11f, -0.5f);;
			}
		}
		else
		{
			if(playerObject.transform.position.y < -11)
			{
				playerObject.transform.position = new Vector3(playerObject.transform.position.x, -11, -0.5f);
			}
			else if(playerObject.transform.position.y > -2.5f)
			{
				playerObject.transform.position = new Vector3(playerObject.transform.position.x, -2.5f, -0.5f);;
			}
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

	public void toggleInPlay()
	{
		this.networkView.RPC ("toggleInPlayRPC", RPCMode.AllBuffered);
	}

	// RPC functions
	[RPC]
	public void setGameObjectToRPC(NetworkViewID viewId)
	{
		this.playerObject = NetworkView.Find(viewId).gameObject;
	}

	[RPC]
	public void toggleInPlayRPC()
	{
		this.inPlay = !this.inPlay;
	}

	[RPC]
	public void setGameObjectColorToRPC(Quaternion c)
	{
		this.playerObject.renderer.material.color = new Color(c.x, c.y, c.z, c.w);
	}
}


