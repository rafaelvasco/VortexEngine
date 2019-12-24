namespace VortexCore
{
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