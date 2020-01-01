/* 
MIT License
Copyright (c) 2019 Rafael Vasco
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

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

        public abstract int MaxDrawCalls { get; }

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
