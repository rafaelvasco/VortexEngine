using System;

namespace VortexCore
{
    public interface Graphics
    {
        public void Begin();

        public void End();

        public void DrawQuad(Texture2D texture, ref Quad quad);

        public void FillRect(float x, float y, float w, float h, Color color);

        internal void ReleaseResources();

        internal Texture2D CreateTexture(IntPtr data, int width, int height);

    }
}
