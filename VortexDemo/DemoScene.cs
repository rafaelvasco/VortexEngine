using VortexCore;
using System;

namespace VortexDemo
{
    public class DemoScene : GameScene
    {
        private float sx = 300;
        private float sy = 300;
        private float x = 100;
        private float y = 100;

        private Sprite sprite;
        private Player player;
        private Gui gui;

        public DemoScene()
        {
        }

        public override void End()
        {
        }

        public override void Init()
        {
        }

        public override void Draw(Graphics graphics)
        {
            graphics.FillRect(550, 400, 100, 100, Colors.Lime);
            graphics.FillRect(400, 100, 100, 100, Colors.Blue);
            graphics.FillRect(400, 400, 100, 100, Colors.Red);
            graphics.FillRect(x, y, 100, 100, Colors.Purple);

            sprite.Draw(graphics, 200f, 300f);
            sprite.Draw(graphics);

            player.Draw(graphics);
            gui.Draw(graphics);

            Game.Title = $"Draw Calls: {graphics.MaxDrawCalls}";
            
        }
       
        public override void Load()
        {
            sprite = new Sprite(Assets.LoadTexture("ball.png"));
            player = new Player();
            gui = new Gui();
        }

        public override void Update(float dt)
        {
            
            x += sx * dt;
            y += sy * dt;

            if (x > 700)
            {
                x = 700;
                sx = -sx;
            }
            else if (x < 0)
            {
                x = 0;
                sx = -sx;
            }

            if (y > 500)
            {
                y = 500;
                sy = -sy;
            }
            else if (y < 0)
            {
                y = 0;
                sy = -sy;
            }

            if (Input.KeyDown(Key.A))
            {
                gui.X -= 100 * dt;
            }

            if (Input.KeyDown(Key.D))
            {
                gui.X += 100 * dt;
            }

            gui.Update(dt);
            player.Update(dt);
        }
    }
}
