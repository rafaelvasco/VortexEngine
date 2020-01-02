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

using System.Collections.Generic;

namespace VortexCore
{
    public class Group : GameObject
    {
        public List<GameObject> GameObjects { get; private set; }

        public override float Width => groupWidth;

        public override float Height => groupHeight;

        public override float Rotation 
        {
            get => 0.0f;
            set {} //TODO:
        }
        public override float Opacity 
        {
            get => opacity;
            set 
            {
                opacity = value;
                foreach(var child in GameObjects) 
                {
                    child.Opacity = opacity;
                }
            }
        }


        private float opacity;
        private float groupWidth;
        private float groupHeight;

        public Group()
        {
            GameObjects = new List<GameObject>();
            groupWidth = 0;
            groupHeight = 0;
        }

        public void Add(GameObject gameObject)
        {
            GameObjects.Add(gameObject);

            RecalculateGroupSize();
        }

        public override void Update(float dt)
        {
            for (var i = 0; i < GameObjects.Count; ++i)
            {
                GameObjects[i].Update(dt);
            }
        }

        public override void Draw(Graphics graphics, float parentX = 0, float parentY = 0)
        {
            if(Visible)
            {
                for (var i = 0; i < GameObjects.Count; ++i)
                {
                    GameObjects[i].Draw(graphics, parentX + X, parentY + Y);
                }
            }
        }

        private void RecalculateGroupSize()
        {
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            for (var i = 0; i < GameObjects.Count; ++i)
            {
                var gameObject = GameObjects[i];

                if (gameObject.X < minX)
                {
                    minX = gameObject.X;
                }

                if (gameObject.X + gameObject.Width > maxX)
                {
                    maxX = gameObject.X + gameObject.Width;
                }

                if (gameObject.Y < minY)
                {
                    minY = gameObject.Y;
                }

                if (gameObject.Y + gameObject.Height > maxY)
                {
                    maxY = gameObject.Y + gameObject.Height;
                }

                groupWidth = maxX - minX;
                groupHeight = maxY - minY;

            }


        }
    }
}
