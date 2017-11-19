using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class AerowandWindowsBluetooth : AerowandInterface
{
	[DllImport("aerowand", EntryPoint = "InitializeDevices")]
	public static extern bool InitializeDevices();

	[DllImport("aerowand", EntryPoint = "CloseDevices")]
	public static extern void CloseDevices();

	[DllImport("aerowand", EntryPoint="GetAerowandHead")]
	private static extern int GetAerowandHead();

	[DllImport("aerowand", EntryPoint="GetAerowandHandDominant")]
	private static extern int GetAerowandHandDominant();

	[DllImport("aerowand", EntryPoint = "GetAerowandHandNondominant")]
	private static extern int GetAerowandHandNondominant();

	[DllImport("aerowand", EntryPoint="GetAerowandData")]
	private static extern AerowandData GetAerowandData(int deviceId);

    int aerowandDevice;

	public AerowandWindowsBluetooth (AerowandDeviceWrapper.ConnectionType connectionType) {
		switch ( connectionType ) {
		case AerowandDeviceWrapper.ConnectionType.AEROWAND_USBHOST:
			aerowandDevice = GetAerowandHead ();
			break;
		case AerowandDeviceWrapper.ConnectionType.AEROWAND_BLUETOOTHCLASSIC:
			aerowandDevice = GetAerowandHandDominant ();
			break;
		default:
			break;
		}
	}

    public double latency = 0.0f;
    public float readtime = 0.0f;

	public AerowandData getAerowandData () {
#if true
        return GetAerowandData (aerowandDevice);
#else
        AerowandDataDebug aerowandDataDebug = GetAerowandDataDebug(aerowandDevice);
        latency = aerowandDataDebug.pluginLatency;
        readtime = Time.realtimeSinceStartup;
        //Debug.Log("plugin latency " + aerowandDataDebug.pluginLatency);
        return aerowandDataDebug.aerowandData;
#endif
    }

	public float getPacketHz() {
		return 0;
	}
}
