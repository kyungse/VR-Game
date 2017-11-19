using UnityEngine;
using System.Collections;


public class FishMaker : MonoBehaviour {
	//attached to fishmaker.
	//set fish number to 20.
	public GameObject[] fishObject=new GameObject[5];
	public static int fishNum=0;
	public int fishMax=20;
	public static int stage=0;
	public static int[] destroyedFish = new int[5];
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(destroyedFish[stage]>=10){
			stage+=1;
			destroyedFish[stage]=0;
			GM.instance.levelUp(stage);
			/*switch(stage) {
			case 1:
				GM.instance.tinyText.enabled=false;
				break;
			case 2:
				GM.instance.smallText.enabled=false;
				break;
			}*/
		}
		int i=Mathf.Clamp (Random.Range(0, stage+2), 0, 4);
		if (fishNum < fishMax) {
			Instantiate (fishObject[i], transform.position, Quaternion.identity);
			fishNum++;
		}
		//print (destroyedFish[0]+", "+destroyedFish[1]+", "+destroyedFish[2]+", "+destroyedFish[3]+", "+destroyedFish[4]);
	}
}
