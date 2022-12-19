using System.Runtime.InteropServices;

namespace Vrm10XR
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FrameHeader
    {
        public long StartTime;
        public long EndTime;
        public int FrameCount;
    }

    // [StructLayout(LayoutKind.Sequential)]
    // public struct FrameItem<T> where T : struct
    // {
    //     public long Time;
    //     public T Value;
    // }
}
