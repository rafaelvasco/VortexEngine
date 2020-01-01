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

using System;
using static VortexCore.SDL;


namespace VortexCore
{
    internal static partial class GamePlatform
    {
        public static IntPtr DisplayHandle { get; private set; }

        public static GraphicsBackend GraphicsBackend { get; private set; }

        public static event EventHandler<Size> DisplayResized;

        public static Graphics Graphics { get; private set; }

        public static void InitializeDisplay(int width, int height, bool fullscreen)
        {
            GraphicsBackend = GetDefaultGraphicsBackendFor(RuntimePlatform);
            CreateDisplay(width, height, fullscreen);
            CreateRenderer(width, height);
        }

        private static void CreateDisplay(int width, int height, bool fullscreen)
        {
            SDL_WindowFlags displayFlags =
             SDL_WindowFlags.SDL_WINDOW_HIDDEN |
             SDL_WindowFlags.SDL_WINDOW_INPUT_FOCUS |
             SDL_WindowFlags.SDL_WINDOW_MOUSE_FOCUS |
             SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI;

            if (fullscreen)
            {
                displayFlags |= SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP;
            }

            switch (GraphicsBackend)
            {
                case GraphicsBackend.OpenGL:
                    SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_FLAGS, (int)SDL_GLcontext.SDL_GL_CONTEXT_FORWARD_COMPATIBLE_FLAG);
                    SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_PROFILE_MASK, SDL_GLprofile.SDL_GL_CONTEXT_PROFILE_CORE);
                    SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, 3);
                    SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION, 3);
                    SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_DOUBLEBUFFER, 1);

                    displayFlags |= SDL_WindowFlags.SDL_WINDOW_OPENGL;

                    break;
            }

            DisplayHandle = SDL_CreateWindow(
                "VortexGame",
                SDL_WINDOWPOS_CENTERED,
                SDL_WINDOWPOS_CENTERED,
                width,
                height,
                displayFlags
            );

            if (DisplayHandle == IntPtr.Zero)
            {
                throw new ApplicationException("Error while creating SDL2 Display: " + SDL_GetError());
            }
        }

        private static void CreateRenderer(int width, int height)
        {
            switch(GraphicsBackend) 
            {
                case GraphicsBackend.OpenGL: 
                    Graphics = new Sdl2Graphics(GamePlatform.DisplayHandle);
                    break;
                case GraphicsBackend.Metal:
                    Graphics = new SokolGraphics(GamePlatform.DisplayHandle);
                    break;
            }
        }

        public static void ShowDisplay(bool show)
        {
            if(show)
            {
                SDL_ShowWindow(DisplayHandle);
            }
            else
            {
                SDL_HideWindow(DisplayHandle);
            }
        }

        public static bool IsFullscreen()
        {
            return GetDisplayFlags().HasFlag(DisplayFlags.Fullscreen);
        }

        public static void SetDisplayFullscreen(bool fullscreen)
        {
            if (IsFullscreen() != fullscreen)
            {
                SDL_SetWindowFullscreen(DisplayHandle, (uint)(fullscreen ? DisplayFlags.FullscreenDesktop : 0));
                
            }
        }

        public static void SetDisplaySize(int width, int height)
        {
            if (IsFullscreen())
            {
                return;
            }

            SDL_SetWindowSize(DisplayHandle, width, height);
            SDL_SetWindowPosition(DisplayHandle, SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED);

        }

        public static Size GetDisplaySize()
        {
            SDL_GetWindowSize(DisplayHandle, out int w, out int h);
            return new Size(w, h);
        }

        public static void SetDisplayBorderless(bool borderless)
        {
            SDL_SetWindowBordered(DisplayHandle, (SDL_bool)(borderless ? 0 : 1));
        }

        public static void SetDisplayTitle(string title)
        {
            SDL_SetWindowTitle(DisplayHandle, title);
        }

        public static string GetDisplayTitle()
        {
            return SDL_GetWindowTitle(DisplayHandle);
        }

        public static void ShowCursor(bool show)
        {
            SDL_ShowCursor(show ? 1 : 0);
        }

        public static DisplayFlags GetDisplayFlags()
        {
            return (DisplayFlags) SDL_GetWindowFlags(DisplayHandle);
        }

        private static void ReleaseDisplayResources()
        {
            if (DisplayHandle != IntPtr.Zero)
            {
                SDL_DestroyWindow(DisplayHandle);
            }

            Graphics.ReleaseResources();
        }

        private static GraphicsBackend GetDefaultGraphicsBackendFor(Platform platform)
        {
            return platform switch
            {
                Platform.Windows => GraphicsBackend.OpenGL, //TODO: Direct3D
                Platform.OSX => GraphicsBackend.Metal,
                Platform.Linux => GraphicsBackend.OpenGL,
                Platform.FreeBSD => GraphicsBackend.OpenGL,
                Platform.Unknown => GraphicsBackend.OpenGL,
                _ => GraphicsBackend.OpenGL
            };
        }

        [Flags]
        public enum DisplayFlags
        {
            Fullscreen = 0x00000001,
            Shown = 0x00000004,
            Hidden = 0x00000008,
            Borderless = 0x00000010,
            Resizable = 0x00000020,
            Minimized = 0x00000040,
            Maximized = 0x00000080,
            InputFocus = 0x00000200,
            MouseFocus = 0x00000400,
            FullscreenDesktop = 0x00001001,
            AllowHighDPI = 0x00002000,
            MouseCapture = 0x00004000
        }

    }
}
