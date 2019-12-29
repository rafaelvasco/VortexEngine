using System.Numerics;
namespace VortexCore
{
    
    public struct SokolVertex
    {
        public Vector3 Position;
        public Color Color;
        public Vector2 Uv;

        public SokolVertex(Vector3 pos, Color col, Vector2 uv) {
            this.Position = pos;
            this.Color = col;
            this.Uv = uv;
    
        }
    }
}