using System.Runtime.InteropServices;

namespace VortexCore
{

    [StructLayout(LayoutKind.Sequential)]
    public struct VertexPCT
    {
        public float X;
        public float Y;
        public float Tx;
        public float Ty;
        public uint Col;

        public VertexPCT(float x, float y, float tx, float ty, uint color)
        {
            this.X = x;
            this.Y = y;
            this.Tx = tx;
            this.Ty = ty;
            this.Col = color;
        }

        public static int Stride => 20;

    }
}
