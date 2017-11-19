using UnityEngine;
using System.Collections;
using UnityEngine.VR;

[RequireComponent ( typeof ( AerowandDeviceWrapper ) )]
public class ReferencedTransform : MonoBehaviour {
	AerowandDeviceWrapper device;
	static Vector3 aerowandReferencePosition = Vector3.zero;
	static Quaternion aerowandReferenceOrientation = Quaternion.identity;
	static Vector3 sceneReferencePosition = Vector3.zero;
	static Quaternion sceneReferenceOrientation = Quaternion.identity;
	static bool hmdSet = false;
	Transform targetTransform;
	Transform aerowandTransform;

	Vector3 defaultPosition;
	Quaternion defaultOrientation;

	void Awake()
	{
		defaultPosition = transform.position;
		defaultOrientation = transform.rotation;
	}

	IEnumerator Start ()
	{
		targetTransform = new GameObject().transform;
		aerowandTransform = new GameObject().transform;
		device = GetComponent<AerowandDeviceWrapper>() as AerowandDeviceWrapper;
		if (device.getConnectionType == AerowandDeviceWrapper.ConnectionType.AEROWAND_USBHOST)
		{
			// Wait until Aerowand has returned non-zero transform data to set reference centers
			while ( device.position == Vector3.zero ) yield return null;
			hmdSet = true;
			sceneReferencePosition = defaultPosition;
			sceneReferenceOrientation = defaultOrientation;
			aerowandReferencePosition = device.position;
			aerowandReferenceOrientation = device.orientation;

			// reset the scene which has been messed up by having changed the gameobject's position and orientation changed by the VR headset
			UpdateTransform();
			transform.position = defaultPosition;
			transform.rotation = defaultOrientation;
		} else {
			// Wait until Aerowand has returned non-zero transform data indicating it is receiving data
			while ( device.position == Vector3.zero ) yield return null;
			// If no Aerowand HMD or other BT device has yet claimed to be the reference frame for the scene, use this device
			// This will be overruled by the HMD once it claims the scene.
			if ( sceneReferencePosition == Vector3.zero ) {
                sceneReferencePosition = defaultPosition;
                sceneReferenceOrientation = defaultOrientation;
				aerowandReferencePosition = device.position;
				aerowandReferenceOrientation = device.orientation;
			}
		}
		Camera.onPreCull += LastPossibleUpdateTransform;

		while ( true )
		{
			UpdateTransform();
			yield return new WaitForFixedUpdate();
		}
	}

	void LastPossibleUpdateTransform (Camera cam)
	{
		UpdateTransform();
	}

	void UpdateTransform ()
	{

		if (
			device.GetButtonUp(AEROWAND_BUTTONS.ButtonRight)
			&& (
				device.getConnectionType == AerowandDeviceWrapper.ConnectionType.AEROWAND_USBHOST
				|| hmdSet == false
			)
		)
		{
			Debug.Log("Resetting offset");
			aerowandReferencePosition = device.position;
			aerowandReferenceOrientation = device.orientation;
		}

		// find offset in Aerowand's IRL space
		aerowandTransform.position = aerowandReferencePosition;
		aerowandTransform.rotation = aerowandReferenceOrientation;
		Vector3 aerowandOffsetIrlSpace = aerowandTransform.InverseTransformPoint(device.position);

		// apply Aerowand IRL space offset in Unity space
		targetTransform.position = sceneReferencePosition;
		targetTransform.rotation = sceneReferenceOrientation;
		targetTransform.Translate(aerowandOffsetIrlSpace);

		// find how the Aerowand has reoriented since it's reference was taken
		Quaternion aerowandOrientationOffset = Quaternion.Inverse(aerowandReferenceOrientation) * device.orientation;
		targetTransform.rotation = sceneReferenceOrientation * aerowandOrientationOffset;

		if (AerowandDeviceWrapper.ConnectionType.AEROWAND_USBHOST.Equals(device.getConnectionType)) {
			Camera cam = GetComponent<Camera>();

			Matrix4x4 worldToCameraMatrix = targetTransform.worldToLocalMatrix;
			Vector4 zInvert = worldToCameraMatrix.GetRow(2) * -1.0f;
			worldToCameraMatrix.SetRow(2, zInvert);
			cam.worldToCameraMatrix = worldToCameraMatrix;

			Matrix4x4 viewL = worldToCameraMatrix;
			Matrix4x4 viewR = worldToCameraMatrix;

			//viewL[0,3] += cam.stereoSeparation / 2.0f;
			//viewR[0,3] -= cam.stereoSeparation / 2.0f;
			cam.SetStereoViewMatrices(viewL, viewR);
		} else {
			float lerpFactorRotation = 1.0f;// 10.0f * Time.fixedDeltaTime;
			transform.rotation = Quaternion.Lerp(transform.rotation, targetTransform.rotation, lerpFactorRotation);
			float lerpFactorPosition = 1.0f;// * Time.fixedDeltaTime;
			transform.position = Vector3.Lerp(transform.position, targetTransform.position, lerpFactorPosition);
		}
	}

	void OnDestroy ()
	{
		Camera.onPreCull -= LastPossibleUpdateTransform;
	}
}
