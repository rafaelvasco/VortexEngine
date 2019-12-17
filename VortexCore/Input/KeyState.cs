
namespace VortexCore
{
    public struct KeyState
    {
        static readonly Key[] Empty = new Key[0];

        uint keys0, keys1, keys2, keys3, keys4, keys5, keys6, keys7;

        internal void SetState(Key key)
        {
            uint mask = (uint)1 << (((int)key) & 0x1f);
            switch (((int)key) >> 5)
            {
                case 0: keys0 |= mask; break;
                case 1: keys1 |= mask; break;
                case 2: keys2 |= mask; break;
                case 3: keys3 |= mask; break;
                case 4: keys4 |= mask; break;
                case 5: keys5 |= mask; break;
                case 6: keys6 |= mask; break;
                case 7: keys7 |= mask; break;
            }
        }

        internal bool GetState(Key key)
        {
            uint mask = (uint)1 << (((int)key) & 0x1f);

            uint element;
            switch (((int)key) >> 5)
            {
                case 0: element = keys0; break;
                case 1: element = keys1; break;
                case 2: element = keys2; break;
                case 3: element = keys3; break;
                case 4: element = keys4; break;
                case 5: element = keys5; break;
                case 6: element = keys6; break;
                case 7: element = keys7; break;
                default: element = 0; break;
            }

            return (element & mask) != 0;
        }

        internal void ClearState(Key key)
        {
            var mask = (uint)1 << ((int)key & 0x1f);
            switch ((int)key >> 5)
            {
                case 0:
                    keys0 &= ~mask;
                    break;
                case 1:
                    keys1 &= ~mask;
                    break;
                case 2:
                    keys2 &= ~mask;
                    break;
                case 3:
                    keys3 &= ~mask;
                    break;
                case 4:
                    keys4 &= ~mask;
                    break;
                case 5:
                    keys5 &= ~mask;
                    break;
                case 6:
                    keys6 &= ~mask;
                    break;
                case 7:
                    keys7 &= ~mask;
                    break;
            }
        }

        internal KeyState(Key[] keys) : this()
        {
            keys0 = 0;
            keys1 = 0;
            keys2 = 0;
            keys3 = 0;
            keys4 = 0;
            keys5 = 0;
            keys6 = 0;
            keys7 = 0;

            if (keys != null)
            {
                foreach (var key in keys)
                {
                    SetState(key);
                }
            }
        }

        public bool this[Key key] => GetState(key);

        public Key[] GetPressedKeys()
        {
            uint count = CountBits(keys0) + CountBits(keys1) + CountBits(keys2) + CountBits(keys3)
                    + CountBits(keys4) + CountBits(keys5) + CountBits(keys6) + CountBits(keys7);
            if (count == 0)
                return Empty;
            Key[] keys = new Key[count];

            int index = 0;
            if (keys0 != 0) index = AddKeysToArray(keys0, 0 * 32, keys, index);
            if (keys1 != 0) index = AddKeysToArray(keys1, 1 * 32, keys, index);
            if (keys2 != 0) index = AddKeysToArray(keys2, 2 * 32, keys, index);
            if (keys3 != 0) index = AddKeysToArray(keys3, 3 * 32, keys, index);
            if (keys4 != 0) index = AddKeysToArray(keys4, 4 * 32, keys, index);
            if (keys5 != 0) index = AddKeysToArray(keys5, 5 * 32, keys, index);
            if (keys6 != 0) index = AddKeysToArray(keys6, 6 * 32, keys, index);
            if (keys7 != 0) index = AddKeysToArray(keys7, 7 * 32, keys, index);

            return keys;
        }

        public override int GetHashCode()
        {
            return (int)(keys0 ^ keys1 ^ keys2 ^ keys3 ^ keys4 ^ keys5 ^ keys6 ^ keys7);
        }

        public static bool operator == (KeyState a, KeyState b)
        {
            return a.keys0 == b.keys0
                && a.keys1 == b.keys1
                && a.keys2 == b.keys2
                && a.keys3 == b.keys3
                && a.keys4 == b.keys4
                && a.keys5 == b.keys5
                && a.keys6 == b.keys6
                && a.keys7 == b.keys7;
        }

        public static bool operator != (KeyState a, KeyState b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return obj is KeyState && this == (KeyState)obj;
        }

        private static uint CountBits(uint v)
        {
            // This uses some complete magic from:
            // http://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetParallel
            v = v - ((v >> 1) & 0x55555555);                    // reuse input as temporary
            v = (v & 0x33333333) + ((v >> 2) & 0x33333333);     // temp
            return ((v + (v >> 4) & 0xF0F0F0F) * 0x1010101) >> 24; // count
        }

        private static int AddKeysToArray(uint keys, int offset, Key[] pressedKeys, int index)
        {
            for (int i = 0; i < 32; i++)
            {
                if ((keys & (1 << i)) != 0)
                    pressedKeys[index++] = (Key)(offset + i);
            }
            return index;
        }
    }
}
