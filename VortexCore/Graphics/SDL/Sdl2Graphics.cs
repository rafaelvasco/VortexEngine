using System;
using static VortexCore.SDL;

namespace VortexCore
{
    internal unsafe class Sdl2Graphics : Graphics
    {
        private readonly IntPtr ctx;

        internal Sdl2Graphics(int width, int height) 
        {
            ctx = SDL_CreateRenderer(
               GamePlatform.DisplayHandle,
               -1,
               SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
               SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC |
               SDL_RendererFlags.SDL_RENDERER_TARGETTEXTURE
            );

            SDL_SetRenderDrawBlendMode(ctx, SDL_BlendMode.SDL_BLENDMODE_NONE);

        }

        ~Sdl2Graphics()
        {
            SDL_DestroyRenderer(ctx);
        }

        Texture2D Graphics.CreateTexture(IntPtr data, int width, int height)
        {
            var surface = SDL_CreateRGBSurfaceWithFormatFrom(data, width, height, 0, width * 4, SDL_PIXELFORMAT_ARGB8888);

            var textureHandle = SDL_CreateTextureFromSurface(ctx, surface);

            var texture = new SdlTexture(textureHandle, width, height);

            return texture;
        }


        void Graphics.Begin()
        {
            SDL_SetRenderDrawColor(ctx, 255, 173, 197, 255);
            
            SDL_RenderClear(ctx);

        }

        void Graphics.End()
        {
            SDL_RenderPresent(ctx);
        }
        void Graphics.ReleaseResources()
        {
            GC.SuppressFinalize(this);
            SDL_DestroyRenderer(ctx);
        }

        void Graphics.DrawTexture(Texture2D texture, float x, float y)
        {
            var targetRect = new SDL_FRect() 
            {
                x = x, 
                y = y,
                w = texture.Width, 
                h = texture.Height
            };
            SDL_RenderCopyF(ctx, texture.Handle, IntPtr.Zero, ref targetRect);
        }

        void Graphics.DrawTextureRegion(Texture2D texture, float x, float y, ref Rect sourceRect)
        {
            var srcRect = new SDL_Rect() 
            {
                x = sourceRect.X,
                y = sourceRect.Y,
                w = sourceRect.Width,
                h = sourceRect.Height
            };

            var targetRect = new SDL_FRect() 
            {
                x = x,
                y = y,
                w = sourceRect.Width,
                h = sourceRect.Height
            };

            SDL_RenderCopyF(ctx, texture.Handle, ref srcRect, ref targetRect);            
        }

        void Graphics.FillRect(float x, float y, float w, float h, Color color)
        {
            var targetRect = new SDL_FRect()
            {
                x = x,
                y = y,
                w = w,
                h = h
            };

            SDL_SetRenderDrawColor(ctx, color.Ri, color.Gi, color.Bi, color.Ai);
            SDL_RenderFillRectF(ctx, ref targetRect);
        }

       
    }
}
