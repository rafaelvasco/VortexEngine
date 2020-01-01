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

namespace VortexCore
{
    public delegate void ChangeDirectionFunc(FourWayDirection oldDirection, FourWayDirection newDirection);

    public delegate void BehaviorEventFunc();

    public abstract class Behavior
    {
        private bool enabled;

        internal void AttachTo(Actor actor) {
            this.AttachedActor = actor;
            OnAttached();
        }

        protected Behavior(bool activeOnStart) {
            enabled = activeOnStart;
        }

        public Actor AttachedActor { get; private set; }

        public bool Enabled
        {
            get => enabled;
            set
            {
                if (enabled != value)
                {
                    enabled = value;

                    if (enabled)
                    {
                        OnEnabled();
                    }
                    else 
                    {
                        OnDisabled();
                    }
                }
            }
        }

        protected abstract void OnEnabled();

        protected abstract void OnDisabled();

        protected abstract void OnAttached();

        protected abstract void OnDetached();

        public abstract void Update(float dt);

        public override string ToString()
        {
            return $"{GetType().Name}, {AttachedActor.Name}";
        }
    }
}