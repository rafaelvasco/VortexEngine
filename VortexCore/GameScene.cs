namespace VortexCore
{
    public abstract class GameScene
    {
        public static Game Game { get; internal set; }

        public abstract void Load();

        public virtual void Unload() { }

        public virtual void OnCanvasResize(int width, int height) { }

        public abstract void Init();

        public abstract void End();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(Graphics graphics);
    }
}
