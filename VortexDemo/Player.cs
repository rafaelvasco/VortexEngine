using VortexCore;


namespace VortexDemo
{
    public class Player : GameObject
    {
        private Spritesheet spritesheet;
        private Animation idle;
        private Animation walking;
        private Animation currentAnimation;

        public Player()
        {
            spritesheet = Spritesheet.FromGrid(Assets.LoadTexture("spritesheet.png"), lines: 4, cols: 4, removeLast: 1);

            idle = new Animation(spritesheet, indices: 4);

            walking = new Animation(spritesheet, 4, 5, 6, 7);

            currentAnimation = idle;
        }

        public override void Draw(Graphics graphics, float parentX = 0, float parentY = 0)
        {
            this.currentAnimation.Draw(graphics);
        }

        public override void Update()
        {
            currentAnimation.Update();

            if (Input.KeyPressed(Key.Right))
            {
                currentAnimation = walking;
                currentAnimation.Play();
            }
            else if (Input.KeyReleased(Key.Right))
            {
                currentAnimation = idle;
                currentAnimation.Play();
            }
        }
    }
}
