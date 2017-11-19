using UnityEngine;

public class AerowandAndroidWrapper : AerowandInterface {
    public enum StreamSource { Usb, Bluetooth }

	AndroidJavaObject aerowand;
	static AndroidJavaObject head;
	static AndroidJavaObject hand;

	public AerowandAndroidWrapper(StreamSource streamSource)
	{
		if (head == null) {
			AndroidJavaObject activityContext;
			using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
			}
			activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() =>
			{
				using (AndroidJavaClass aerowandDeviceFactory = new AndroidJavaClass("com.accups.aerowandandroid.AerowandDeviceFactory"))
				{
					head = aerowandDeviceFactory.CallStatic<AndroidJavaObject>("getSingletonHead", activityContext);
					hand = aerowandDeviceFactory.CallStatic<AndroidJavaObject>("getSingletonHand", activityContext);
				}
			}));
		}
		switch (streamSource)
		{
			case StreamSource.Usb:
				while (head == null) { } // XXX wait for ui thread to return
				aerowand = head;
				break;
			case StreamSource.Bluetooth:
				while (hand == null) { } // XXX wait for ui thread to return
				aerowand = hand;
				break;
			default:
				break;
		}
	}

	public AerowandData getAerowandData () {
		AerowandData aerowandData;
		aerowandData.positionXYZ = aerowand.Call<float[]> ("getPositionXYZ");
		aerowandData.orientationRPY = aerowand.Call<float[]> ("getOrientationRPY");
		aerowandData.buttonFlags = aerowand.Call<byte> ( "getButtonFlags" );
		return aerowandData;
	}

	public float getPacketHz() {
		return aerowand.Call<float> ( "getPacketHz" );
	}
}
