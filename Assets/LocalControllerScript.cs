using UnityEngine;
using System.Collections;

public class LocalControllerScript : MonoBehaviour {

	public GameObject networkControllerPrefab;

	// Use this for initialization
	void Start () {
		Initialize();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public GameObject Initialize()
	{
		return (GameObject)Instantiate(networkControllerPrefab);
	}
}
