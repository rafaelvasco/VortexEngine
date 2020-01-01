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
using System.Collections.Generic;
using SysPath = System.IO.Path;

namespace VortexCore
{
    public static class Assets
    {
        private static Dictionary<string, Asset> loadedAssets;
        private static Dictionary<string, Asset> runtimeAssets;

        private static Loader loader;

        public static string Path { get; set; } = "Assets";

        internal static void Initialize()
        {
            loadedAssets = new Dictionary<string, Asset>();
            runtimeAssets = new Dictionary<string, Asset>();
            loader = new Loader();
        }

        public static T Get<T>(string assetId) where T : Asset
        {
            if(loadedAssets.TryGetValue(assetId, out var asset))
            {
                return (T)asset;
            }

            return null;
        }

        public static Texture2D LoadTexture(string relativePath)
        {
            var fullPath = BuildFullPath(relativePath);

            var id = SysPath.GetFileNameWithoutExtension(relativePath);

            if (loadedAssets.TryGetValue(id, out var asset))
            {
                return (Texture2D)asset;
            }

            var texture = loader.LoadTexture(fullPath);

            loadedAssets.Add(id, texture);

            return texture;

        }

        private static string BuildFullPath(string relativePath) 
        {
            return SysPath.Combine(AppDomain.CurrentDomain.BaseDirectory, Path, relativePath);
        }

        internal static void Free()
        {
            foreach(var asset in loadedAssets)
            {
                asset.Value.Dispose();
            }

            foreach(var asset in runtimeAssets)
            {
                asset.Value.Dispose();
            }

            loadedAssets.Clear();
            runtimeAssets.Clear();

        }


    }
}
