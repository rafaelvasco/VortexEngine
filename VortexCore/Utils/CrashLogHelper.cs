using System;
using System.IO;

namespace VortexCore
{
    public static class CrashLogHelper
    {
        public static void LogToFile(Exception e, Game game) 
        {
            using (var fs = File.CreateText("crashlog.txt"))
            {
                fs.WriteLine(e.ToString());
                fs.WriteLine();
                fs.WriteLine(GamePlatform.RuntimePlatform);
                fs.WriteLine(GamePlatform.GraphicsBackend);
                fs.WriteLine($"Resolution: {game.DisplaySize}");
            }
        }
    }
}