using UnityEngine;
using System.Collections;

public class MovingText : MonoBehaviour {

	public float ScoreDelay = 0.5f;
	// Use this for initialization
	void Start () {
		StartCoroutine("DisplayScore"); 
	}

	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		pos.y += 0.001f;
		transform.position = pos;
	}
	IEnumerator DisplayScore()
	{
		yield return new WaitForSeconds(ScoreDelay);

		for(float a = 1; a >= 0; a -= 0.005f)
		{
			gameObject.GetComponent<TextMesh>().color = new Vector4(1, 1, 1, a);
			yield return new WaitForFixedUpdate();
		}

		gameObject.SetActive(false);
	}
}
