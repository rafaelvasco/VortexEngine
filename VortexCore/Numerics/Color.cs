/* Adapted from https://github.com/lithiumtoast/sokol-sharp/blob/develop/src/Sokol.Graphics/RgbaFloat.cs and */
/* from https://github.com/mellinoe/veldrid/blob/master/src/Veldrid/RgbaFloat.cs */
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
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace VortexCore
{
    public struct Color : IEquatable<Color>
    {
        private readonly Vector4 channels;
        public float R => channels.X;
        public float G => channels.Y;
        public float B => channels.Z;
        public float A => channels.W;

        public byte Rb => (byte)MathF.Round(channels.X * 255.0f); 
        public byte Gb => (byte)MathF.Round(channels.Y * 255.0f); 
        public byte Bb => (byte)MathF.Round(channels.Z * 255.0f); 
        public byte Ab => (byte)MathF.Round(channels.W * 255.0f); 
     

        public Color(float r, float g, float b, float a = 1f)
        {
            channels = new Vector4(r, g, b, a);
        }


        public Color(float v, float a = 1.0f)
        {
            channels = new Vector4(v, v, v, a);
        }

        public Color(Vector4 vector4)
        {
            channels = vector4;
        }

        public Color(byte r, byte g, byte b, byte a = 255) 
        {
            channels = new Vector4
            (
                r/255f,
                g/255f,
                b/255f,
                a/255f
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return channels.GetHashCode();
        }

        public override string ToString()
        {
            return $"R:{R}, G:{G}, B:{B}, A:{A}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector4(Color color)
        {
            return color.channels;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Color(Vector4 vector)
        {
            return new Color(vector);
        }

        public static implicit operator Color(uint value)
        {
            var r = ((value >> 24) & 0xFF) / 255f;
            var g = ((value >> 16) & 0xFF) / 255f;
            var b = ((value >> 8) & 0xFF) / 255f;
            var a = (value & 0xFF) / 255f;

            return new Color(r, g, b, a);
        }

        public static implicit operator uint(Color color)
        {   
            return unchecked((uint)((color.Rb << 24) | (color.Gb << 16) | (color.Bb << 8) | (color.Ab)));
        }

        public static implicit operator Color(string hex)
        {
            uint value;
            try
            {
                var span = hex.AsSpan();
                if (span[0] == '#')
                {
                    span = span.Slice(1);
                }
                value = uint.Parse(span, NumberStyles.HexNumber);
                if (span.Length == 6)
                {
                    value = (value << 8) + 0xFF;
                }
            }
            catch
            {
                throw new ArgumentException($"Failed to parse the hex rgb '{hex}' as an unsigned 32-bit integer.");
            }

            return value;
        }

        public static bool operator ==(Color left, Color right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Color left, Color right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (obj is Color)
            {
                return Equals((Color)obj);
            }

            return false;
        }

        public bool Equals(Color other)
        {
            return R == other.R && G == other.G && B == other.B && A == other.A;
        }
      
        public static Color Lerp(Color value1, Color value2, float amount)
        {
            return new Color
            (
                Calc.Lerp(value1.R, value2.R, amount),
                Calc.Lerp(value1.G, value2.G, amount),
                Calc.Lerp(value1.B, value2.B, amount),
                Calc.Lerp(value1.A, value2.A, amount)
            );
        }
    }
}
