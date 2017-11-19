using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent ( typeof ( Text ) ) ]
public class ReadAerowandOffset : MonoBehaviour {
	AerowandDeviceWrapper aerowandHead;
	AerowandDeviceWrapper aerowandHand;
	Text textObject;
	
	void Awake () {
		textObject = GetComponent<Text> () as Text;
		aerowandHead = GameObject.FindGameObjectWithTag ( "Aerowand Head" ).GetComponent<AerowandDeviceWrapper> () as AerowandDeviceWrapper;
		aerowandHand = GameObject.FindGameObjectWithTag ( "Aerowand Hand" ).GetComponent<AerowandDeviceWrapper> () as AerowandDeviceWrapper;
	}
	
	void Update () {
		Vector3 headPositionHeadSpace = aerowandHead.gameObject.transform.InverseTransformPoint ( aerowandHead.position / aerowandHead.positionScale );
		Vector3 handPositionHeadSpace = aerowandHead.gameObject.transform.InverseTransformPoint ( aerowandHand.position / aerowandHand.positionScale );
		Vector3 offsetPosition = headPositionHeadSpace - handPositionHeadSpace;
		Quaternion offsetRotation = Quaternion.Inverse ( aerowandHead.orientation ) * aerowandHand.orientation;
		textObject.text = 
			"--- Device Offset Head Space ---" + Environment.NewLine
			+ offsetPosition + Environment.NewLine
			+ "--- Orientation Offset ---" + Environment.NewLine
			+ offsetRotation.eulerAngles;
		;
	}
}
