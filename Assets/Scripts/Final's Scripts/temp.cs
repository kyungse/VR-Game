using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class temp : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		Invoke ("SetSound", 3.5f);
		Invoke ("MoveToGameScene", 10.0f);

	}

	// Update is called once per frame
	void Update () {
		
	}

	private void SetSound() {
		GameObject.FindGameObjectWithTag ("movie").GetComponent<AudioSource>().Play();
	}

	private void MoveToGameScene() {
		SceneManager.LoadScene ("Final");
	}
}
