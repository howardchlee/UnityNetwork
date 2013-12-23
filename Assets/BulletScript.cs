using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	public float yspeed = 10;
	public bool serverBullet;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		int dir = (this.serverBullet)? -1:1;
		transform.Translate(0, dir*yspeed*Time.deltaTime, 0);
		if(this.transform.position.y < -13f || this.transform.position.y > 13f)
		{
			Destroy (this.gameObject);
		}
	}
}
