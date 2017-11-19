using UnityEngine;
using System.Collections;

public class CatmullromSplineMovement : MonoBehaviour {
	private float angle; 
	float x, y, z;
	float scale;
	float speed;
	bool initFlag;	//initiate x and z of the target position
	int steps; 		//moving steps
	Vector3 toward;	
	Vector3 temp;
	float splineStep=0;	//for catmull rom spline steps
	Vector3[] controlPoints = new Vector3[4];
	Vector3 newDir;
	float[] speed_min = { 0.004f, 0.0045f, 0.005f };
	float[] speed_max = { 0.008f, 0.009f, 0.010f };
	float[] spaceX_min = { -50.0f, -53.0f, -56.0f };
	float[] spaceX_max = { -24.0f, -22.0f, -20.0f };
	float[] spaceY_min = { -3.0f, -2.0f, -1.0f };
	float[] spaceY_max = { 5.0f, 6.0f, 7.0f };
	float[] spaceZ_min = { 40.0f, 37.0f, 34.0f };
	float[] spaceZ_max = { 48.0f, 51.0f, 54.0f };

	int stage_number;

	// Use this for initialization
	void Start () {
		angle = Mathf.PI*(1/3);
		x = 0;	y = 0;	z = 0;
		scale = 5;
		speed = 5.5f;

		toward = new Vector3(Random.Range(spaceX_min[stage_number], spaceX_max[stage_number]), Random.Range(spaceY_min[stage_number], spaceY_max[stage_number]), Random.Range(spaceZ_min[stage_number], spaceZ_max[stage_number]));

		initFlag = false;
		transform.position = new Vector3(Random.Range(spaceX_min[stage_number], spaceX_max[stage_number]), Random.Range(spaceY_min[stage_number], spaceY_max[stage_number]), Random.Range(spaceZ_min[stage_number], spaceZ_max[stage_number]));

		initControlPoints ();
		Random.seed = (int)System.DateTime.Now.Ticks;
	}

	// Update is called once per frame
	void Update () {
		stage_number = FishInfoShow.stage;
		setFishDirection ();
		turnFish ();
	}

	void setFishDirection()
	{

		if (initFlag == false || steps > Random.Range(400, 1000)) 
		{
			x = Random.Range(spaceX_min[stage_number], spaceX_max[stage_number]); 
			y = Random.Range(spaceY_min[stage_number], spaceY_max[stage_number]);
			z = Random.Range(spaceZ_min[stage_number], spaceZ_max[stage_number]);
			steps = 0;
			toward = new Vector3(x, y, z);
			initFlag = true;
		}

	}

	void turnFish()
	{
		//transform.position = Vector3.MoveTowards (transform.position, new Vector3 (x, 1, z), speed * Time.deltaTime);
		toward = CatmullRom (splineStep);

		splineStep+=Random.Range(speed_min[stage_number], speed_max[stage_number]);     //differentiate each fish's speed and angle toward;
		newDir = Vector3.RotateTowards (transform.forward,  toward - transform.position, 500, 0.0F);
		transform.position = toward;
		//Debug.DrawRay(transform.position, newDir, Color.red);
		transform.rotation = Quaternion.LookRotation (newDir);
		transform.Rotate (transform.up, 0, Space.Self);
		steps++;

		if (splineStep >= 1.0f)
		{
			splineStep=0;
			updateControlPoints();
		}
	}

	/* Returns a point on a cubic Catmull-Rom/Blended Parabolas curve
      * u is a scalar value from 0 to 1
      * segment_number indicates which 4 points to use for interpolation
      */
	Vector3 CatmullRom(float u)
	{
		Vector3 point = new Vector3();

		Vector3 a = controlPoints[0];
		Vector3 b = controlPoints[1];
		Vector3 c = controlPoints[2];
		Vector3 d = controlPoints[3];

		point = ((-0.5f*a + 1.5f*b - 1.5f*c + 0.5f*d) * (u * u * u) + (1f*a - 2.5f*b + 2f*c - 0.5f*d) * (u * u) + (-0.5f*a + 0.5f*c) * u + 1f*b);

		return point;
	}

	void initControlPoints()
	{
		controlPoints [0] = new Vector3(Random.Range(spaceX_min[stage_number], spaceX_max[stage_number]), Random.Range(spaceY_min[stage_number], spaceY_max[stage_number]), Random.Range(spaceZ_min[stage_number], spaceZ_max[stage_number]));
		controlPoints [1] = new Vector3(Random.Range(spaceX_min[stage_number], spaceX_max[stage_number]), Random.Range(spaceY_min[stage_number], spaceY_max[stage_number]), Random.Range(spaceZ_min[stage_number], spaceZ_max[stage_number]));
		controlPoints [2] = new Vector3(Random.Range(spaceX_min[stage_number], spaceX_max[stage_number]), Random.Range(spaceY_min[stage_number], spaceY_max[stage_number]), Random.Range(spaceZ_min[stage_number], spaceZ_max[stage_number]));
		controlPoints [3] = new Vector3(Random.Range(spaceX_min[stage_number], spaceX_max[stage_number]), Random.Range(spaceY_min[stage_number], spaceY_max[stage_number]), Random.Range(spaceZ_min[stage_number], spaceZ_max[stage_number]));
	}

	void updateControlPoints()
	{
		controlPoints [0] = controlPoints [1];
		controlPoints [1] = controlPoints [2];
		controlPoints [2] = controlPoints [3];
		controlPoints [3] = new Vector3(Random.Range(spaceX_min[stage_number], spaceX_max[stage_number]), Random.Range(spaceY_min[stage_number], spaceY_max[stage_number]), Random.Range(spaceZ_min[stage_number], spaceZ_max[stage_number]));
		//Debug.Log ("control point = " + controlPoints [3]);
	}
}

