using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GM : MonoBehaviour {
	public Text [] fishText = new Text[4];
	public GameObject [] fishObject = new GameObject[4];
	public string [] fishName = {"tiny", "small", "big", "large"};
	public static GM instance = null;

	void Awake() {
		if(instance == null) {
			instance= this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}
	}
	// Use this for initialization
	void Start () {
		for(int i =0; i<4; i++) {
			fishText[i] = GameObject.Find(fishName[i]+"num").GetComponent<Text>();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void fishEaten(int fishSize) {
		if(FishMaker.stage < fishSize+1 )
			fishText[fishSize].text = FishMaker.destroyedFish[fishSize]+" / 10";
	}
	public void levelUp(int stage) {
		fishText[stage-1].text = "";
		if(stage<4) {
			fishText[stage].text = "0 / 10";
			fishObject[stage].SetActive(true);
		}
		/*switch(stage){
		case 1:
			tinyText.text = "";
			smallFish.SetActive(true);
			smallText.text = "0 / 10";
			break;
		case 2:
			smallText.text = "";
			bigFish.SetActive(true);
			bigText.text = "0 / 10";
			break;
		case 3:
			bigText.text = "";
			largeFish.SetActive(true);
			largeText.text = "0 / 10";
			break;
		case 4:
			largeText.text = "";
			break;
		}*/
	}
}
