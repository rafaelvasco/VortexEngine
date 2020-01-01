/* Adapted from https://github.com/lithiumtoast/sokol-sharp/blob/develop/src/Sokol.Graphics/RgbaFloat.cs */
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
    public static class Colors
    {
        // Custom colors
        public static readonly Color TransparentBlack = 0x00000000;
        public static readonly Color Transparent = 0x00000000;
        public static readonly Color MonoGameOrange = 0xE73C00FF;

        // Basic colors: https://www.w3.org/TR/css-color-3/#html4
        public static readonly Color Black = 0x000000FF;
        public static readonly Color Silver = 0xC0C0C0FF;
        public static readonly Color Gray = 0x808080FF;
        public static readonly Color White = 0xFFFFFFFF;
        public static readonly Color Maroon = 0x800000FF;
        public static readonly Color Red = 0xFF0000FF;
        public static readonly Color Purple = 0x800080FF;
        public static readonly Color Fuchsia = 0xFF00FFFF;
        public static readonly Color Green = 0x008000FF;
        public static readonly Color Lime = 0x00FF00FF;
        public static readonly Color Olive = 0x808000FF;
        public static readonly Color Yellow = 0xFFFF00FF;
        public static readonly Color Navy = 0x000080FF;
        public static readonly Color Blue = 0x0000FFFF;
        public static readonly Color Teal = 0x008080FF;
        public static readonly Color Aqua = 0x00FFFFFF;
        
        // Extended colors: https://www.w3.org/TR/css-color-3/#svg-color
        public static readonly Color AliceBlue = 0xF0F8FFFF;
        public static readonly Color AntiqueWhite = 0xFAEBD7FF;
        public static readonly Color Aquamarine = 0x7FFFD4FF;
        public static readonly Color Azure = 0xF0FFFFFF;
        public static readonly Color Beige = 0xF5F5DCFF;
        public static readonly Color Bisque = 0xFFE4C4FF;
        public static readonly Color BlanchedAlmond = 0xFFEBCDFF;
        public static readonly Color BlueViolet = 0x8A2BE2FF;
        public static readonly Color Brown = 0xA52A2AFF;
        public static readonly Color BurlyWood = 0xDEB887FF;
        public static readonly Color CadetBlue = 0x5F9EA0FF;
        public static readonly Color Chartreuse = 0x7FFF00FF;
        public static readonly Color Chocolate = 0xD2691EFF;
        public static readonly Color Coral = 0xFF7F50FF;
        public static readonly Color CornflowerBlue = 0x6495EDFF;
        public static readonly Color Cornsilk = 0xFFF8DCFF;
        public static readonly Color Crimson = 0xDC143CFF;
        public static readonly Color Cyan = 0x00FFFFFF;
        public static readonly Color DarkBlue = 0x00008BFF;
        public static readonly Color DarkCyan = 0x008B8BFF;
        public static readonly Color DarkGoldenrod = 0xB8860BFF;
        public static readonly Color DarkGray = 0xA9A9A9FF;
        public static readonly Color DarkGreen = 0x006400FF;
        public static readonly Color DarkGrey = 0xA9A9A9FF;
        public static readonly Color DarkKhaki = 0xBDB76BFF;
        public static readonly Color DarkMagenta = 0x8B008BFF;
        public static readonly Color DarkOliveGreen = 0x556B2FFF;
        public static readonly Color DarkOrange = 0xFF8C00FF;
        public static readonly Color DarkOrchid = 0x9932CCFF;
        public static readonly Color DarkRed = 0x8B0000FF;
        public static readonly Color DarkSalmon = 0xE9967AFF;
        public static readonly Color DarkSeaGreen = 0x8FBC8FFF;
        public static readonly Color DarkStateBlue = 0x483D8BFF;
        public static readonly Color DarkStateGray = 0x2F4F4FFF;
        public static readonly Color DarkStateGrey = 0x2F4F4FFF;
        public static readonly Color DarkTurquoise = 0x00CED1FF;
        public static readonly Color DarkViolet = 0x9400D3FF;
        public static readonly Color DeepPink = 0xFF1493FF;
        public static readonly Color DeepSkyBlue = 0x00BFFFFF;
        public static readonly Color DimGray = 0x696969FF;
        public static readonly Color DimGrey = 0x696969FF;
        public static readonly Color DodgerBlue = 0x1E90FFFF;
        public static readonly Color Firebrick = 0xB22222FF;
        public static readonly Color FloralWhite = 0xFFFAF0FF;
        public static readonly Color ForestGreen = 0x228B22FF;
        public static readonly Color Gainsboro = 0xDCDCDCFF;
        public static readonly Color GhostWhite = 0xF8F8FFFF;
        public static readonly Color Gold = 0xFFD700FF;
        public static readonly Color Goldenrod = 0xDAA520FF;
        public static readonly Color GreenYellow = 0xADFF2FFF;
        public static readonly Color Grey = 0x808080FF;
        public static readonly Color Honeydew = 0xF0FFF0FF;
        public static readonly Color HotPink = 0xFF69B4FF;
        public static readonly Color IndianRed = 0xCD5C5CFF;
        public static readonly Color Indigo = 0x4B0082FF;
        public static readonly Color Ivory = 0xFFFFF0FF;
        public static readonly Color Khaki = 0xF0E68CFF;
        public static readonly Color Lavender = 0xE6E6FAFF;
        public static readonly Color LavenderBlush = 0xFFF0F5FF;
        public static readonly Color LawnGreen = 0x7CFC00FF;
        public static readonly Color LemonChiffon = 0xFFFACDFF;
        public static readonly Color LightBlue = 0xADD8E6FF;
        public static readonly Color LightCoral = 0xF08080FF;
        public static readonly Color LightCyan = 0xE0FFFFFF;
        public static readonly Color LightGoldenrodYellow = 0xFAFAD2FF;
        public static readonly Color LightGray = 0xD3D3D3FF;
        public static readonly Color LightGreen = 0x90EE90FF;
        public static readonly Color LightGrey = 0xD3D3D3FF;
        public static readonly Color LightPink = 0xFFB6C1FF;
        public static readonly Color LightSalmon = 0xFFA07AFF;
        public static readonly Color LightSeaGreen = 0x20B2AAFF;
        public static readonly Color LightSkyBlue = 0x87CEFAFF;
        public static readonly Color LightSlateGray = 0x778899FF;
        public static readonly Color LightSlateGrey = 0x778899FF;
        public static readonly Color LightSteelBlue = 0xB0C4DEFF;
        public static readonly Color LightYellow = 0xFFFFE0FF;
        public static readonly Color LimeGreen = 0x32CD32FF;
        public static readonly Color Linen = 0xFAF0E6FF;
        public static readonly Color Magenta = 0xFF00FFFF;
        public static readonly Color MediumAquamarine = 0x66CDAAFF;
        public static readonly Color MediumBlue = 0x0000CDFF;
        public static readonly Color MediumOrchid = 0xBA55D3FF;
        public static readonly Color MediumPurple = 0x9370DBFF;
        public static readonly Color MediumSeaGreen = 0x3CB371FF;
        public static readonly Color MediumStateBlue = 0x7B68EEFF;
        public static readonly Color MediumSpringGreen = 0x00FA9AFF;
        public static readonly Color MediumTurquoise = 0x48D1CCFF;
        public static readonly Color MediumVioletRed = 0xC71585FF;
        public static readonly Color MidnightBlue = 0x191970FF;
        public static readonly Color MintCream = 0xF5FFFAFF;
        public static readonly Color MistyRose = 0xFFE4E1FF;
        public static readonly Color Moccasin = 0xFFE4B5FF;
        public static readonly Color NavajoWhite = 0xFFDEADFF;
        public static readonly Color OldLace = 0xFDF5E6FF;
        public static readonly Color OliveDrab = 0x6B8E23FF;
        public static readonly Color Orange = 0xFFA500FF;
        public static readonly Color Orangered = 0xFF4500FF;
        public static readonly Color Orchid = 0xDA70D6FF;
        public static readonly Color PaleGoldenrod = 0xEEE8AAFF;
        public static readonly Color PaleGreen = 0x98FB98FF;
        public static readonly Color PaleTurquoise = 0xAFEEEEFF;
        public static readonly Color PaleVioletRed = 0xDB7093FF;
        public static readonly Color PapayaWhip = 0xFFEFD5FF;
        public static readonly Color PeachPuff = 0xFFDAB9FF;
        public static readonly Color Peru = 0xCD853FFF;
        public static readonly Color Pink = 0xFFC0CBFF;
        public static readonly Color Plum = 0xDDA0DDFF;
        public static readonly Color PowderBlue = 0xB0E0E6FF;
        public static readonly Color RosyBrown = 0xBC8F8FFF;
        public static readonly Color RoyalBlue = 0x4169E1FF;
        public static readonly Color SaddleBrown = 0x8B4513FF;
        public static readonly Color Salmon = 0xFA8072FF;
        public static readonly Color SandyBrown = 0xF4A460FF;
        public static readonly Color SeaGreen = 0x2E8B57FF;
        public static readonly Color SeaShell = 0xFFF5EEFF;
        public static readonly Color Sienna = 0xA0522DFF;
        public static readonly Color SkyBlue = 0x87CEEBFF;
        public static readonly Color SlateBlue = 0x6A5ACDFF;
        public static readonly Color SlateGray = 0x708090FF;
        public static readonly Color SlateGrey = 0x708090FF;
        public static readonly Color Snow = 0xFFFAFAFF;
        public static readonly Color SpringGreen = 0x00FF7FFF;
        public static readonly Color SteelBlue = 0x4682B4FF;
        public static readonly Color Tan = 0xD2B48CFF;
        // public static readonly Color Teal = 0x008080FF;
        public static readonly Color Thistle = 0xD8BFD8FF;
        public static readonly Color Tomato = 0xFF6347FF;
        public static readonly Color Turquoise = 0x40E0D0FF;
        public static readonly Color Violet = 0xEE82EEFF;
        public static readonly Color Wheat = 0xF5DEB3FF;
        public static readonly Color Whitesmoke = 0xF5F5F5FF;
        public static readonly Color YellowGreen = 0x9ACD32FF;

        public static readonly Color EgyptianBlue = 0x1034A6FF;
    }
}