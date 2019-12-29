// Original code derived from:
// (1) https://github.com/mellinoe/veldrid/blob/master/src/Veldrid.MetalBindings/CAMetalLayer.cs

/*
The MIT License (MIT)

Copyright (c) 2017 Eric Mellino and Veldrid contributors

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
using VortexCore.SOKOL.Metal.ObjCRuntime;
using VortexCore.SOKOL.Metal.CoreGraphics;
using static VortexCore.SOKOL.Metal.ObjCRuntime.Messaging;


namespace VortexCore.SOKOL.Metal.CoreAnimation
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CAMetalLayer
    {
        public readonly IntPtr Handle;

        public CAMetalLayer(IntPtr handle)
        {
            Handle = handle;
        }
        
        public MTLDevice device => t_objc_msgSend<MTLDevice>(Handle, sel_device);

        public BlittableBoolean framebufferOnly
        {
            get => bool_objc_msgSend(Handle, sel_framebufferOnly);
            set => void_objc_msgSend_bool(Handle, sel_setFramebufferOnly, value);
        }
        
        public CGSize drawableSize => cgsize_objc_msgSend(Handle, sel_drawableSize);

        public BlittableBoolean displaySyncEnabled
        {
            get => bool_objc_msgSend(Handle, sel_displaySyncEnabled);
            set => void_objc_msgSend_bool(Handle, sel_setDisplaySyncEnabled, value);
        }

        public CAMetalDrawable nextDrawable()
        {
            return t_objc_msgSend<CAMetalDrawable>(Handle, sel_nextDrawable);
        }
        
        public static implicit operator IntPtr(CAMetalLayer value)
        {
            return value.Handle;
        }
        
        private static readonly Selector sel_device = "device";
        private static readonly Selector sel_framebufferOnly = "framebufferOnly";
        private static readonly Selector sel_setFramebufferOnly = "setFramebufferOnly:";
        private static readonly Selector sel_drawableSize = "drawableSize";
        private static readonly Selector sel_displaySyncEnabled = "displaySyncEnabled";
        private static readonly Selector sel_setDisplaySyncEnabled = "setDisplaySyncEnabled:";
        private static readonly Selector sel_nextDrawable = "nextDrawable";
    }
}