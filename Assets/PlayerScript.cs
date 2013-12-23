using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public GameObject playerObject = null;
	public bool inPlay = false;

	// Use this for initialization
	void Start () {

	}

	public int health = 100;
	public string s = "";

	public GameObject bulletPrefab;

	public void ReduceHealthByPublic(int d)
	{
		networkView.RPC ("ReduceHealthBy", RPCMode.AllBuffered, d);
	}

	[RPC]
	void ReduceHealthBy(int d)
	{
		this.health -= d;
	}

	public void Reset()
	{
		networkView.RPC ("ResetRPC", RPCMode.AllBuffered);
	}

	[RPC]
	void ResetRPC()
	{
		this.health = 100;
		this.transform.position = new Vector3(0, 8f, -0.5f);
	}

	void OnGUI()
	{
		if(!inPlay)
			return;
		try
		{
			/*Vector3 worldPos = new Vector3(this.playerObject.transform.position.x, this.playerObject.transform.position.y, -0.5f);
			Vector3 healthBarPos = Camera.current.WorldToScreenPoint(worldPos);
			GUI.Label (new Rect(healthBarPos.x - 20.0f, Screen.height - healthBarPos.y - 70.0f, 100.0f, 16.0f), progressBarBg);
			Rect healthBar = new Rect(healthBarPos.x - 20.0f, Screen.height - healthBarPos.y - 70.0f, 100.0f * health / 100.0f, 16.0f);
			if(networkView.isMine)
				Debug.Log (healthBar.ToString());
			GUI.Label (healthBar, progressBarFull);*/

			if((Network.isServer && networkView.isMine) || (Network.isClient && !networkView.isMine))
			{
				GUI.Box (new Rect(0f, 0f, Screen.width * health/100.0f, 30), "health");
			}
			else
			{
				GUI.Box (new Rect(0f, Screen.height-30, Screen.width * health/100.0f, 30), "health");
			}
		}
		catch
		{

		}
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

		if(Input.GetKeyDown (KeyCode.Space))
		{
			//Debug.Log ("====================>" + this.playerObject.transform.position.ToString ());
			Vector3 spawnPos = this.playerObject.transform.position;
			spawnPos.Set(spawnPos.x, spawnPos.y + ((Network.isServer)? -2.01f : 2.01f), spawnPos.z);
			GameObject b = (GameObject) Network.Instantiate(bulletPrefab, spawnPos, Quaternion.identity, 0);
			b.GetComponent<BulletScript>().SetIsServerBullet( (Network.isServer)? 1: 0);
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


