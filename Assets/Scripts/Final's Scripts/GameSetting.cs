using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameSetting : MonoBehaviour {

	public static int[] cathced_check = new int[15];

	void Start ()
	{
		Invoke ("CloseStartMessage", 2.0f);
		for (int i = 0; i < 15; i++) {
			cathced_check [i] = 0;
		}
	}

	void Update () 
	{

	}

	private void CloseStartMessage() {
		GameObject.Find ("StartText").SetActive (false);
	}

}
