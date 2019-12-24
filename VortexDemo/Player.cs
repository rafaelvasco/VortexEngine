using VortexCore;


namespace VortexDemo
{
    public class Player : Actor
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

            //FourAxisMovement behavior = AddBehavior<FourAxisMovement>();

            //behavior.Friction = 0.05f;

            AddBehavior<GridMovement>();

        }

        public override void Draw(Graphics graphics, float parentX = 0, float parentY = 0)
        {
            this.currentAnimation.Draw(graphics, this.X, this.Y);
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            currentAnimation.Update(dt);
        }
    }
}
