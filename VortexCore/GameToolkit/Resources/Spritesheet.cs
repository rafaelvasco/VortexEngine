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

using System.Collections.Generic;

namespace VortexCore
{
    public class Spritesheet
    {
        public Texture2D Texture { get; private set; }

        public Rect[] Regions { get; private set; }

        public Dictionary<string, int> Atlas { get; private set; }

        public Spritesheet(Texture2D texture, Rect[] regions)
        {
            this.Texture = texture;
            this.Regions = regions;

            this.Atlas = new Dictionary<string, int>();

            for(var i = 0; i < regions.Length; ++i)
            {
                this.Atlas.Add("Frame" + i, i);
            }
        }

        public static Spritesheet FromGrid(Texture2D texture, int lines, int cols, int removeLast = -1)
        {
            var count = lines * cols;

            if (removeLast != -1)
            {
                count -= removeLast;
            }

            var sourceRects = new Rect[count];

            var cellWidth = texture.Width / cols;
            var cellHeight = texture.Height / lines;

            for (var i = 0; i < count; ++i)
            {
                sourceRects[i] = new Rect(
                    (i % cols) * cellWidth,
                    (i / cols) * cellHeight,
                    cellWidth,
                    cellHeight
                );
            }

            return new Spritesheet(texture, sourceRects);
        }

        public void SetFrameName(string name, int regionIndex)
        {
            this.Atlas[name] = regionIndex;
        }

        public Rect this[int index]
        {
            get => Regions[index];
        }

        public Rect this[string name]
        {
            get
            {
                if(Atlas.TryGetValue(name, out var index))
                {
                    return Regions[index];
                }

                return Rect.Empty;
            }
        }
    }
}
