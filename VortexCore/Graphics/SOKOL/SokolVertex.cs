using System.Numerics;
namespace VortexCore
{
    
    public struct SokolVertex
    {
        public Vector3 Position;
        public Vector2 Uv;
        public Vector4 Color;
        
        public static readonly int SizeInBytes = 36;

        public SokolVertex(Vector3 pos, Vector2 uv, Color col) {
            this.Position = pos;
            this.Uv = uv;
            this.Color = col;
        }
    }
}