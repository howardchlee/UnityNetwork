using UnityEngine;
using System.Collections;

public class PlayerObjectScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 	{
	
	}

	public void PublicChangeColor(float r, float g, float b)
	{
		networkView.RPC ("ChangeColor", RPCMode.AllBuffered, r, g, b);
	}

	[RPC]
	void ChangeColor(float r, float g, float b)
	{
		//networkView.renderer.material.color = new Color(r, g, b);
		this.renderer.material.color = new Color(r, g, b);
	}
}
