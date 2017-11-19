using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent ( typeof ( Text ) ) ]
public class ReadAerowandToGuiText : MonoBehaviour {
	[SerializeField]AerowandDeviceWrapper aerowand;
    [SerializeField]String deviceName = "";
	Text textObject;

	void Awake () {
		textObject = GetComponent<Text> () as Text;
	}

	void Update () {
        Vector3 positionInCm = aerowand.positionRaw * 100.0f;
        positionInCm.z = positionInCm.z * -1.0f;
        Vector3 orientationPYR = aerowand.orientation.eulerAngles;
        float roll = orientationPYR.x;
        orientationPYR.x = orientationPYR.y;
        orientationPYR.y = roll;

        textObject.text = 
			"Aerowand " + deviceName + Environment.NewLine
			+ "Position (cm)" + Environment.NewLine
			+ positionInCm + Environment.NewLine
			+ "Orientation (deg)" + Environment.NewLine
			+ orientationPYR + Environment.NewLine
            ;
	}
}
