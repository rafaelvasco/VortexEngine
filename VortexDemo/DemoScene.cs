using VortexCore;
using System;

namespace VortexDemo
{
    public class DemoScene : GameScene
    {
        // private float sx = 300;
        // private float sy = 300;
        // private float x = 100;
        // private float y = 100;

        private Sprite sprite;
        // private Player player;
        // private Gui gui;

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
            //graphics.FillRect(450, 450, 100, 100, Colors.Red);

            // graphics.FillRect(100, 100, 100, 100, Colors.Lime);

            // graphics.FillRect(400, 100, 100, 100, Colors.Blue);

            sprite.Draw(graphics);
            sprite.Draw(graphics, 300f, 300f);

            // player.Draw(graphics);
            // gui.Draw(graphics);

            
        }
       
        public override void Load()
        {
            sprite = new Sprite(Assets.LoadTexture("ball.png"));
            //player = new Player();
            //gui = new Gui();
        }

        public override void Update(float dt)
        {
            // x += sx * dt;
            // y += sy * dt;

            // if (x > 700)
            // {
            //     x = 700;
            //     sx = -sx;
            // }
            // else if (x < 0)
            // {
            //     x = 0;
            //     sx = -sx;
            // }

            // if (y > 500)
            // {
            //     y = 500;
            //     sy = -sy;
            // }
            // else if (y < 0)
            // {
            //     y = 0;
            //     sy = -sy;
            // }

            // if (Input.KeyDown(Key.A))
            // {
            //     gui.X -= 100 * dt;
            // }

            // if (Input.KeyDown(Key.D))
            // {
            //     gui.X += 100 * dt;
            // }

            // gui.Update(dt);
            // player.Update(dt);
        }
    }
}
