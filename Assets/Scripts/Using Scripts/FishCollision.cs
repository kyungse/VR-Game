using UnityEngine;
using System.Collections;

public class FishCollision : MonoBehaviour {
	public GameObject particle;
	public GameObject particle2;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Player")) {
			if(gameObject.CompareTag("Tiny")) {
				nonenemy();
				FishMaker.destroyedFish[0]+=1;
				GM.instance.fishEaten(0);
			}
			else if(gameObject.CompareTag("Small")) {
				if(FishMaker.stage > 0) {
					nonenemy();
					FishMaker.destroyedFish[1]+=1;
					GM.instance.fishEaten (1);
				}
				else
					enemy (other);
			}
			else if(gameObject.CompareTag("Big")) {
				if (FishMaker.stage > 1){
					nonenemy();
					FishMaker.destroyedFish[2]+=1;
					GM.instance.fishEaten (2);
				}
				else
					enemy(other);
			}
			else if(gameObject.CompareTag("Large")) {
				if (FishMaker.stage > 2){
					nonenemy();
					FishMaker.destroyedFish[3]+=1;
					GM.instance.fishEaten (3);
				}
				else
					enemy(other);
			}
		}
	}

	void nonenemy() {
		Destroy(gameObject);
		Destroy(gameObject.transform.parent.gameObject);
		FishMaker.fishNum--;
		Instantiate (particle, transform.position, transform.rotation);
	}
	void enemy(Collider other) {
		//other.transform.parent.gameObject.SetActive(false);
		Instantiate (particle2, other.transform.position, other.transform.rotation);
	}
}
