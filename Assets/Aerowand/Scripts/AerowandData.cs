using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct AerowandData {
	[MarshalAs(UnmanagedType.ByValArray, SizeConst=3)]
	public float[] positionXYZ;
	[MarshalAs(UnmanagedType.ByValArray, SizeConst=3)]
	public float[] orientationRPY;
	public byte buttonFlags;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct AerowandDataDebug
{
    public AerowandData aerowandData;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
    private byte[] padding;
    public long packetReceived;
    public long dataQueried;
    public double pluginLatency;
}
