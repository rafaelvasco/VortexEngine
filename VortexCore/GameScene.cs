﻿namespace VortexCore
{
    public abstract class GameScene
    {
        public static Game Game { get; internal set; }

        public float TimeScale { get; set; } = 1.0f;

        public abstract void Load();

        public virtual void Unload() { }

        public virtual void OnCanvasResize(int width, int height) { }

        public abstract void Init();

        public abstract void End();

        public abstract void Update(float dt);

        public abstract void Draw(Graphics graphics);
    }
}