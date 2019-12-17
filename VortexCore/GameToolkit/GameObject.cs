namespace VortexCore
{
    public abstract class GameObject
    {
        public string Id { get; internal set; }

        public float X;

        public float Y;

        public virtual float Width { get; }

        public virtual float Height { get; }

        public bool Visible = true;

        public abstract void Update();

        public abstract void Draw(Graphics graphics, float parentX = 0, float parentY = 0);


    }
}
