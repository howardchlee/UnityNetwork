using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public GameObject playerObject = null;
	private bool inPlay = false;

	// Use this for initialization
	void Start () {

	}
	
	public void setPlayerObject(GameObject playerObject)
	{
		this.playerObject = playerObject;
		this.inPlay = true;
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
			this.playerObject.GetComponent<PlayerObjectScript>().PublicChangeColor(0f, 1f, 0f);
		}


	}


}


