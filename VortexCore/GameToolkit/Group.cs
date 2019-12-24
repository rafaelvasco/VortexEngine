using System.Collections.Generic;

namespace VortexCore
{
    public class Group : GameObject
    {
        public List<GameObject> GameObjects { get; private set; }

        public override float Width => groupWidth;

        public override float Height => groupHeight;

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
