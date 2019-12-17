using VortexCore;

namespace VortexDemo
{
    public class DemoScene : GameScene
    {
        private float sx = 3;
        private float sy = 3;
        private float x = 100;
        private float y = 100;

        private Sprite sprite;
        private Player player;
        private Gui gui;

        public override void Draw(Graphics graphics)
        {
            sprite.Draw(graphics);
            player.Draw(graphics);
            gui.Draw(graphics);
            graphics.FillRect(x, y, 100, 100, Color.Red);
        }

        public override void End()
        {
        }

        public override void Init()
        {
        }

        public override void Load()
        {
            sprite = new Sprite(Assets.LoadTexture("ball.png"));
            player = new Player();
            gui = new Gui();
        }

        public override void Update(GameTime gameTime)
        {
            x += sx;
            y += sy;

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
                gui.X -= 1;
            }

            if (Input.KeyDown(Key.D))
            {
                gui.X += 1;
            }
 

            gui.Update();
            player.Update();
        }
    }
}
