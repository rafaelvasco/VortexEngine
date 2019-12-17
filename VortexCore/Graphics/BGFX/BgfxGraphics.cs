using System;

namespace VortexCore
{
    public sealed unsafe class BgfxGraphics : Graphics
    {

        internal BgfxGraphics(int width, int height)
        {
            var bgfxPlatformData = new Bgfx.PlatformData()
            {
                ndt = null,
                nwh = (void*)GamePlatform.NativeDisplayHandle
            };

            Bgfx.SetPlatformData(&bgfxPlatformData);

            var bgfxInitData = new Bgfx.InitProps()
            {
                type = Bgfx.RendererType.OpenGL,

                resolution = new Bgfx.Resolution()
                {
                    width = (uint)width,
                    height = (uint)height
                },
            };

            Bgfx.Init(&bgfxInitData);
            Bgfx.Reset(800, 600, (uint)Bgfx.ResetFlags.Vsync, Bgfx.TextureFormat.RGBA8);
            Bgfx.SetDebug((uint)Bgfx.DebugFlags.Text);
            Bgfx.SetViewClear(0, (ushort)Bgfx.ClearFlags.Color, Color.Red, 0f, 0);
        }

        void Graphics.Begin()
        {
            Bgfx.SetViewRect(0, 0, 0, 800, 600);
            Bgfx.Touch(0);
        }

        Texture2D Graphics.CreateTexture(IntPtr data, int width, int height)
        {
            throw new NotImplementedException();
        }

        void Graphics.DrawTexture(Texture2D texture, float x, float y)
        {
            throw new NotImplementedException();
        }

        void Graphics.DrawTextureRegion(Texture2D texture, float x, float y, ref Rect sourceRect)
        {
            throw new NotImplementedException();
        }

        void Graphics.End()
        {
            Bgfx.Frame(false);
        }

        void Graphics.FillRect(float x, float y, float w, float h, Color color)
        {
            throw new NotImplementedException();
        }

        void Graphics.ReleaseResources()
        {
            GC.SuppressFinalize(this);
            Bgfx.Shutdown();
        }


    }
}
