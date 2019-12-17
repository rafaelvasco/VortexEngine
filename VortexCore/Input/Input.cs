namespace VortexCore
{
    public static class Input
    {
        private static KeyState prevKeyState;
        private static KeyState curKeyState;
        private static MouseState prevMouseState;
        private static MouseState curMouseState;
        private static Point mousePosition;


        public static int MouseX => mousePosition.X;

        public static int MouseY => mousePosition.Y;

        public static int ScrollValue { get; private set; }

        internal static void Init()
        {
            GamePlatform.OnMouseScroll += OnGamePlatformMouseScroll;
            GamePlatform.OnMouseMove += OnGamePlatformMouseMove;
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
