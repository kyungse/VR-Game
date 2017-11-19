using UnityEngine;
using System.Collections;

public class Fish4 : MonoBehaviour {
	//attached to main fish
	//set material
	//set trigger event.
		
	private float fishSize;
	private int currentStage;
	public static int life = 5;
	private Rigidbody parentRB;
	private Rigidbody rb;
	//public GameObject particle;

	// Use this for initialization	
	void Start () {
		fishSize = 3f;
		currentStage=0;
		parentRB = gameObject.GetComponentInParent<Rigidbody>();
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("Horizontal") < 0)
			transform.localRotation = Quaternion.Euler(new Vector3(0, 270f, 0));
		else if (Input.GetAxis ("Horizontal") > 0)
			transform.localRotation = Quaternion.Euler(new Vector3(0, 90f, 0));
		if(currentStage != FishMaker.stage) {
			currentStage = FishMaker.stage;
			switch(currentStage){
			case 0:
				fishSize = 2f;
				break;
			case 1:
				fishSize = 2.3f;
				break;
			case 2:
				fishSize = 2.7f;
				break;
			case 3:
				fishSize = 3f;
				break;
			}
			transform.localScale=fishSize*Vector3.one;
		}
		
		
	}
	private void panelty() {
		if(rb.isKinematic==false && parentRB.angularVelocity==Vector3.zero) {
			rb.isKinematic=true;
			parentRB.isKinematic=true;
		}
	}
	/*void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Tiny"))
			FishMaker.destroyedFish[0]+=1;
		else if (other.gameObject.CompareTag("Small"))
			FishMaker.destroyedFish[1]+=1;
		else if (other.gameObject.CompareTag("Big"))
			FishMaker.destroyedFish[2]+=1;
		else if (other.gameObject.CompareTag("Large"))
			FishMaker.destroyedFish[3]+=1;
		Destroy (other.gameObject.transform.parent.gameObject, 5);
		Destroy (other.gameObject);
		FishMaker.fishNum--;
		Instantiate (particle, other.transform.position, other.transform.rotation);
	}
	*/
	/*void OnCollisionEnter(Collision others) {
		if(others.gameObject.CompareTag ("Tiny") || others.gameObject.CompareTag ("Small") ||others.gameObject.CompareTag ("Big") ||others.gameObject.CompareTag ("Large")){
			print ("1");
			print (others.contacts[0].point);
			rb.isKinematic=false;
			parentRB.isKinematic = false;
			parentRB.AddTorque (transform.position-others.contacts[0].point);
			Destroy (others.gameObject.transform.parent.gameObject);
			Destroy (others.gameObject);
			FishMaker.fishNum--;
			life--;
		}
	}*/
}
