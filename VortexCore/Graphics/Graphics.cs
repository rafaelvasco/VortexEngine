using System;

namespace VortexCore
{
    public interface Graphics
    {
        public void Begin();

        public void End();

        public void DrawTexture(Texture2D texture, float x, float y);

        public void DrawTextureRegion(Texture2D texture, float x, float y, ref Rect sourceRect);

        public void FillRect(float x, float y, float w, float h, Color color);

        internal void ReleaseResources();

        internal Texture2D CreateTexture(IntPtr data, int width, int height);

    }
}
