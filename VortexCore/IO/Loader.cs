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

            var texture = GamePlatform.Graphics.CreateTexture(pixmap.DataPtr, pixmap.Width, pixmap.Height);

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
