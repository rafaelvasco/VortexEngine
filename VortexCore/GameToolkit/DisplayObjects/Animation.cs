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
    public class Animation : Sprite
    {
        public int FrameIndex 
        {
            get => index;
            set 
            {
                index = Calc.Clamp(value, 0, sourceRects.Length-1);
                this.SetRegion(this.sourceRects[index]);
            }
        }

        public bool Playing 
        {
            get => playing;
            set => playing = value;
        }

        public float Fps
        {
            get => 1 / frameDeltaSeconds;
            set
            {
                frameDeltaSeconds = 1 / value;
            }
        }

        public override float Width => sourceRects[index].Width;

        public override float Height => sourceRects[index].Height;

        private Rect[] sourceRects;
        private bool playing = true;
        private int index;
        private float frameDeltaSeconds = 1 / 10f;
        private float animTime;

        public Animation(Spritesheet spritesheet) : base(spritesheet.Texture)
        {

            this.sourceRects = new Rect[spritesheet.Regions.Length];

            for (var i = 0; i < spritesheet.Regions.Length; ++i)
            {
                this.sourceRects[i] = spritesheet[i];
            }

            FrameIndex = 0;
            
        }

        public Animation(Spritesheet spritesheet, params int[] indices) : base(spritesheet.Texture)
        {
            this.sourceRects = new Rect[indices.Length];

            for (var i = 0; i < indices.Length; ++i)
            {
                this.sourceRects[i] = spritesheet[indices[i]];
            }

            this.SetRegion(this.sourceRects[0]);
        }

        public Animation(Spritesheet spritesheet, string[] frameNames) : base(spritesheet.Texture)
        {
            this.sourceRects = new Rect[frameNames.Length];

            for (var i = 0; i < frameNames.Length; ++i)
            {
                this.sourceRects[i] = spritesheet[frameNames[i]];
            }

            this.SetRegion(this.sourceRects[0]);
        }

        public override void Update(float dt)
        {
            if (playing && sourceRects.Length > 1)
            {

                animTime += dt;

                if (animTime > frameDeltaSeconds)
                {
                    index++;

                    if (index > sourceRects.Length - 1)
                    {
                        index = 0;
                    }

                    animTime = 0.0f;

                    this.SetRegion(this.sourceRects[index]);
                }
            }

        }
    }
}