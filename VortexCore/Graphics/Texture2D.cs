using System;

namespace VortexCore
{
    public abstract class Texture2D : Asset
    {
        public IntPtr Handle { get; protected set; }

        public int Width { get; protected set; }

        public int Height { get; protected set; }

    }
}
