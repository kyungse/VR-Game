using UnityEngine;
using System.Collections;

public class NetControl : MonoBehaviour {
	private float netSize;
	private int currentStage;
	public static int life = 5;
	// Use this for initialization
	void Start () {
		netSize = 0.4f;
		currentStage=0;
	}
	
	// Update is called once per frame
	void Update () {
		if(currentStage != FishMaker.stage) {
			currentStage = FishMaker.stage;
			switch(currentStage){
			case 0:
				netSize = 0.4f;
				break;
			case 1:
				netSize = 0.65f;
				break;
			case 2:
				netSize = 0.9f;
				break;
			case 3:
				netSize = 1.15f;
				break;
			}
			transform.localScale=netSize*Vector3.one;
		}
	}
}
