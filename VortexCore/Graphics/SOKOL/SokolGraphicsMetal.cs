/* Based on code from https://github.com/lithiumtoast/sokol-sharp/ */
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

#if OSX

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using VortexCore.SOKOL;
using VortexCore.SOKOL.Metal;
using VortexCore.SOKOL.Metal.CoreAnimation;
using VortexCore.SOKOL.Metal.ObjCRuntime;
using static VortexCore.SDL;
using static VortexCore.SOKOL.sokol_gfx;

namespace VortexCore
{
    internal unsafe partial class SokolGraphics : Graphics
    {
        private static readonly string BaseVertexShaderSource = @"

            #include <metal_stdlib>
            using namespace metal;
            struct params_t {
                float4x4 mvp;
            };
            struct vs_in {
                float4 position [[attribute(0)]];
                float2 uv [[attribute(1)]];
                float4 color [[attribute(2)]];
                
            };
            struct vs_out {
                float4 position [[position]];
                float2 uv;
                float4 color;
            };
            vertex vs_out _main(vs_in in [[stage_in]], constant params_t& params [[buffer(0)]]) {
                vs_out out;
                out.position = params.mvp * in.position;
                out.uv = in.uv;
                out.color = in.color;
                return out;
            }
        ";

        private static readonly string BaseFragShaderSource = @"

            #include <metal_stdlib>
            #include <simd/simd.h>
            using namespace metal;
            struct fs_in {
                float2 uv;
                float4 color;
            };
            fragment float4 _main(fs_in in [[stage_in]], texture2d<float> tex [[texture(0)]], sampler smp [[sampler(0)]]) {
                return tex.sample(smp, in.uv) * in.color;
            };
        ";

        private static SokolGraphics instance;

        private CAMetalDrawable drawable;
        private MTLRenderPassDescriptor renderPassDescriptor;
        private CAMetalLayer metalLayer;

        private GCHandle getMetalRenderPassDescriptorGCHandle;
        private GCHandle getMetalDrawableGCHandle;




        bool Graphics.VSyncEnabled 
        { 
            get => metalLayer.displaySyncEnabled; 
            set => metalLayer.displaySyncEnabled = value; 
        }

        


        private void ImplInitialize(ref sg_desc deviceDescription, IntPtr windowHandle) 
        {
            instance = this;

            SDL_SetHint("SDL_HINT_RENDER_DRIVER", "metal");
            const SDL_RendererFlags sdlRendererFlags = SDL_RendererFlags.SDL_RENDERER_ACCELERATED;
            var sdlRendererHandle = SDL_CreateRenderer(windowHandle, -1, sdlRendererFlags);
            var metalLayerHandle = SDL_RenderGetMetalLayer(sdlRendererHandle);
            SDL_DestroyRenderer(sdlRendererHandle);

            if (metalLayerHandle == IntPtr.Zero)
            {
                throw new InvalidOperationException("Could not initialize Metal Renderer: CAMetalLayer is null");
            }

            metalLayer = new CAMetalLayer(metalLayerHandle)
            {
                framebufferOnly = true,
                displaySyncEnabled = true
            };

            var getMetalRenderPassDescriptor = new GetPointerDelegate(StaticGetMetalRenderPassDescriptor);
            var getMetalDrawable = new GetPointerDelegate(StaticGetMetalDrawable);
            
            getMetalRenderPassDescriptorGCHandle = GCHandle.Alloc(getMetalRenderPassDescriptor);
            getMetalDrawableGCHandle = GCHandle.Alloc(getMetalDrawable);

            deviceDescription.mtl_device = (void*) metalLayer.device.Handle;
            deviceDescription.mtl_renderpass_descriptor_cb = (void*) Marshal.GetFunctionPointerForDelegate(getMetalRenderPassDescriptor);
            deviceDescription.mtl_drawable_cb = (void*) Marshal.GetFunctionPointerForDelegate(getMetalDrawable);

            //deviceDescription.GraphicsBackend = SOKOL.GraphicsBackend.Metal;

            NativeLibrary.SetDllImportResolver(typeof(sokol_gfx).Assembly, ResolveLibrary);

            var limits = sg_query_limits();
            this.info = new GraphicsInfo()
            {
                Name = "SokolGraphicsMetal",
                MaxTextureWidth = (int) limits.max_image_size_2d,
                MaxTextureHeight = (int) limits.max_image_size_2d
            };
        }

        

        

        private (int width, int height) ImplGetRenderSize() 
        {
            var drawableSize = metalLayer.drawableSize;
            var width = (int)drawableSize.width;
            var height = (int)drawableSize.height;
            return (width, height);
        }

        private void ImplReleaseResources() 
        {
            if (drawable.Handle != IntPtr.Zero)
            {
                NSObject.release(drawable);
            }

            if (metalLayer.Handle != IntPtr.Zero)
            {
                NSObject.release(metalLayer);
            }

            if (getMetalRenderPassDescriptorGCHandle.IsAllocated)
            {
                getMetalRenderPassDescriptorGCHandle.Free();
            }

            if (getMetalDrawableGCHandle.IsAllocated)
            {
                getMetalDrawableGCHandle.Free();
            }
        }

        private static IntPtr ResolveLibrary(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            libraryName = libraryName.ToLower() switch
            {
                "sokol_gfx" => "sokol_gfx",
                _ => libraryName
            };

            return NativeLibrary.Load(libraryName, assembly, searchPath);
        }

        private static IntPtr StaticGetMetalRenderPassDescriptor()
        {
            // This callback is invoked by sokol_gfx native library in `sg_begin_default_pass()`
            return instance.GetMetalRenderPassDescriptor();
        }
        
        private static IntPtr StaticGetMetalDrawable()
        {
            // This callback is invoked by sokol_gfx native library in `sg_end_pass()` for the default pass
            return instance.drawable;
        }

        private IntPtr GetMetalRenderPassDescriptor()
        {
            if (drawable.Handle != IntPtr.Zero)
            {
                NSObject.release(drawable);
            }
            drawable = metalLayer.nextDrawable();
            
            if (renderPassDescriptor != IntPtr.Zero)
            {
                NSObject.release(renderPassDescriptor);
            }
            renderPassDescriptor = MTLRenderPassDescriptor.New();
            
            var colorAttachment = renderPassDescriptor.colorAttachments[0];
            colorAttachment.texture = drawable.texture;

            return renderPassDescriptor; 
        }
    }
}

#endif