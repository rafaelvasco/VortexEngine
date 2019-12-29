using System;

namespace VortexCore
{
    public abstract class Texture2D : Asset
    {
        internal IntPtr Handle { get; set; }

        internal uint IndexHandle { get; set;}

        public int Width { get; protected set; }

        public int Height { get; protected set; }

    }
}
