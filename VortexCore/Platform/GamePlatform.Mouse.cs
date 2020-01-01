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
