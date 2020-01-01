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
    public class FourAxisMovement : Behavior
    {

        public event ChangeDirectionFunc ChangedDirection;

        public event BehaviorEventFunc StartedMoving;

        public event BehaviorEventFunc StoppedMoving;

        public InputAction MoveLeft { get; private set; }

        public InputAction MoveRight { get; private set; }

        public InputAction MoveUp { get; private set; }

        public InputAction MoveDown { get; private set; }

        public float Speed { get; set; } = 80;

        public Vector2 Velocity { get; private set; }

        public float Friction { get; set; } = 0.3f;

        public Vector2 MaxVelocity {get;set;} = new Vector2(100.0f, 100.0f);

        private bool moving = false;

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

        private bool isInputDown = false;

        private FourWayDirection currentDirection;

        public FourAxisMovement(bool activeOnStart) : base(activeOnStart)
        {
            MoveLeft = Input.Map(Key.Left);
            MoveRight = Input.Map(Key.Right);
            MoveUp = Input.Map(Key.Up);
            MoveDown = Input.Map(Key.Down);
        }

        public override void Update(float dt)
        {
            if (!Enabled) 
            {
                return;
            }

            var actor = AttachedActor;
            var speedDt = Speed * dt;
            var vx = Velocity.X;
            var vy = Velocity.Y;
            var oldVx = vx;
            var oldVy = vy;

            if (MoveLeft.IsDown)
            {
                Direction = FourWayDirection.Left;
                moving = true;
             
                if (!isInputDown)
                {
                    isInputDown = true;
                    
                    StartedMoving?.Invoke();
                }

                vx -= speedDt;
            }
            if (MoveRight.IsDown)
            {
                Direction = FourWayDirection.Right;
                moving = true;

                if (!isInputDown)
                {
                    isInputDown = true;
                    StartedMoving?.Invoke();
                }

                vx += speedDt;
            }
            if (MoveUp.IsDown)
            {
                Direction = FourWayDirection.Up;
                moving = true;

                if (!isInputDown)
                {
                    isInputDown = true;
                    StartedMoving?.Invoke();
                }

                vy -= speedDt;
            }
            if (MoveDown.IsDown)
            {
                Direction = FourWayDirection.Down;
                moving = true;
                
                if (!isInputDown)
                {
                    isInputDown = true;
                    StartedMoving?.Invoke();
                }

                vy += speedDt;
            }

            if (!MoveLeft.IsDown && !MoveRight.IsDown && !MoveUp.IsDown && !MoveDown.IsDown)
            {
                isInputDown = false;
            }


            actor.X += vx;
            actor.Y += vy;

            vx = Calc.Clamp(vx, -MaxVelocity.X, MaxVelocity.X);
            vy = Calc.Clamp(vy, -MaxVelocity.Y, MaxVelocity.Y);

            vx *= (1 - Friction);
            vy *= (1 - Friction);

            Velocity = new Vector2(vx, vy);

            if (!isInputDown && moving)
            {
                if(Calc.Abs(vx) < 0.5f) 
                {
                    Velocity = new Vector2(0f, Velocity.Y);
                }
                if(Calc.Abs(vy) < 0.5f) 
                {
                    Velocity = new Vector2(Velocity.X, 0f);
                }

                if (Velocity.X == 0f && Velocity.Y == 0f) 
                {
                    moving = false;
                    StoppedMoving?.Invoke();
                }
            }
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