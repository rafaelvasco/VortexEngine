using System;
using System.Diagnostics;
using System.Threading;

namespace VortexCore
{
    public abstract class GameObject
    {
        private static long latestAssignedId = 0;

        public ulong Id { get; }

        public string Name { get; set; }

        public float X;

        public float Y;

        public float Rotation;

        public Color Tint;

        public float Alpha = 1.0f;

        public abstract float Width { get; }

        public abstract float Height { get; }

        public RectF BoundingRect 
        {
            get => new RectF(X, Y, Width, Height);
        }

        public bool Visible = true;

        protected GameObject() : this(Guid.NewGuid().ToString()) { }

        private GameObject(string name)
        {
            Name = name;
            Id = GetNextId();
        }

        public abstract void Update(float dt);

        public abstract void Draw(Graphics graphics, float parentX = 0, float parentY = 0);

        private static ulong GetNextId()
        {
            ulong newID = unchecked((ulong)Interlocked.Increment(ref latestAssignedId));
            Debug.Assert(newID != 0); // Overflow

            return newID;
        }
    }
}