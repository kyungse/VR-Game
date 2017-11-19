using UnityEngine;
using System.Collections;

public class Camera2 : MonoBehaviour {
	//attached to main camera.
	//rotate by keyboard.

	public float xSpeed = 50f;
	public float ySpeed = 50f;
	private Vector3 rot;

	// Use this for initialization
	void Start () {
		rot = new Vector3(0,180f,0);
	}
	
	// Update is called once per frame
	void Update () {
		float sideRotate = xSpeed * Input.GetAxis ("Horizontal")*Time.deltaTime;
		float upsideRotate = (-1)* ySpeed * Input.GetAxis ("Vertical")*Time.deltaTime;
		rot += new Vector3(upsideRotate, sideRotate, 0);
		rot = new Vector3(Mathf.Clamp(rot.x, -60f, 30f), rot.y, rot.z);
		transform.eulerAngles = rot;
	}
}
