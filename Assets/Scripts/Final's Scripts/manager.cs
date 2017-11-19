using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manager : MonoBehaviour {

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

	void OnTriggerEnter (Collider other) {
		// if other is exit, change check value
		if (other.CompareTag ("Exit")) {
			check = true;
			// release catched fish again; 
			other.gameObject.transform.position = new Vector3 (200.0f, 200.0f, 200.0f);
			HoldedFish.gameObject.GetComponent<CatmullromSplineMovement> ().enabled = true;
			HoldedFish.gameObject.GetComponent<BoxCollider> ().enabled = true;
			HoldedFish.transform.Find ("specific").gameObject.SetActive (false);
			HoldedFish.transform.rotation = Quaternion.Euler (0.0f, 0.0f, 0.0f);
		} else {
			// Catch the fish code
			// if check is true it's catching fish mode
			if (check) {
				check = false;
				HoldedFishTag = int.Parse(other.gameObject.tag);
				HoldedFish = other.gameObject;
				GameSetting.cathced_check [HoldedFishTag] = 1;
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

}
