using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manager : MonoBehaviour {

	private int once = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (once == 0) {
			release ();
		}
	}

	private void release () {
		once = 1;
		for (int i = 0; i < 15; i++) {
			GameObject temp = GameObject.FindGameObjectWithTag (i.ToString ());
			if (GameSetting.cathced_check [i] == 1) {
				temp.gameObject.GetComponent<CatmullromSplineMovement> ().enabled = true;
				temp.gameObject.GetComponent<BoxCollider> ().enabled = true;
			} else {
				temp.SetActive (false);
			}
		}
	}
}
