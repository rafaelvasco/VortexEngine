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