using System;
using static VortexCore.SDL;

namespace VortexCore
{
    internal unsafe class Sdl2Graphics : Graphics
    {
        private readonly IntPtr ctx;
        private GraphicsInfo info;

        bool Graphics.VSyncEnabled
        {
            get => SDL_GL_GetSwapInterval() != 0;
            set 
            {
                // Try for adaptive VSYNC first, if it fails go for normal VSYNC
                var result = SDL_GL_SetSwapInterval(-1);
                
                if(result != 0) 
                {
                    SDL_GL_SetSwapInterval(1);
                }
            }
        }

        GraphicsInfo Graphics.Info => info;

        internal Sdl2Graphics(IntPtr windowHandle, int width, int height)
        {
            SDL_SetHint("SDL_HINT_RENDER_DRIVER", "opengl");
            ctx = SDL_CreateRenderer(
               windowHandle,
               -1,
               SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
               SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC |
               SDL_RendererFlags.SDL_RENDERER_TARGETTEXTURE
            );

            this.info = new GraphicsInfo();

            if (SDL_GetRendererInfo(ctx, out var rendererInfo) == 0 &&
                SDL_GetRenderDriverInfo(0, out var renderDriverInfo) == 0)
            {
                this.info.Name = "Sdl2Graphics";
                this.info.MaxTextureWidth = rendererInfo.max_texture_width;
                this.info.MaxTextureHeight = rendererInfo.max_texture_height;
                this.info.Driver = UTF8_ToManaged(renderDriverInfo.name);
            }

            SDL_SetRenderDrawBlendMode(ctx, SDL_BlendMode.SDL_BLENDMODE_NONE);

        }

        ~Sdl2Graphics()
        {
            SDL_DestroyRenderer(ctx);
        }

        Texture2D Graphics.CreateTexture(Pixmap pixmap)
        {
            var surface = SDL_CreateRGBSurfaceWithFormatFrom(pixmap.DataPtr, pixmap.Width, pixmap.Height, 0, pixmap.Width * pixmap.Pitch, SDL_PIXELFORMAT_ARGB8888);

            var textureHandle = SDL_CreateTextureFromSurface(ctx, surface);

            var texture = new SdlTexture(textureHandle, pixmap.Width, pixmap.Height);

            return texture;
        }


        void Graphics.Begin()
        {
            SDL_SetRenderDrawColor(ctx, 255, 173, 197, 255);

            SDL_RenderClear(ctx);

        }

        void Graphics.End()
        {

        }
        void Graphics.ReleaseResources()
        {
            GC.SuppressFinalize(this);
            SDL_DestroyRenderer(ctx);
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

        void Graphics.DrawQuad(Texture2D texture, ref Quad quad)
        {
            var flip = SDL_RendererFlip.SDL_FLIP_NONE;

            if (quad.FlipH)
            {
                flip |= SDL_RendererFlip.SDL_FLIP_HORIZONTAL;
            }

            if (quad.FlipV)
            {
                flip |= SDL_RendererFlip.SDL_FLIP_VERTICAL;
            }

            var srcRect = new SDL_Rect()
            {
                x = (int)quad.SrcX,
                y = (int)quad.SrcY,
                w = (int)quad.SrcWidth,
                h = (int)quad.SrcHeight,
            };

            var targetRect = new SDL_FRect()
            {
                x = quad.X,
                y = quad.Y,
                w = quad.Width,
                h = quad.Height
            };

            SDL_RenderCopyExF
           (
               ctx,
               texture.Handle,
               ref srcRect,
               ref targetRect,
               quad.Rotation,
               IntPtr.Zero,
               flip
           );

        }

        (int width, int height) Graphics.GetRenderSize()
        {
            SDL_GetRendererOutputSize(ctx, out int width, out int height);
            return (width, height);
        }

        void Graphics.Present()
        {
            SDL_RenderPresent(ctx);
        }
    }
}
