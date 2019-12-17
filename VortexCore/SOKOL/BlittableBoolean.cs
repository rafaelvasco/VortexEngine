using System;

namespace VortexCore
{
    public struct BlittableBoolean
    {
        private byte byteValue;

        public bool Value
        {
            get => Convert.ToBoolean(byteValue);
            set => byteValue = Convert.ToByte(value);
        }

        public static implicit operator BlittableBoolean(bool value)
        {
            return new BlittableBoolean { Value = value };
        }

        public static implicit operator bool(BlittableBoolean value)
        {
            return value.Value;
        }
    }
}
