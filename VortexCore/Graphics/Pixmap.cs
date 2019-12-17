using System;
using System.Runtime.InteropServices;

namespace VortexCore
{
    public class Pixmap : IDisposable
    {
        public byte[] Pixels => pixelData.data;

        internal IntPtr DataPtr => pixelData.dataPtr;

        public int Width { get; }

        public int Height { get; }

        public int Pitch => Width * 4;

        private readonly PinnedByteArray pixelData;

        public Pixmap(int width, int height)
        {
            Width = width;
            Height = height;
            pixelData = new PinnedByteArray(width * height * 4);
        }

        public Pixmap(byte[] data, int width, int height, bool convertToBgra = true)
        {
            this.pixelData = new PinnedByteArray(data);

            Width = width;
            Height = height;

            if (convertToBgra)
            {
                var pixelData = this.pixelData.data;

                for(int i = 0; i < Width * Height; ++i)
                {
                    var idx = i * 4;
                    
                    byte r = pixelData[idx];
                    byte g = pixelData[idx + 1];
                    byte b = pixelData[idx + 2];
                    byte a = pixelData[idx + 3];
                    
                    pixelData[idx] = b;
                    pixelData[idx + 1] = g;
                    pixelData[idx + 2] = r;
                    pixelData[idx + 3] = a;
                }
            }
        }

        public void Dispose()
        {
            pixelData.Dispose();
        }
    }

    internal class PinnedByteArray : IDisposable
    {
        public byte[] data;
        public GCHandle gcHandle;
        public IntPtr dataPtr;

        private bool disposed;

        public PinnedByteArray(int length)
        {
            data = new byte[length];
            gcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            dataPtr = Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);
        }

        public PinnedByteArray(byte[] sourceData)
        {
            data = new byte[sourceData.Length];

            Buffer.BlockCopy(sourceData, 0, data, 0, sourceData.Length);

            gcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            dataPtr = Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);
        }

        ~PinnedByteArray()
        {
        }

        public void Dispose()
        {
            if (!disposed)
            {
                gcHandle.Free();
                data = null;
                dataPtr = IntPtr.Zero;
                disposed = true;
            }
        }
    }
}
