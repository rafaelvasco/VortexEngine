/* 
MIT License
Copyright (c) 2019 Rafael Vasco
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

using System.Numerics;

namespace VortexCore
{
    public class GridMovement : Behavior
    {
        public event ChangeDirectionFunc ChangedDirection;

        public event BehaviorEventFunc StartedMoving;

        public event BehaviorEventFunc StoppedMoving;

        public InputAction MoveLeft { get; private set; }

        public InputAction MoveRight { get; private set; }

        public InputAction MoveUp { get; private set; }

        public InputAction MoveDown { get; private set; }

        public float MoveDuration { get; set; } = 0.1f;

        public float GridWidth { get; set; } = 64;

        public float GridHeight { get; set; } = 64;

        public bool Moving { get; private set; }

        public FourWayDirection Direction
        {
            get => currentDirection;
            set 
            {
                if (currentDirection != value) 
                {
                    ChangedDirection?.Invoke(currentDirection, value);
                    currentDirection = value;
                }
            }
        }

        private float currentTargetX = 0.0f;

        private float currentTargetY = 0.0f;

        private Vector2Tween movementTween;

        private FourWayDirection currentDirection;


        public GridMovement(bool activeOnStart) : base(activeOnStart)
        {
            MoveLeft = Input.Map(Key.Left);
            MoveRight = Input.Map(Key.Right);
            MoveUp = Input.Map(Key.Up);
            MoveDown = Input.Map(Key.Down);

            movementTween = new Vector2Tween();
            movementTween.OnFinish += OnMovementFinished;

        }

        private void OnMovementFinished()
        {
            Moving = false;
            StoppedMoving?.Invoke();
        }

        public override void Update(float dt)
        {
            if (!Enabled) 
            {
                return;
            }

            if (!Moving)
            {
                if (MoveLeft.WasPressed)
                {
                    Direction = FourWayDirection.Left;
                    Moving = true;
                    currentTargetX = AttachedActor.X - GridWidth;
                    currentTargetY = AttachedActor.Y;
                    StartedMoving?.Invoke();
                }
                else if (MoveRight.WasPressed)
                {
                    Direction = FourWayDirection.Right;
                    Moving = true;
                    currentTargetX = AttachedActor.X + GridWidth;
                    currentTargetY = AttachedActor.Y;
                    StartedMoving?.Invoke();
                }
                else if (MoveUp.WasPressed)
                {
                    Direction = FourWayDirection.Up;
                    Moving = true;
                    currentTargetX = AttachedActor.X;
                    currentTargetY = AttachedActor.Y - GridHeight;
                    StartedMoving?.Invoke();
                }
                else if (MoveDown.WasPressed)
                {
                    Direction = FourWayDirection.Down;
                    Moving = true;
                    currentTargetX = AttachedActor.X;
                    currentTargetY = AttachedActor.Y + GridHeight;
                    StartedMoving?.Invoke();
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