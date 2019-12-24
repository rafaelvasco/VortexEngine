using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace VortexCore
{
    public struct Color : IEquatable<Color>
    {

        public static readonly Color Red = new Color(1, 0, 0, 1);
        public static readonly Color DarkRed = new Color(0.6f, 0, 0, 1);
        public static readonly Color Green = new Color(0, 1, 0, 1);
        public static readonly Color Blue = new Color(0, 0, 1, 1);
        public static readonly Color Yellow = new Color(1, 1, 0, 1);
        public static readonly Color Grey = new Color(.25f, .25f, .25f, 1);
        public static readonly Color LightGrey = new Color(.65f, .65f, .65f, 1);
        public static readonly Color Cyan = new Color(0, 1, 1, 1);
        public static readonly Color White = new Color(1, 1, 1, 1);
        public static readonly Color CornflowerBlue = new Color(0.3921f, 0.5843f, 0.9294f, 1);
        public static readonly Color Clear = new Color(0, 0, 0, 0);
        public static readonly Color Black = new Color(0, 0, 0, 1);
        public static readonly Color Pink = new Color(1f, 0.45f, 0.75f, 1);
        public static readonly Color Orange = new Color(1f, 0.36f, 0f, 1);


        public float R;
        public float G;
        public float B;
        public float A;

        public byte Ri => (byte)(R * 255);
        public byte Gi => (byte)(G * 255);
        public byte Bi => (byte)(B * 255);
        public byte Ai => (byte)(A * 255);


        public Color(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color(float v, float a = 1.0f)
        {
            R = v;
            G = v;
            B = v;
            A = a;
        }

        public Color(int rgba)
        {
            A = (rgba & 0xFF) / 255.0f;
            rgba >>= 8;
            B = (rgba & 0xFF) / 255.0f;
            rgba >>= 8;
            G = (rgba & 0xFF) / 255.0f;
            rgba >>= 8;
            R = (rgba & 0xFF) / 255.0f;
        }


        public Color(Vector4 vector4)
        {
            R = vector4.X;
            G = vector4.Y;
            B = vector4.Z;
            A = vector4.W;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return HashCode.Combine(R.GetHashCode(), G.GetHashCode(), B.GetHashCode(), A.GetHashCode());
        }

        public override string ToString()
        {
            return $"R:{R}, G:{G}, B:{B}, A:{A}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector4(Color color)
        {
            return new Vector4(color.R, color.G, color.B, color.A);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Color(Vector4 vector)
        {
            return new Color(vector);
        }

        public static implicit operator uint(Color color)
        {
            return (uint)color.ToRgba32();
        }

        public static implicit operator int(Color color)
        {
            return color.ToRgba32();
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

        public int ToArgb32()
        {
            int c = (byte)MathF.Round(A * 255.0f);
            c <<= 8;
            c |= (byte)MathF.Round(R * 255.0f);
            c <<= 8;
            c |= (byte)MathF.Round(G * 255.0f);
            c <<= 8;
            c |= (byte)MathF.Round(B * 255.0f);

            return c;
        }

        public int ToRgba32()
        {
            int c = (byte)MathF.Round(R * 255.0f);
            c <<= 8;
            c |= (byte)MathF.Round(G * 255.0f);
            c <<= 8;
            c |= (byte)MathF.Round(B * 255.0f);
            c <<= 8;
            c |= (byte)MathF.Round(A * 255.0f);

            return c;
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
