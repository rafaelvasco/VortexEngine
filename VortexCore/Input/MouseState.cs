namespace VortexCore
{
    public struct MouseState
    {
        private MouseButton buttonState;

        public bool this[MouseButton button]
        {
            get => (buttonState & button) == button;
            set
            {
                if (value)
                {
                    if (value)
                    {
                        buttonState |= button;
                    }
                    else
                    {
                        buttonState &= ~button;
                    }
                }
            }
        }
    }
}
