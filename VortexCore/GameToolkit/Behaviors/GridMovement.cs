using System;
using System.Numerics;

namespace VortexCore
{
    public class GridMovement : Behavior
    {
        public InputAction MoveLeft { get; private set; }

        public InputAction MoveRight { get; private set; }

        public InputAction MoveUp { get; private set; }

        public InputAction MoveDown { get; private set; }

        public float MoveDuration { get; set; } = 0.1f;

        public float GridWidth { get; set; } = 64;

        public float GridHeight { get; set; } = 64;

        public bool Moving { get; private set; }

        private float currentTargetX = 0.0f;

        private float currentTargetY = 0.0f;

        private Vector2Tween movementTween;


        public GridMovement(bool activeOnStart) : base(activeOnStart)
        {
            MoveLeft = Input.Map(Key.Left);
            MoveRight = Input.Map(Key.Right);
            MoveUp = Input.Map(Key.Up);
            MoveDown = Input.Map(Key.Down);

            movementTween = new Vector2Tween();
            movementTween.OnFinish += OnMovementFinished;

        }

        public void OnMovementFinished() {
            Moving = false;
        }

        public override void Update(float dt)
        {
            if (!Moving)
            {
                if (MoveLeft.WasPressed)
                {
                    Moving = true;
                    currentTargetX = AttachedActor.X - GridWidth;
                    currentTargetY = AttachedActor.Y;
                }
                else if (MoveRight.WasPressed)
                {
                    Moving = true;
                    currentTargetX = AttachedActor.X + GridWidth;
                    currentTargetY = AttachedActor.Y;
                }
                else if (MoveUp.WasPressed)
                {
                    Moving = true;
                    currentTargetX = AttachedActor.X;
                    currentTargetY = AttachedActor.Y - GridHeight;
                }
                else if (MoveDown.WasPressed)
                {
                    Moving = true;
                    currentTargetX = AttachedActor.X;
                    currentTargetY = AttachedActor.Y + GridHeight;
                }
            }
            else
            {
                if (movementTween.State == TweenState.Stopped)
                {
                    movementTween.Start
                    (
                        new Vector2(AttachedActor.X, AttachedActor.Y),
                        new Vector2(currentTargetX, currentTargetY),
                        MoveDuration, TweenScaleFuncs.Linear
                    );
                }
                else
                {
                    movementTween.Update(dt);
                    AttachedActor.X = movementTween.CurrentValue.X;
                    AttachedActor.Y = movementTween.CurrentValue.Y;
                }

            }

        }

        protected override void OnAttached()
        {
            AttachedActor.X = Calc.Snap(AttachedActor.X, GridWidth);
            AttachedActor.Y = Calc.Snap(AttachedActor.Y, GridHeight);
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