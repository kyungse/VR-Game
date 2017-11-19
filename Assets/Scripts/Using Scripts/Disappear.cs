using UnityEngine;
using System.Collections;

public class Disappear : MonoBehaviour {
	private float startTime;
	private float currTime;
	public GameObject player;
	// Use this for initialization
	void Start () {
		Destroy (gameObject, 3);
		startTime = Time.realtimeSinceStartup;
		player = GameObject.Find("Aerowand Hand");
	}
	
	// Update is called once per frame
	void Update () {
		currTime = Time.realtimeSinceStartup;
		if(currTime - startTime > 2.8f && gameObject.CompareTag("Enemy")) {
			player.transform.GetChild(0).gameObject.SetActive(true);
		}
	}
}
