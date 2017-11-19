using UnityEngine;
using System.Collections;

//sample!!!!!!!!!!

public class Fish2 : MonoBehaviour {
	//attached to main fish
	//set material
	//set trigger event.

	public Material[] materials = new Material[2];
	private Renderer rend;
	private float fishSize;
	private int currentStage;
	public static int life = 5;

	// Use this for initialization	
	void Start () {
		rend = GetComponent<Renderer>();
		fishSize = 2f;
		currentStage=0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("Horizontal") < 0)
			rend.material=materials[0];
		else if (Input.GetAxis ("Horizontal") > 0)
			rend.material=materials[1];
		if(currentStage != FishMaker.stage) {
			currentStage = FishMaker.stage;
			switch(currentStage){
			case 0:
				fishSize = 2f;
				break;
			case 1:
				fishSize = 2.8f;
				break;
			case 2:
				fishSize = 3.6f;
				break;
			case 3:
				fishSize = 4.4f;
				break;
			}
			transform.localScale=new Vector3(fishSize*1f, fishSize*0.9f, 1f);
		}


	}
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.transform.parent.gameObject.CompareTag("Tiny"))
			FishMaker.destroyedFish[0]+=1;
		else if (other.gameObject.transform.parent.gameObject.CompareTag("Small"))
			FishMaker.destroyedFish[1]+=1;
		else if (other.gameObject.transform.parent.gameObject.CompareTag("Big"))
			FishMaker.destroyedFish[2]+=1;
		else if (other.gameObject.transform.parent.gameObject.CompareTag("Large"))
			FishMaker.destroyedFish[3]+=1;
		Destroy (other.gameObject.transform.parent.gameObject);
		Destroy (other.gameObject);
		FishMaker.fishNum--;
	}
	void OnCollisionEnter(Collision others) {
		Destroy (others.gameObject.transform.parent.gameObject);
		Destroy (others.gameObject);
		FishMaker.fishNum--;
		life--;
	}
}
