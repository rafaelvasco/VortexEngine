using System;
using static VortexCore.SDL;

namespace VortexCore
{
    internal static partial class GamePlatform
    {

        public static event EventHandler OnQuit;

        public static bool IsActive { get; private set; }

        private static Platform? runtimePlatform;

        public static Platform RuntimePlatform
        {
            get
            {
                if (runtimePlatform != null)
                {
                    return runtimePlatform.Value;
                }

                var platformString = SDL_GetPlatform();
                runtimePlatform = GetPlatformFrom(platformString);
                return runtimePlatform.Value;
            }
        }

        private static Platform GetPlatformFrom(string @string)
        {
            return @string switch
            {
                "Windows" => Platform.Windows,
                "Mac OS X" => Platform.MacOS,
                "Linux" => Platform.Linux,
                _ => Platform.Unknown
            };
        }

        public static void Initialize(int displayWidth, int displayHeight, bool fullscreen)
        {
            SDL_SetHint(SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING, "1");

            if(SDL_Init(
                SDL_INIT_VIDEO |
                SDL_INIT_JOYSTICK |
                SDL_INIT_GAMECONTROLLER |
                SDL_INIT_HAPTIC
            ) < 0)
            {
                SDL_Quit();
                throw new ApplicationException("Failed to initialize SDL2");
            }

            SDL_DisableScreenSaver();
            InitializeDisplay(displayWidth, displayHeight, fullscreen);
            InitializeKeyboard();

        }


        public static void Shutdown()
        {
            Console.WriteLine("Vortex Platform Shutdown");
            ReleaseDisplayResources();
            SDL_Quit();
        }

        public static void ProcessEvents()
        {
            while (SDL_PollEvent(out var ev) == 1)
            {
                switch (ev.type)
                {
                    case SDL_EventType.SDL_QUIT:
                        OnQuit?.Invoke(null, EventArgs.Empty);
                        break;

                    case SDL_EventType.SDL_KEYDOWN:
                        AddKey((int)ev.key.keysym.sym);
                        break;

                    case SDL_EventType.SDL_KEYUP:
                        RemoveKey((int)ev.key.keysym.sym);
                        break;

                    case SDL_EventType.SDL_TEXTINPUT:
                        break;

                    case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                        SetMouseButtonState(ev.button.button, true);
                        break;

                    case SDL_EventType.SDL_MOUSEBUTTONUP:
                        SetMouseButtonState(ev.button.button, false);
                        break;

                    case SDL_EventType.SDL_MOUSEMOTION:
                        TriggerMouseMove(ev.motion.x, ev.motion.y);
                        break;

                    case SDL_EventType.SDL_WINDOWEVENT:
                        switch (ev.window.windowEvent)
                        {
                            case SDL_WindowEventID.SDL_WINDOWEVENT_SIZE_CHANGED:
                                var newW = ev.window.data1;
                                var newH = ev.window.data2;
                                DisplayResized?.Invoke(null, new Size(newW, newH));
                                break;

                            case SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE:
                                OnQuit?.Invoke(null, EventArgs.Empty);
                                break;

                            case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST:
                                IsActive = false;
                                break;

                            case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED:
                                IsActive = true;
                                break;


                        }
                        break;
                    case SDL_EventType.SDL_MOUSEWHEEL:
                        TriggerMouseScroll(ev.wheel.y * 120);
                        break;

                    case SDL_EventType.SDL_CONTROLLERDEVICEADDED:
                        break;

                    case SDL_EventType.SDL_CONTROLLERDEVICEREMOVED:
                        break;
                }
            }
        }
    }
}
