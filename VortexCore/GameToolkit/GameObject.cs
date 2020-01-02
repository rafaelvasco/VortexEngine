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

        public abstract float Rotation { get; set; }

        public abstract float Opacity { get; set; }

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