using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	public float yspeed = 10;
	public bool serverBullet;

	// Use this for initialization
	void Start () {
	
	}

	public void SetIsServerBullet(int b)
	{
		networkView.RPC ("SetIsServerBulletRPC", RPCMode.AllBuffered, b);
	}

	[RPC]
	void SetIsServerBulletRPC(int b)
	{
		this.serverBullet = (b==1);
		//Debug.Log ("serverBullet = " + serverBullet.ToString());
	}
	
	// Update is called once per frame
	void Update () {
		int dir = (this.serverBullet)? -1:1;
		transform.Translate(0, dir*yspeed*Time.deltaTime, 0);
		if(this.transform.position.y < -13f || this.transform.position.y > 13f)
		{
			if(Network.isServer)
				Network.Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter(Collider c)
	{
		if(Network.isClient)
			return;
		if(c.gameObject.name != "playerObject(Clone)")
			return;

		c.gameObject.GetComponent<PlayerObjectScript>().player.ReduceHealthByPublic(20);
		Network.Destroy(this.gameObject);

	}
}
