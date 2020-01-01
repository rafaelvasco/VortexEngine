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

using System;
using System.IO;

namespace VortexCore
{
    internal class Loader
    {
        private readonly STB.ImageStreamLoader imageLoader = new STB.ImageStreamLoader();
        //private readonly BinaryFormatter binarySerializer = new BinaryFormatter();

        //private T DeserializeObject<T>(string path)
        //{
        //    using var stream = File.OpenRead(path);
        //    var obj = binarySerializer.Deserialize(stream);
        //    return (T)obj;
        //}

        public Texture2D LoadTexture(string path)
        {
            var imageData = LoadImageData(path);

            return LoadTexture(imageData);
        }

        public Texture2D LoadTexture(ImageData imageData)
        {
            var pixmap = new Pixmap(imageData.Data, imageData.Width, imageData.Height);

            var texture = GamePlatform.Graphics.CreateTexture(pixmap);

            pixmap.Dispose();

            texture.Id = imageData.Id;

            return texture;
        }

        public ImageData LoadImageData(string path)
        {
            using var stream = File.OpenRead(path);

            var image = imageLoader.Load(stream, STB.ColorComponents.RedGreenBlueAlpha);

            var id = Path.GetFileNameWithoutExtension(path);

            var imageData = new ImageData()
            {
                Id = id,
                Data = image.Data,
                Width = image.Width,
                Height = image.Height
            };

            return imageData;
        }

    }
}
