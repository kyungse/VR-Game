using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum AEROWAND_BUTTONS {
	ButtonLeft = 1 << 0,
	ButtonRight = 1 << 1
}

public class AerowandDeviceWrapper : MonoBehaviour {
	public enum ConnectionType {
		AEROWAND_USBHOST
		, PLACEHOLDER_AEROWAND_USBACCESSORY // XXX not implemented
		, AEROWAND_BLUETOOTHCLASSIC
		, PLACEHOLDER_AEROWAND_BLUETOOTHLE // XXX not implemented
	}
	public ConnectionType getConnectionType {
		get { return connectionType; }
	}
	[SerializeField]ConnectionType connectionType;
	public float positionScale = 6.0f;

	private AerowandInterface AerowandDevice = null;
	public static AerowandDeviceWrapper AerowandHead { get; private set; }
	public static AerowandDeviceWrapper AerowandHand { get; private set; }
	public Vector3 position { get; private set; }
	public Quaternion orientation { get; private set; }
    public Vector3 positionRaw { get; private set; }
    public Quaternion orientationRaw { get; private set; }
	public byte buttonFlags { get; private set; }
	public float packetHz { get; private set; }

	static bool init = false;
	static bool closed = false;

	private Dictionary<AEROWAND_BUTTONS, ButtonState> buttonDictionary = new Dictionary<AEROWAND_BUTTONS, ButtonState>();
	private class ButtonState {
		public bool down = false;
		public bool lastFrameDown = false;
	}

	public bool GetButtonDown ( AEROWAND_BUTTONS button ) {
		ButtonState buttonState;
		buttonDictionary.TryGetValue (button, out buttonState);
		return (buttonState.down && !buttonState.lastFrameDown);
	}
	public bool GetButtonUp ( AEROWAND_BUTTONS button ) {
		ButtonState buttonState;
		buttonDictionary.TryGetValue (button, out buttonState);
		return (!buttonState.down && buttonState.lastFrameDown);
	}
	public bool GetButton ( AEROWAND_BUTTONS button ) {
		ButtonState buttonState;
		buttonDictionary.TryGetValue (button, out buttonState);
		return buttonState.down;
	}

    private AerowandWindowsBluetooth debug;

	void OnEnable () {
        position = Vector3.zero;
        orientation = Quaternion.identity;
        positionRaw = Vector3.zero;
        orientationRaw = Quaternion.identity;
		packetHz = 0.0f;

		foreach (AEROWAND_BUTTONS button in Enum.GetValues ( typeof ( AEROWAND_BUTTONS ) )) {
			buttonDictionary.Add (button, new ButtonState ());
		}

		if ( AerowandDevice == null ) {
			switch ( connectionType ) {
			case ConnectionType.AEROWAND_USBHOST:
				#if ( UNITY_ANDROID && !UNITY_EDITOR )
				AerowandDevice = new AerowandAndroidWrapper ( AerowandAndroidWrapper.StreamSource.Usb );
				#elif ( UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN )
				if (init == false)
				{
					AerowandWindowsBluetooth.InitializeDevices();
					init = true;
				}
				debug = new AerowandWindowsBluetooth(connectionType);
				AerowandDevice = debug;
				#endif
				break;
			case ConnectionType.AEROWAND_BLUETOOTHCLASSIC:
				#if ( UNITY_ANDROID && !UNITY_EDITOR )
				AerowandDevice = new AerowandAndroidWrapper ( AerowandAndroidWrapper.StreamSource.Bluetooth );
				#elif ( UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN )
				if (init == false)
				{
					AerowandWindowsBluetooth.InitializeDevices();
					init = true;
				}
				debug = new AerowandWindowsBluetooth ( connectionType );
				AerowandDevice = debug;
				#endif
				break;
			default:
				break;
			}
		}

		switch ( connectionType ) {
		case ConnectionType.AEROWAND_USBHOST:
			if ( AerowandHead == null ) {
				AerowandHead = this;
			}
			break;
		case ConnectionType.AEROWAND_BLUETOOTHCLASSIC:
			if ( AerowandHand == null ) {
				AerowandHand = this;
			}
			break;
		default:
			break;
		}
	}

	void UpdateButtons () {
		foreach (AEROWAND_BUTTONS button in Enum.GetValues ( typeof ( AEROWAND_BUTTONS ) )) {
			ButtonState buttonState;
			buttonDictionary.TryGetValue (button, out buttonState);

			buttonState.lastFrameDown = buttonState.down;
			buttonState.down = (buttonFlags & (byte)button) != 0;
			if ( GetButtonDown(button) ) {
				Debug.Log ( button.ToString() + " pressed");
			} else if ( GetButtonUp(button) ) {
				Debug.Log ( button.ToString() + " released");
			}
		}
	}

	void GetData () {
		AerowandData aerowandData = AerowandDevice.getAerowandData ();
		aerowandData.positionXYZ[2] *= -1.0f;
		position = new Vector3 ( aerowandData.positionXYZ[0], aerowandData.positionXYZ[1], aerowandData.positionXYZ[2] );
        positionRaw = position;
        position *= positionScale;

		aerowandData.orientationRPY[0] *= -1.0f;
		Quaternion yaw = Quaternion.Euler ( 0.0f, aerowandData.orientationRPY[0], 0.0f );
		Quaternion pitch = Quaternion.Euler (aerowandData.orientationRPY[1], 0.0f, 0.0f);
		Quaternion roll = Quaternion.Euler ( 0.0f, 0.0f, -aerowandData.orientationRPY[2]);

		orientation = yaw * pitch * roll;

		buttonFlags = aerowandData.buttonFlags;
		packetHz = AerowandDevice.getPacketHz();
	}

    void LateUpdate()
    {
        if (AerowandDevice == null) return;
        GetData();
        UpdateButtons();
#if UNITY_EDITOR_WIN
		StartCoroutine(PrintDelay(Time.realtimeSinceStartup));
#endif
    }

	public void ForceUpdate() {
		if (AerowandDevice == null) return;
		GetData();
		UpdateButtons();
	}

    void OnDestroy ()
	{
		if (AerowandDevice != null && closed == false)
		{
			AerowandWindowsBluetooth.CloseDevices();
			closed = true;
		}
    }

    IEnumerator PrintDelay(float beginTime)
    {
		yield return new WaitForEndOfFrame();
        //Debug.Log("Draw Latency " + ((Time.realtimeSinceStartup - beginTime) / 1000.0f));
    }
}
