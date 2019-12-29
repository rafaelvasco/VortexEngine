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
