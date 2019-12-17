﻿using System;
using System.Runtime.InteropServices;

namespace VortexCore
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SizeF : IEquatable<SizeF>
    {
        public float Width;
        public float Height;

        public SizeF(float width, float height)
        {
            this.Width = width;
            this.Height = height;
        }

        public SizeF(Size size)
        {
            this.Width = size.Width;
            this.Height = size.Height;
        }

        public bool Equals(SizeF other)
        {
            return this.Width == other.Width && this.Height == other.Height;
        }

        public static bool Equals(ref SizeF value1, ref SizeF value2)
        {
            return value1.Width == value2.Width && value1.Height == value2.Height;
        }

        public static bool operator ==(SizeF value1, SizeF value2)
        {
            return Equals(ref value1, ref value2);
        }

        public static bool operator !=(SizeF value1, SizeF value2)
        {
            return !Equals(ref value1, ref value2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is SizeF && Equals((SizeF)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Width.GetHashCode() * 397) ^ Height.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"[Width={Width},Height={Height}]";
        }
    }
}
