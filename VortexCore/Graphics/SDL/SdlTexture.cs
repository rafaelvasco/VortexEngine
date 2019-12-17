using System;
using static VortexCore.SDL;

namespace VortexCore
{
    public class SdlTexture : Texture2D
    {
        internal SdlTexture(IntPtr handle, int width, int height)
        {
            Handle = handle;

            Width = width;

            Height = height;
        }

        ~SdlTexture()
        {
            SDL_DestroyTexture(Handle);
        }

        internal override void Dispose()
        {
            SDL_DestroyTexture(Handle);
        }
    }
}
