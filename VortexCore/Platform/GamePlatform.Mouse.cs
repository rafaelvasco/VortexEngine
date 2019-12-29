
using System;
using static VortexCore.SDL;

namespace VortexCore
{
    internal static partial class GamePlatform
    {
        public static event EventHandler<int> OnMouseScroll;
        public static event EventHandler<Point> OnMouseMove;

        private static MouseState mouseState;

        public static ref readonly MouseState GetMouseState()
        {
            return ref mouseState;
        }

        public static Point GetMousePosition()
        {
            SDL_GetMouseState(out int x, out int y);
            return new Point(x, y);
        }

        private static MouseButton TranslatePlatformMouseButton(byte button)
        {
            switch(button)
            {
                case 1: return MouseButton.Left;
                case 2: return MouseButton.Middle;
                case 3: return MouseButton.Right;
            }

            return MouseButton.None;
        }

        private static void TriggerMouseScroll(int value)
        {
            OnMouseScroll?.Invoke(null, value);
        }

        private static void TriggerMouseMove(int x, int y)
        {
            OnMouseMove?.Invoke(null, new Point(x, y));
        }


        private static void SetMouseButtonState(byte sdl_button, bool down)
        {
            MouseButton button = TranslatePlatformMouseButton(sdl_button);
            mouseState[button] = down;
        }
    }
}
