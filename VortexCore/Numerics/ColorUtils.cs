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
    public static class ColorUtils
    {
        public static Color Blend(Color baseColor, Color overColor)
        {
            float sa = 1.0f - overColor.A;
            float alpha = baseColor.A * sa + overColor.A;

            if (alpha == 0)
            {
                return new Color(0, 0, 0, 0);
            }

            float baseA = baseColor.A;
            float overA = overColor.A;

            return new Color
            (
                (baseColor.R * baseA * sa + overColor.R * overA) / alpha,
                (baseColor.G * baseA * sa + overColor.G * overA) / alpha,
                (baseColor.B * baseA * sa + overColor.B * overA) / alpha,
                alpha
            );
        }
    
        public static Color Darkened(Color color, float amount)
        {
            float invAmount = 1.0f - amount;
            return new Color(
                color.R * invAmount,
                color.G * invAmount,
                color.B * invAmount,
                color.A
            );
        }

        public static Color Lightened(Color color, float amount)
        {
            float r = color.R;
            float g = color.G;
            float b = color.B;

            return new Color(
                r + (1.0f - r) * amount,
                g + (1.0f - g) * amount,
                b + (1.0f - b) * amount,
                1.0f
            );
        }

        public static Color Inverted(Color color)
        {
            return new Color(
                1.0f - color.R,
                1.0f - color.G,
                1.0f - color.B,
                color.A
            );
        }

        public static Color Lerp(Color colorSource, Color colorTarget, float t)
        {
            float r = colorSource.R;
            float g = colorSource.G;
            float b = colorSource.B;
            float a = colorSource.A;

            return new Color(
                r + (colorTarget.R - r) * t,
                g + (colorTarget.G - g) * t,
                b + (colorTarget.B - b) * t,
                a + (colorTarget.A - a) * t
            );
        }

        public static Color FromHSV(float hue, float saturation, float value, float alpha = 1.0f)
        {
            if (saturation == 0)
            {
                // acp_hromatic (grey)
                return new Color(value, value, value, alpha);
            }

            int i;
            float f, p, q, t;

            hue *= 6.0f;
            hue %= 6f;
            i = (int)hue;

            f = hue - i;
            p = value * (1 - saturation);
            q = value * (1 - saturation * f);
            t = value * (1 - saturation * (1 - f));

            return i switch
            {
                // Red is the dominant color
                0 => new Color(value, t, p, alpha),
                // Green is the dominant color
                1 => new Color(q, value, p, alpha),
                2 => new Color(p, value, t, alpha),
                // Blue is the dominant color
                3 => new Color(p, q, value, alpha),
                4 => new Color(t, p, value, alpha),
                // (5) Red is the dominant color
                _ => new Color(value, p, q, alpha),
            };
        }
    }
}
