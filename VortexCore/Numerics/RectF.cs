using System;
using System.Runtime.InteropServices;
using System.Numerics;

namespace VortexCore
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RectF : IEquatable<RectF>
    {
        public static readonly RectF Empty = new RectF(0f, 0f, 0f, 0f);

        public float X;

        public float Y;

        public float Width;

        public float Height;


        public bool IsEmpty => Width == 0f && Height == 0f;


        /// <summary>
        ///     Gets or sets the size of rectangle.
        /// </summary>
        /// <value>The size of rectangle.</value>
        public SizeF Size
        {
            get { return new SizeF(Width, Height); }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        /// <summary>
        ///     Gets or sets the width half.
        /// </summary>
        /// <value>The half of width .</value>
        public float HalfWidth
        {
            get { return Width / 2; }
            set { Width = value * 2; }
        }

        /// <summary>
        ///     Gets or sets the half of height .
        /// </summary>
        /// <value>The half of height.</value>
        public float HalfHeight
        {
            get { return Height / 2; }
            set { Height = value * 2; }
        }

        /// <summary>
        ///     Gets or sets the half of size of rectangle.
        /// </summary>
        /// <value>The half of size of rectangle.</value>
        public Vector2 HalfSize
        {
            get { return new Vector2(HalfWidth, HalfHeight); }
            set
            {
                HalfWidth = value.X;
                HalfHeight = value.Y;
            }
        }

        /// <summary>
        ///     Gets or sets the center X.
        /// </summary>
        /// <value>The center X.</value>
        public float CenterX
        {
            get { return X + Width / 2; }
            set
            {
                var delta = value - CenterX;
                X += delta;
            }
        }

        /// <summary>
        ///     Gets or sets the center Y.
        /// </summary>
        /// <value>The center Y.</value>
        public float CenterY
        {
            get { return Y + Height / 2; }
            set
            {
                var delta = value - CenterY;
                Y += delta;
            }
        }

        /// <summary>
        ///     Gets or sets the center of rectangle.
        /// </summary>
        /// <value>The center of rectangle.</value>
        public Vector2 Center
        {
            get { return new Vector2(CenterX, CenterY); }
            set
            {
                CenterX = value.X;
                CenterY = value.Y;
            }
        }

        /// <summary>
        ///     Gets or sets the left top point.
        /// </summary>
        /// <value>The left top point.</value>
        public Vector2 TopLeft
        {
            get { return new Vector2(X, Y); }
        }

        /// <summary>
        ///     Gets or sets the right top  point.
        /// </summary>
        /// <value>The right top point.</value>
        public Vector2 TopRight
        {
            get { return new Vector2(X+Width, Y); }
        }

        /// <summary>
        ///     Gets or sets the right bottom point.
        /// </summary>
        /// <value>The right bottom point.</value>
        public Vector2 BottomRight
        {
            get { return new Vector2(X+Width, Y+Height); }
        }

        /// <summary>
        ///     Gets or sets the left bottom point.
        /// </summary>
        /// <value>The left bottom point.</value>
        public Vector2 BottomLeft
        {
            get { return new Vector2(X, Y+Height); }
        }


        public RectF(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }




        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///     true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(RectF other)
        {
            return (X == other.X) && (Y == other.Y) && (Width == other.Width) && (Height == other.Height);
        }

        /// <summary>
        ///     Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///     <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object other)
        {
            return other is RectF && Equals((RectF)other);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return (int)X + (int)Y + (int)Width + (int)Height;
        }

        /// <summary>
        ///     Compares two instances of <see cref="Rect" />.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns><c>true</c> if values of type <see cref="Rect" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool Equals(ref RectF value1, ref RectF value2)
        {
            return value1.X == value2.X && value1.Y == value2.Y && value1.Width == value2.Width &&
                   value1.Height == value2.Height;
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this rect.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this rect.
        /// </returns>
        public override string ToString()
        {
            return $"{{{X},{Y},{Width},{Height}}}";
        }

            
        public RectF Inflate(float delta)
        {
            return new RectF(X - delta, Y - delta, Width + 2 * delta, Height + 2 * delta);
        }

        public RectF Deflate(float delta)
        {
            return new RectF(X + delta, Y + delta, Width - 2 * delta, Height - 2 * delta);
        }
          


        public bool Contains(int pointX, int pointY)
        {
            return this.X <= pointX && this.Y <= pointY && this.X + this.Width > pointX && this.Y + this.Height > pointY;
        }

        public bool Contains(Point point)
        {
            return this.Contains(point.X, point.Y);
        }

        public bool Contains(Rect rect)
        {
            return
                this.X <= rect.X &&
                this.Y <= rect.Y &&
                this.X + this.Width >= rect.X + rect.Width &&
                this.Y + this.Height >= rect.Y + rect.Height;
        }

        public bool Intersects(Rect rect)
        {
            return this.X <= rect.X + rect.Width &&
                   this.Y <= rect.Y + rect.Height &&
                   this.X + this.Width >= rect.X &&
                   this.Y + this.Height >= rect.Y;
        }

        public static bool operator ==(RectF value1, RectF value2)
        {
            return Equals(ref value1, ref value2);
        }

        public static bool operator !=(RectF value1, RectF value2)
        {
            return !Equals(ref value1, ref value2);
        }

    }
}