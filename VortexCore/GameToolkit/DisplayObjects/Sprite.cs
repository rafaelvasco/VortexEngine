namespace VortexCore
{
    public class Sprite : GameObject
    {
        private Texture2D texture;
        private Quad quad;

        public Color Tint 
        {
            get => quad.Color;
            set => quad.Color = value;
            
        }

        public Sprite(Texture2D texture, Rect sourceRect = default)
        {
            this.texture = texture;
            var region = !sourceRect.IsEmpty ? sourceRect : new Rect(0, 0, texture.Width, texture.Height);
            this.quad = new Quad()
            {
                X = this.X,
                Y = this.Y,
                Width = region.Width,
                Height = region.Height,
                SrcX = region.X,
                SrcY = region.Y,
                SrcWidth = region.Width,
                SrcHeight = region.Height,
                Rotation = this.Rotation,
                FlipH = false,
                FlipV = false,
                Color = 0xFFFFFFFF
            };
        }

        public override float Width => quad.Width;

        public override float Height => quad.Height;

        public bool FlipHorizontal
        {
            get => quad.FlipH;
            set => quad.FlipH = value;

        }

        public bool FlipVertical
        {
            get => quad.FlipV;
            set => quad.FlipH = value;
        }
        public override float Rotation 
        {
            get => quad.Rotation;
            set => quad.Rotation = value;
        }
        public override float Opacity 
        {
            get => quad.Alpha;
            set => quad.Alpha = Calc.Clamp(value, 0.0f, 1.0f);
        }

        public override void Draw(Graphics graphics, float parentX = 0, float parentY = 0)
        {
            if (Visible)
            {
                quad.X = parentX + X;
                quad.Y = parentY + Y;
                quad.Rotation = this.Rotation;
                graphics.DrawQuad(texture, ref quad);
            }
        }

        public void SetRegion(Rect sourceRect)
        {
            quad.SrcX = sourceRect.X;
            quad.SrcY = sourceRect.Y;
            quad.SrcWidth = sourceRect.Width;
            quad.SrcHeight = sourceRect.Height;
            quad.Width = sourceRect.Width;
            quad.Height = sourceRect.Height;
        }

        public override void Update(float dt)
        {
        }
    }
}
