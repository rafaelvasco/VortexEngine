using System.Runtime.InteropServices;

namespace VortexCore.GLEW
{
    public static class Glew
    {
        private const string GlewLibrary = "glew32";

        [DllImport(GlewLibrary, EntryPoint = "glewInit")]
        public static extern int GlewInit();
    }
}
