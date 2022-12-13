using System;
using System.Runtime.InteropServices;

namespace OxrExtraFeatures
{
    class ArrayPin : IDisposable
    {
        GCHandle pinnedJointArray;

        public IntPtr Ptr => pinnedJointArray.AddrOfPinnedObject();

        public ArrayPin(Array array)
        {
            pinnedJointArray = GCHandle.Alloc(array, GCHandleType.Pinned);
        }

        public void Dispose()
        {
            pinnedJointArray.Free();
        }
    }
}