using System.Numerics;

namespace VortexCore
{
    public class FourAxisMovement : Behavior
    {
        public InputAction MoveLeft { get; private set; }

        public InputAction MoveRight { get; private set; }

        public InputAction MoveUp { get; private set; }

        public InputAction MoveDown { get; private set; }

        public float Speed { get; set; } = 150;

        public Vector2 Velocity { get; private set; }

        public float Friction { get; set; } = 1.0f;

        public bool MoveAtDiagonals { get; set; } = true;

        internal FourAxisMovement(bool activeOnStart) : base(activeOnStart)
        {
            MoveLeft = Input.Map(Key.Left);
            MoveRight = Input.Map(Key.Right);
            MoveUp = Input.Map(Key.Up);
            MoveDown = Input.Map(Key.Down);
        }

        public override void Update(float dt)
        {
            var actor = AttachedActor;
            var speedDt = Speed * dt;
            var vx = Velocity.X;
            var vy = Velocity.Y;

            if (MoveAtDiagonals)
            {
                if (MoveLeft.IsDown)
                {
                    vx -= speedDt;
                }
                if (MoveRight.IsDown)
                {
                    vx += speedDt;
                }
                if (MoveUp.IsDown)
                {
                    vy -= speedDt;
                }
                if (MoveDown.IsDown)
                {
                    vy += speedDt;
                }
            }
            else
            {
                if (MoveLeft.IsDown)
                {
                    vx -= speedDt;
                }
                else if (MoveRight.IsDown)
                {
                    vx += speedDt;
                }
                else if (MoveUp.IsDown)
                {
                    vy -= speedDt;
                }
                else if (MoveDown.IsDown)
                {
                    vy += speedDt;
                }
            }


            actor.X += vx;
            actor.Y += vy;

            vx *= (1 - Friction);
            vy *= (1 - Friction);

            Velocity = new Vector2(vx, vy);

        }

        protected override void OnAttached()
        {

        }

        protected override void OnDetached()
        {

        }

        protected override void OnDisabled()
        {

        }

        protected override void OnEnabled()
        {

        }
    }
}