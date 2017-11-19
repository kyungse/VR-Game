﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FishInfoShow : MonoBehaviour {
	public static int cnt = 0;
	public static int stage = 0;
	public Vector3 pos;
	public Vector3 temp;
	public Vector3 head;
	public Vector3 head_pos;
	public int HoldedFishTag;
	public bool check = true;
	public GameObject HoldedFish;
	FishInfoClass Infos = new FishInfoClass();
	public float[] catched_x = new float[15];
	public float catched_z = 37.0f;
	public float catched_y = -2.2f;
	private float origin_time = 10.0f;
	private float done = 10.0f;

	// Use this for initialization
	void Start () {
		//set the catched fish's position
		catched_x [0] = -25.0f;

		for (int i = 1; i < 15; i++) {
			catched_x [i] = catched_x [i-1] - 1.5f;
			Debug.Log ("catched_x [" + i + "]: " + catched_x [i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (done > 0f) {
			done -= Time.deltaTime;
			GameObject.Find ("Timer").GetComponent<TextMesh> ().text = done.ToString ("N2") + " Sec";
		} else {
			done = 0f;
			GameObject.Find ("Timer").GetComponent<TextMesh> ().text = done.ToString ("N2") + " Sec";
			pos = new Vector3 (15.5f, 4.0f, 16.0f);
			head_pos = GameObject.Find ("Aerowand Head").transform.position;
			GameObject.Find ("EndText").transform.position = head_pos + pos;
			Invoke ("CloseEndMessage", 3.0f);
		}
		GameObject.Find ("Score").GetComponent<TextMesh> ().text = "Count : " + cnt.ToString () + " / 15";
		GameObject.Find ("StageText").GetComponent<TextMesh> ().text = "# STAGE " + (stage+1).ToString ();
	}

	void OnTriggerEnter (Collider other) {
		// if other is exit, change check value
		if (other.CompareTag ("Exit")) {
			Debug.Log ("Exit Mode Entered");
			check = true;
			//turn off the ui mode
			other.gameObject.transform.position = new Vector3 (200.0f, 200.0f, 200.0f);
			HoldedFish.transform.position = new Vector3 (catched_x[HoldedFishTag], catched_y, catched_z);
			HoldedFish.transform.Find ("specific").gameObject.SetActive (false);
			HoldedFish.transform.rotation = Quaternion.Euler (0.0f, 0.0f, 0.0f);
			// if all fishes are catched when stage number != 2 -> next level;
			if (cnt == 15 && stage != 2) {
				done = origin_time;				// timer reset;
				cnt = 0;
				stage++;
				pos = new Vector3 (15.5f, 3.0f, 16.0f);
				head_pos = GameObject.Find ("Aerowand Head").transform.position;
				GameObject.Find ("NextStage").transform.position = head_pos + pos;
				Invoke ("CloseNextLevelMessage", 3.0f);
				for (int i = 0; i < 15; i++) {
					GameObject temp = GameObject.FindGameObjectWithTag (i.ToString ());
					temp.gameObject.GetComponent<CatmullromSplineMovement> ().enabled = true;
					temp.gameObject.GetComponent<BoxCollider> ().enabled = true;
				}
			} 
			// if all fishes are cathed when stage number == 2 -> game is done;
			else if (cnt == 15 && stage == 2) {
				pos = new Vector3 (15.5f, 4.0f, 16.0f);
				head_pos = GameObject.Find ("Aerowand Head").transform.position;
				GameObject.Find ("EndText").transform.position = head_pos + pos;
			}
		} else {
			// Catch the fish code
			// if check is true it's catching fish mode
			if (check) {
				cnt++; // The Number of Cathed Fish;
				HoldedFishTag = int.Parse(other.gameObject.tag);
				HoldedFish = other.gameObject;
				GameSetting.cathced_check [HoldedFishTag] = 1;
				Debug.Log (HoldedFishTag);
				check = false;
				other.transform.Find ("NewText").gameObject.SetActive (true);
				//show the information of the fish
				other.transform.Find ("specific").gameObject.SetActive (true);
				// move fish from own position to Aerowand Head's front = expand size;
				other.gameObject.GetComponent<CatmullromSplineMovement> ().enabled = false;
				other.gameObject.GetComponent<BoxCollider> ().enabled = false;
				head = GameObject.Find ("Aerowand Head").transform.position;
				temp = new Vector3 (2.0f, 0.0f, 1.5f);
				other.gameObject.transform.position = head + temp;
				other.gameObject.transform.rotation = Quaternion.Euler (-30.0f, 0.0f, 0.0f);
				// after fish catched, the exit sphere should be appeared
				GameObject.Find("ExitSphere").transform.position = head + new Vector3(1.0f, -0.46f, -0.01f);
				GameObject.Find("ExitSphere").transform.rotation = Quaternion.Euler (0.0f, 0.0f, 0.0f);

				//show the information of the fish
				if (HoldedFishTag < 3) {
					//Red
					other.transform.Find("specific").gameObject.GetComponent<TextMesh> ().text = Infos.Exp[0];
				} else if (3 <= HoldedFishTag && HoldedFishTag < 6) {
					//Yellow04
					other.transform.Find("specific").gameObject.GetComponent<TextMesh> ().text = Infos.Exp[1];
				} else if (6 <= HoldedFishTag && HoldedFishTag < 9) {
					//Yellow05
					other.transform.Find("specific").gameObject.GetComponent<TextMesh> ().text = Infos.Exp[2];
				} else if (9 <= HoldedFishTag && HoldedFishTag < 12) {
					//Yellow07
					other.transform.Find("specific").gameObject.GetComponent<TextMesh> ().text = Infos.Exp[3];
				} else {
					//Yellow09
					other.transform.Find("specific").gameObject.GetComponent<TextMesh> ().text = Infos.Exp[4];
				}
			}
		}
	}

	private void CloseNextLevelMessage() {
		GameObject.Find ("NextStage").transform.position = new Vector3(299.0f, 299.0f, 299.0f);
	}

	private void CloseEndMessage() {
		GameObject.Find ("EndText").transform.position = new Vector3 (200.0f, 200.0f, 200.0f);
		Debug.Log ("load scene");
		SceneManager.LoadScene ("aquarium");
	}
}
