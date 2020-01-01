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
using System.Collections.Generic;

namespace VortexCore
{
    public class InputAction
    {

        public int InputCode { get; internal set; }

        public Action Action { get; internal set; }

        public bool WasPressed 
        {
            get => Input.KeyPressed((Key)this.InputCode) ||
                   Input.MousePressed((MouseButton)this.InputCode);
        }

        public bool WasReleased 
        {
            get => Input.KeyReleased((Key)this.InputCode) ||
                   Input.MouseReleased((MouseButton)this.InputCode);
        }

        public bool IsDown 
        {
            get => Input.KeyDown((Key)this.InputCode) ||
                   Input.MouseDown((MouseButton)this.InputCode);
        }

        internal InputAction(int inputCode, Action action)
        {
            this.InternalMap(inputCode, action);
        }

        public void Map(Key key, Action action = null)
        {
            this.InternalMap((int)key, action);
        }

        public void Map(MouseButton button, Action action = null)
        {
            this.InternalMap((int)button, action);
        }

        private void InternalMap(int inputCode, Action action)
        {
            this.InputCode = inputCode;
            this.Action = action;
        }

        public void Execute()
        {
            this.Action?.Invoke();
        }

        public void ExecuteIfPressed()
        {
            if (WasPressed)
            {
                Execute();
            }
        }

        public void ExecuteIfDown()
        {
            if (IsDown)
            {
                Execute();
            }
        }

        public void ExecuteIfReleased()
        {
            if (WasReleased)
            {
                Execute();
            }

        }
    }

    public static class Input
    {
        private static KeyState prevKeyState;
        private static KeyState curKeyState;
        private static MouseState prevMouseState;
        private static MouseState curMouseState;
        private static Point mousePosition;

        private static Dictionary<int, InputAction> inputActionsPool = new Dictionary<int, InputAction>();

        public static int MouseX => mousePosition.X;

        public static int MouseY => mousePosition.Y;

        public static int ScrollValue { get; private set; }

        internal static void Init()
        {
            GamePlatform.OnMouseScroll += OnGamePlatformMouseScroll;
            GamePlatform.OnMouseMove += OnGamePlatformMouseMove;
        }

        private static InputAction MapInternal(int inputCode, Action action)
        {

            if (inputActionsPool.TryGetValue(inputCode, out var inputAction))
            {
                inputAction.InputCode = inputCode;
                inputAction.Action = action;
                return inputAction;
            }

            var newInputAction = new InputAction(inputCode, action);

            inputActionsPool.Add(inputCode, newInputAction);

            return newInputAction;
        }

        public static InputAction Map(Key key, Action action = null)
        {
            return MapInternal((int)key, action);
        }

        public static InputAction Map(MouseButton mouseButton, Action action = null)
        {
            return MapInternal((int)mouseButton, action);
        }

        public static bool KeyDown(Key key)
        {
            return curKeyState[key];
        }

        public static bool KeyPressed(Key key)
        {
            return curKeyState[key] && !prevKeyState[key];
        }

        public static bool KeyReleased(Key key)
        {
            return !curKeyState[key] && prevKeyState[key];
        }

        public static bool MouseDown(MouseButton button)
        {
            return curMouseState[button];
        }

        public static bool MousePressed(MouseButton button)
        {
            return curMouseState[button] && !prevMouseState[button];
        }

        public static bool MouseReleased(MouseButton button)
        {
            return !curMouseState[button] && prevMouseState[button];
        }

        private static void OnGamePlatformMouseMove(object sender, Point e)
        {
            mousePosition = e;
        }

        private static void OnGamePlatformMouseScroll(object sender, int e)
        {
            ScrollValue += e;
        }

        internal static void Update()
        {
            prevKeyState = curKeyState;

            curKeyState = GamePlatform.GetKeyState();

            prevMouseState = curMouseState;

            curMouseState = GamePlatform.GetMouseState();

        }

        internal static void PostUpdate()
        {
            ScrollValue = 0;
        }


    }
}
