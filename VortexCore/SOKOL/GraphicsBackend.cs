/*
MIT License

Copyright (c) 2019 Lucas Girouard-Stranks

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
using System.Runtime.InteropServices;

namespace VortexCore.SOKOL
{
    public enum GraphicsBackend
    {
        Default,
        OpenGL, // Core 3.3
        OpenGLES2,
        OpenGLES3,
        Direct3D11,
        Metal
    }

    public static class GraphicsBackendHelper
    {
        public static GraphicsBackend GetDefaultPlatformGraphicsBackend()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // TODO: Use DirectX
                return GraphicsBackend.OpenGL;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return GraphicsBackend.Metal;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return GraphicsBackend.OpenGL;
            }

            throw new NotSupportedException("Unknown platform; could not decide on graphics backend.");
        }
    }
}