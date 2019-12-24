namespace VortexCore {
    public class Animation : Sprite {
        private Rect[] sourceRects;
        private bool playing = true;
        private int length;
        private int index = 0;

        private float frameDeltaSeconds = 1 / 10f;

        private float animTime = 0.0f;

        public float Fps {
            get => 1 / frameDeltaSeconds;
            set {
                frameDeltaSeconds = 1 / value;
            }
        }

        public override float Width => sourceRects[index].Width;

        public override float Height => sourceRects[index].Height;

        public Animation (Spritesheet spritesheet): base(spritesheet.Texture) {
            
            this.sourceRects = new Rect[spritesheet.Regions.Length];

            for (var i = 0; i < spritesheet.Regions.Length; ++i) {
                this.sourceRects[i] = spritesheet[i];
            }

            this.length = sourceRects.Length;

            this.SetRegion(this.sourceRects[0]);
        }

        public Animation (Spritesheet spritesheet, params int[] indices) : base(spritesheet.Texture) {
            this.sourceRects = new Rect[indices.Length];

            for (var i = 0; i < indices.Length; ++i) {
                this.sourceRects[i] = spritesheet[indices[i]];
            }

            this.length = sourceRects.Length;

            this.SetRegion(this.sourceRects[0]);
        }

        public Animation (Spritesheet spritesheet, string[] frameNames) : base(spritesheet.Texture) {
            this.sourceRects = new Rect[frameNames.Length];

            for (var i = 0; i < frameNames.Length; ++i) {
                this.sourceRects[i] = spritesheet[frameNames[i]];
            }

            this.SetRegion(this.sourceRects[0]);
        }

        public void Play () {
            this.playing = true;

            this.index = 0;
        }

        public override void Update (float dt) {
            if (playing && sourceRects.Length > 1) {

                animTime += dt;

                if (animTime > frameDeltaSeconds) {
                    index++;

                    if (index > length - 1) {
                        index = 0;
                    }

                    animTime = 0.0f;

                    this.SetRegion(this.sourceRects[index]);
                }
            }

        }
    }
}