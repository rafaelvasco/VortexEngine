namespace VortexCore
{
    public class Animation : GameObject
    {
        public int DeltaTicks 
        {
            get => deltaTicks;
            set
            {
                if (deltaTicks != value)
                {
                    deltaTicks = value;
                    this.ticks = deltaTicks;
                }
            }
        }

        public override float Width => sourceRects[index].Width;

        public override float Height => sourceRects[index].Height;

        private Texture2D texture;
        private Rect[] sourceRects;
        private bool playing = true;
        private int length;
        private int index = 0;
        private int deltaTicks = 10;
        private int ticks = 10;

        public Animation(Spritesheet spritesheet)
        {
            this.texture = spritesheet.Texture;
            this.sourceRects = new Rect[spritesheet.Regions.Length];

            for (var i = 0; i < spritesheet.Regions.Length; ++i)
            {
                this.sourceRects[i] = spritesheet[i];
            }

            this.length = sourceRects.Length;
        }

        public Animation(Spritesheet spritesheet, params int[] indices)
        {
            this.texture = spritesheet.Texture;
            this.sourceRects = new Rect[indices.Length];

            for (var i = 0; i < indices.Length; ++i)
            {
                this.sourceRects[i] = spritesheet[indices[i]];
            }
           

            this.length = sourceRects.Length;
        }

        public Animation(Spritesheet spritesheet, string[] frameNames)
        {
            this.texture = spritesheet.Texture;
            this.sourceRects = new Rect[frameNames.Length];

            for(var i = 0; i < frameNames.Length; ++i)
            {
                this.sourceRects[i] = spritesheet[frameNames[i]];
            }
        }

        public void Play()
        {
            this.playing = true;

            this.index = 0;
        }

        public override void Draw(Graphics graphics, float parentX = 0, float parentY = 0)
        {
            if(Visible)
            {
                if (Visible)
                {
                    graphics.DrawTextureRegion(texture, parentX + X, parentY + Y, ref sourceRects[index]);
                }
            }
        }

        public override void Update()
        {
            if(playing && sourceRects.Length > 1)
            {
                ticks--;

                if (ticks < 0)
                {
                    index++;

                    if (index > length - 1)
                    {
                        index = 0;
                    }

                    ticks = DeltaTicks;
                }
            }
           
        }
    }
}

