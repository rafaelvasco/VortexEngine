using System;

namespace VortexCore
{
    public struct GraphicsInfo
    {
        public string Name;
        public string Driver;
        public int MaxTextureWidth;
        public int MaxTextureHeight;
    }

    public interface Graphics
    {
        public abstract bool VSyncEnabled { get; set; }

        public abstract GraphicsInfo Info { get; }

        public void Begin();

        public void End();

        public void DrawQuad(Texture2D texture, ref Quad quad);

        public void FillRect(float x, float y, float w, float h, Color color);

        public abstract (int width, int height) GetRenderSize();

        internal void ReleaseResources();

        internal abstract void Present();

        internal Texture2D CreateTexture(Pixmap pixmap);

    }
}
