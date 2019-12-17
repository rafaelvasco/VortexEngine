namespace VortexCore
{
    public class Sprite : GameObject
    {
        private Texture2D texture;
        private Rect sourceRect;

        public Sprite(Texture2D texture, Rect sourceRect = default)
        {
            this.texture = texture;
            this.sourceRect = !sourceRect.IsEmpty ? sourceRect : new Rect(0, 0, texture.Width, texture.Height);
        }

        public override float Width => sourceRect.Width;

        public override float Height => sourceRect.Height;

        public override void Draw(Graphics graphics, float parentX = 0, float parentY = 0)
        {
            if(Visible)
            {
                graphics.DrawTextureRegion(texture, parentX + X, parentY + Y, ref sourceRect);
            }
            
        }

        public void SetRegion(Rect sourceRect)
        {
            this.sourceRect = sourceRect;
        }

        public override void Update()
        {
        }
    }
}
