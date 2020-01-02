using System;
using System.Numerics;
using VortexCore;


namespace VortexDemo
{
    public class Player : Actor
    {
        //TODO: Refactor GameObject system
        private Spritesheet spritesheet;
        private Animation idleHorizontal;
        private Animation walkingHorizontal;

        private Animation walkingUp;

        private Animation walkingDown;

        private Animation idleUp;

        private Animation idleDown;

        private Animation currentAnimation;

        private FourAxisMovement movementBehavior;

        private Interactive interactiveBehavior;

        public override float Width => currentAnimation.Width;

        public override float Height => currentAnimation.Width;

        public Player()
        {
            spritesheet = Spritesheet.FromGrid(Assets.LoadTexture("spritesheet.png"), lines: 4, cols: 4, removeLast: 1);

            idleHorizontal = new Animation(spritesheet, indices: 5);

            idleUp = new Animation(spritesheet, 0);

            idleDown = new Animation(spritesheet, 8);

            walkingHorizontal = new Animation(spritesheet, 4, 5, 6, 7);

            walkingUp = new Animation(spritesheet, 0, 1, 2, 3);

            walkingDown = new Animation(spritesheet, 8, 9, 10, 11);

            currentAnimation = idleHorizontal;

            movementBehavior = AddBehavior<FourAxisMovement>();

            movementBehavior.Friction = 1;

            movementBehavior.MaxVelocity = new Vector2(7.0f, 7.0f);

            movementBehavior.Speed = 250;

            movementBehavior.Direction = FourWayDirection.Right;

            movementBehavior.StoppedMoving += OnStoppedMoving;

            interactiveBehavior = AddBehavior<Interactive>();

            interactiveBehavior.Draggable = true;

            interactiveBehavior.OnClick += () => { Console.WriteLine("CLICK"); };
            interactiveBehavior.OnMouseDown += () => { Console.WriteLine("MOUSE DOWN"); };
            interactiveBehavior.OnMouseOver += () => { Console.WriteLine("MOUSE OVER"); };
            interactiveBehavior.OnMouseLeave += () => { Console.WriteLine("MOUSE LEAVE"); };

            //var gridMovement = AddBehavior<GridMovement>();

            //gridMovement.ChangedDirection += OnDirectionChange;

        }

        private void OnStoppedMoving()
        {
            switch (movementBehavior.Direction)
            {
                case FourWayDirection.Left:
                    currentAnimation = idleHorizontal;
                    currentAnimation.FlipHorizontal = true;
                    break;
                case FourWayDirection.Right:
                    currentAnimation = idleHorizontal;
                    currentAnimation.FlipHorizontal = false;
                    break;
                case FourWayDirection.Up:
                    currentAnimation = idleUp;
                    currentAnimation.FlipHorizontal = false;
                    break;
                case FourWayDirection.Down:
                    currentAnimation = idleDown;
                    currentAnimation.FlipHorizontal = false;
                    break;
            }
            currentAnimation.FrameIndex = 0;
            
        }

        public override void Draw(Graphics graphics, float parentX = 0, float parentY = 0)
        {
            this.currentAnimation.Draw(graphics, this.X, this.Y);
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            var left = movementBehavior.MoveLeft.IsDown;
            var right = movementBehavior.MoveRight.IsDown;
            var up = movementBehavior.MoveUp.IsDown;
            var down = movementBehavior.MoveDown.IsDown;

            if (left && (!up && !down && !right))
            {
                this.currentAnimation = walkingHorizontal;
                this.currentAnimation.FlipHorizontal = true;
            }
            else if (right && (!up && !down && !left))
            {
                this.currentAnimation = walkingHorizontal;
                this.currentAnimation.FlipHorizontal = false;
            }

            else if (up && (!left && !right && !down))
            {
                this.currentAnimation = walkingUp;
                this.currentAnimation.FlipHorizontal = false;
            }
            else if (down && (!left && !right && !up))
            {
                this.currentAnimation = walkingDown;
                this.currentAnimation.FlipHorizontal = false;
            }

            currentAnimation.Update(dt);
        }
    }
}
