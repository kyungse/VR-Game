using UnityEngine;
using System.Collections;

public class FishController2 : MonoBehaviour {
	//attached to fishrotator.
	//randomly make fish and set speed of it.
	//set material of child(fish).
	
	private Vector3 rndAngle;
	private Vector3 fishAngle;
	public float minSpeed=0.5f;
	public float maxSpeed=1.2f;
	private float fishVelocity;
	private float fishDirection;
	private float xPos;
	private float makeTime;
	private float currTime;
	private GameObject child;
	private Rigidbody rb;
	//private Rigidbody childRB;
	
	// Use this for initialization
	void Start () {
		rndAngle = new Vector3(Random.Range(-50f, 20f), Random.Range (-180f, 180f), 0f);
		fishVelocity=Random.Range (minSpeed, maxSpeed);
		fishDirection=Random.value;
		if(fishDirection < 0.5){
			fishDirection=-1f;
			fishAngle = new Vector3(0, -90f, 0);
		}
		else {
			fishDirection=1f;
			fishAngle = new Vector3(0, 90f, 0);
		}
		fishVelocity*=fishDirection;
		xPos=rndAngle.x;
		makeTime=Time.time;
		child=transform.GetChild (0).gameObject;
		child.transform.localEulerAngles= fishAngle;
		rb=GetComponent<Rigidbody>();
		//childRB=child.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		//enemyCheck();
		rndAngle += new Vector3(Random.value, fishVelocity, 0);
		rndAngle = new Vector3(Mathf.Clamp(rndAngle.x, xPos-3f, xPos+3f), rndAngle.y, 0f);	
		transform.eulerAngles=rndAngle;
		currTime=Time.time;
		// && rend.isVisible != true
		if(currTime-makeTime > 15f) {
			Destroy (gameObject);
			FishMaker.fishNum--;
		}
		if(transform.GetChild (0).gameObject.activeSelf == false)
			transform.GetChild (0).gameObject.SetActive(true);
	}


	/*private void enemyCheck() {
		Collider childCol=child.GetComponent<Collider>();
		if(FishMaker.stage==0 && child.CompareTag ("Small")){
			childCol.isTrigger=false;
		}
		else if(FishMaker.stage==1 && child.CompareTag ("Big")){
			childCol.isTrigger=false;
		}
		else if(FishMaker.stage==2 && child.CompareTag ("Large")){
			childCol.isTrigger=false;
		}
		else if(childCol.isTrigger==false){
			childCol.isTrigger=true;
		}
	}*/
}
