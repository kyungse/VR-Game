using UnityEngine;
using System.Collections;

public class FishController : MonoBehaviour {
	//attached to fishrotator.
	//randomly make fish and set speed of it.
	//set material of child(fish).

	private Vector3 rndAngle;
	public float minSpeed=0.5f;
	public float maxSpeed=1.2f;
	private float fishVelocity;
	private float fishDirection;
	private float xPos;
	public Material[] materials = new Material[2];
	private bool setMat = false;
	private Renderer rend;
	private float makeTime;
	private float currTime;
	//private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rndAngle = new Vector3(Random.Range(-70f, 70f), Random.Range (-180f, 180f), 0f);
		fishVelocity=Random.Range (minSpeed, maxSpeed);
		rend = transform.GetChild(0).GetComponent<Renderer>();
		fishDirection=Random.value;
		if(fishDirection < 0.5){
			fishDirection=-1f;
		}
		else {
			fishDirection=1f;
		}
		fishVelocity*=fishDirection;
		xPos=rndAngle.x;
		makeTime=Time.time;
		//rb=GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		rndAngle += new Vector3(Random.value, fishVelocity, 0);
		rndAngle = new Vector3(Mathf.Clamp(rndAngle.x, xPos-3f, xPos+3f), rndAngle.y, 0f);	
		transform.eulerAngles=rndAngle;
		if (setMat == false && fishVelocity<0) {
			rend.material=materials[0];
			setMat = true;
		}
		else if (setMat == false && fishVelocity>0) {
			rend.material=materials[1];
			setMat=true;
		}
		currTime=Time.time;
		// && rend.isVisible != true
		if(currTime-makeTime > 15f) {
			Destroy (gameObject);
			FishMaker.fishNum--;
		}
	}

	private void enemyCheck() {
		if (gameObject.CompareTag ("Small") && FishMaker.stage==0){


		}	
	}
}
