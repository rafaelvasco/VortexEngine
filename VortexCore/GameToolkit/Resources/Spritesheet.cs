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
