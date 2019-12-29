namespace VortexCore
{
    public class Interactive : Behavior
    {
        public event BehaviorEventFunc OnClick;
        public event BehaviorEventFunc OnMouseDown;
        public event BehaviorEventFunc OnMouseOver;
        public event BehaviorEventFunc OnMouseLeave;

        public bool Draggable {get; set;}

        public bool Hovered {get; private set;}
        public bool Active {get; private set;}

        private bool dragging = false;

        private float lastDragPosX;

        private float lastDragPosY;


        public Interactive(bool activeOnStart) : base(activeOnStart)
        {
        }

        public override void Update(float dt)
        {
            if (!Enabled) 
            {
                return;
            }

            if(!Hovered && AttachedActor.BoundingRect.Contains(Input.MouseX, Input.MouseY)) 
            {
                Hovered = true;
                OnMouseOver?.Invoke();
            }
            else if (Hovered && !AttachedActor.BoundingRect.Contains(Input.MouseX, Input.MouseY)) 
            {
                Hovered = false;
                OnMouseLeave?.Invoke();
                
            }

            if (Hovered) 
            {
                if (Input.MousePressed(MouseButton.Left)) 
                {
                    OnMouseDown?.Invoke();
                    Active = true;
                }
                if (Input.MouseReleased(MouseButton.Left)) 
                {
                    OnClick?.Invoke();
                }
            }

            if (Active && Input.MouseReleased(MouseButton.Left)) 
            {
                Active = false;
            }

            // Dragging

            if(Draggable && Hovered && Active && !dragging) 
            {
                dragging = true;
                lastDragPosX = Input.MouseX;
                lastDragPosY = Input.MouseY; 
            }

            if(dragging) 
            {
                var mouseX = Input.MouseX;
                var mouseY = Input.MouseY;

                var dx = mouseX - lastDragPosX;
                var dy = mouseY - lastDragPosY;

                AttachedActor.X += dx;
                AttachedActor.Y += dy;

                lastDragPosX = mouseX;
                lastDragPosY = mouseY;
            }

            if(dragging && Input.MouseReleased(MouseButton.Left)) 
            {
                dragging = false;
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