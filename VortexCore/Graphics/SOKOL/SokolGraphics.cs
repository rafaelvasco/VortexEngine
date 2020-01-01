/* Based on code from https://github.com/lithiumtoast/sokol-sharp/ */
/* and original example codes from https://github.com/floooh/sokol-samples/ */
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
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using static VortexCore.SOKOL.sokol_gfx;

namespace VortexCore
{
    internal unsafe partial class SokolGraphics : Graphics
    {
        public static readonly int MaxQuads = 16;

        private GraphicsInfo info;

        private sg_pipeline pipeline;

        private sg_bindings bindings;

        private sg_buffer vertexBuffer;

        private sg_buffer indexBuffer;

        private sg_shader shader;

        private sg_pass_action clearAction;

        private SokolVertex[] vertices;

        private int batchQuadCount = 0;

        private int accumQuadCount = 0;

        private Texture2D onePxTexture;

        private Texture2D currentTexture;

        private Matrix4x4 transform;

        private Matrix4x4 projection;

        private Matrix4x4 renderMatrix;

        private int renderWidth;

        private int renderHeight;

        private int drawCalls;

        private int maxDrawCalls;

        public GraphicsInfo Info => info;

        public int MaxDrawCalls => maxDrawCalls;

        internal SokolGraphics(IntPtr windowHandle)
        {
            InitializeGraphicsDevice(windowHandle);

            InitializeBufferBindings();

            InitializeBasePipeline();

            InitializeBaseStateAndResources();
        }

        private void InitializeGraphicsDevice(IntPtr windowHandle)
        {
            var graphicsDeviceDescr = new sg_desc();

            ImplInitialize(ref graphicsDeviceDescr, windowHandle);

            sg_setup(ref graphicsDeviceDescr);

            var driverInfo = sg_query_backend();

            this.info.Driver = driverInfo.ToString();
        }

        private void InitializeBufferBindings()
        {
            int maxVertices = MaxQuads * 4;
            vertices = new SokolVertex[maxVertices];

            var vertexBufferDesc = new sg_buffer_desc();
            vertexBufferDesc.usage = sg_usage.SG_USAGE_STREAM;
            vertexBufferDesc.type = sg_buffer_type.SG_BUFFERTYPE_VERTEXBUFFER;
            vertexBufferDesc.size = SokolVertex.SizeInBytes * vertices.Length;

            vertexBuffer = sg_make_buffer(ref vertexBufferDesc);

            var indicesLen = MaxQuads * 6;

            var indices = stackalloc ushort[MaxQuads * 6];

            ushort indice_i = 0;

            for (var i = 0; i < indicesLen; i += 6, indice_i += 4)
            {
                indices[i + 0] = (ushort)(indice_i + 0);
                indices[i + 1] = (ushort)(indice_i + 1);
                indices[i + 2] = (ushort)(indice_i + 2);
                indices[i + 3] = (ushort)(indice_i + 0);
                indices[i + 4] = (ushort)(indice_i + 2);
                indices[i + 5] = (ushort)(indice_i + 3);
            }

            var indexBufferDesc = new sg_buffer_desc();
            indexBufferDesc.usage = sg_usage.SG_USAGE_IMMUTABLE;
            indexBufferDesc.type = sg_buffer_type.SG_BUFFERTYPE_INDEXBUFFER;
            indexBufferDesc.content = indices;
            indexBufferDesc.size = Unsafe.SizeOf<ushort>() * indicesLen;

            indexBuffer = sg_make_buffer(ref indexBufferDesc);

            bindings.vertex_buffer(0) = vertexBuffer;
            bindings.index_buffer = indexBuffer;

        }

        private void InitializeBasePipeline()
        {
            var shaderDescr = new sg_shader_desc();
            shaderDescr.vs.uniformBlock(0).size = Unsafe.SizeOf<Matrix4x4>();

            ref var mvpUniform = ref shaderDescr.vs.uniformBlock(0).uniform(0);
            mvpUniform.name = (byte*)Marshal.StringToHGlobalAnsi("mvp");
            mvpUniform.type = sg_uniform_type.SG_UNIFORMTYPE_MAT4;

            shaderDescr.fs.image(0).type = sg_image_type.SG_IMAGETYPE_2D;

            shaderDescr.vs.source = (byte*)Marshal.StringToHGlobalAnsi(BaseVertexShaderSource);
            shaderDescr.fs.source = (byte*)Marshal.StringToHGlobalAnsi(BaseFragShaderSource);

            shader = sg_make_shader(ref shaderDescr);

            Marshal.FreeHGlobal((IntPtr)shaderDescr.vs.uniformBlock(0).uniform(0).name);
            Marshal.FreeHGlobal((IntPtr)shaderDescr.vs.source);
            Marshal.FreeHGlobal((IntPtr)shaderDescr.fs.source);

            var quadPipelineDescr = new sg_pipeline_desc();
            quadPipelineDescr.layout.attr(0).format = sg_vertex_format.SG_VERTEXFORMAT_FLOAT3;
            quadPipelineDescr.layout.attr(1).format = sg_vertex_format.SG_VERTEXFORMAT_FLOAT2;
            quadPipelineDescr.layout.attr(2).format = sg_vertex_format.SG_VERTEXFORMAT_FLOAT4;
            quadPipelineDescr.index_type = sg_index_type.SG_INDEXTYPE_UINT16;
            quadPipelineDescr.shader = shader;
            quadPipelineDescr.primitive_type = sg_primitive_type.SG_PRIMITIVETYPE_TRIANGLES;
            quadPipelineDescr.blend.enabled = true;
            quadPipelineDescr.blend.color_format = sg_pixel_format.SG_PIXELFORMAT_RGBA8;
            quadPipelineDescr.blend.src_factor_rgb = sg_blend_factor.SG_BLENDFACTOR_SRC_ALPHA;
            quadPipelineDescr.blend.dst_factor_rgb = sg_blend_factor.SG_BLENDFACTOR_ONE_MINUS_SRC_ALPHA;
            quadPipelineDescr.blend.src_factor_alpha = sg_blend_factor.SG_BLENDFACTOR_ONE;
            quadPipelineDescr.blend.dst_factor_alpha = sg_blend_factor.SG_BLENDFACTOR_ZERO;
            // quadPipelineDescr.depth_stencil.depth_compare_func = sg_compare_func.SG_COMPAREFUNC_LESS_EQUAL;
            // quadPipelineDescr.depth_stencil.depth_write_enabled = true;
            // quadPipelineDescr.rasterizer.cull_mode = sg_cull_mode.SG_CULLMODE_BACK;
            quadPipelineDescr.rasterizer.sample_count = 0;

            pipeline = sg_make_pipeline(ref quadPipelineDescr);

            if (pipeline.id == SG_INVALID_ID)
            {
                throw new Exception("Error while creating Metal Pipeline: Invalid Id");
            }

            clearAction = sg_pass_action.clear(Colors.Black);
        }

        private void InitializeBaseStateAndResources()
        {
            var (width, height) = ImplGetRenderSize();

            this.renderWidth = width;
            this.renderHeight = height;

            var pixmap = Pixmap.CreateFilled(1, 1, Colors.White);
            onePxTexture = (SokolTexture)ImplCreateTexture(pixmap);
            pixmap.Dispose();

            projection = Matrix4x4.CreateOrthographicOffCenter
            (
                0f,
                renderWidth,
                renderHeight,
                0f,
                0.0f,
                1.0f
            );

            transform = Matrix4x4.Identity;

            renderMatrix = projection * transform;

            SetCurrentTexture(onePxTexture);
        }

        void Graphics.Begin()
        {
            sg_begin_default_pass(ref clearAction, this.renderWidth, this.renderHeight);
            sg_apply_pipeline(pipeline);
            var mvpMatrix = Unsafe.AsPointer(ref renderMatrix);
            sg_apply_uniforms(sg_shader_stage.SG_SHADERSTAGE_VS, 0, mvpMatrix, Unsafe.SizeOf<Matrix4x4>());
        }

        void Graphics.End()
        {
            Flush();
            if (drawCalls > maxDrawCalls)
            {
                maxDrawCalls = drawCalls;
            }
            drawCalls = 0;
            accumQuadCount = 0;
        }



        void Graphics.DrawQuad(Texture2D texture, ref Quad quad)
        {
            if (accumQuadCount + 1 > MaxQuads) 
            {
                return;
            }
            
            if (currentTexture != null && currentTexture.IndexHandle != texture.IndexHandle)
            {
                Flush();
                SetCurrentTexture(texture);
            }

            float x1 = quad.X;
            float y1 = quad.Y;
            float x2 = quad.X + quad.Width;
            float y2 = quad.Y + quad.Height;

            float invTexWidth = 1.0f / texture.Width;
            float invTexHeight = 1.0f / texture.Height;

            float u = quad.SrcX * invTexWidth;
            float v = quad.SrcY * invTexHeight;
            float u2 = (quad.SrcX + quad.SrcWidth) * invTexWidth;
            float v2 = (quad.SrcY + quad.SrcHeight) * invTexHeight;

            if (quad.FlipH) 
            {
                Calc.Swap(ref u, ref u2);
            }

            if (quad.FlipV)
            {
                Calc.Swap(ref v, ref v2);
            }

            var col = quad.Color;

            fixed (SokolVertex* fixedVertexPointer = vertices)
            {
                var vertexPointer = fixedVertexPointer + (batchQuadCount++) * 4;

                vertexPointer[0].Position = new Vector3(x1, y1, 0f);
                vertexPointer[0].Uv = new Vector2(u, v);
                vertexPointer[0].Color = col;

                vertexPointer[1].Position = new Vector3(x2, y1, 0f);
                vertexPointer[1].Uv = new Vector2(u2, v);
                vertexPointer[1].Color = col;

                vertexPointer[2].Position = new Vector3(x2, y2, 0f);
                vertexPointer[2].Uv = new Vector2(u2, v2);
                vertexPointer[2].Color = col;

                vertexPointer[3].Position = new Vector3(x1, y2, 0f);
                vertexPointer[3].Uv = new Vector2(u, v2);
                vertexPointer[3].Color = col;
            }
            accumQuadCount++;
        }

        void Graphics.FillRect(float x, float y, float w, float h, Color color)
        {
            if (accumQuadCount + 1 > MaxQuads) 
            {
                return;
            }

            if (currentTexture != null && currentTexture.IndexHandle != onePxTexture.IndexHandle)
            {
                Flush();
                SetCurrentTexture(onePxTexture);
            }

            float x1 = x;
            float y1 = y;
            float x2 = x + w;
            float y2 = y + h;

            fixed (SokolVertex* fixedVertexPointer = vertices)
            {
                var vertexPointer = fixedVertexPointer + (batchQuadCount++) * 4;

                vertexPointer[0].Position = new Vector3(x1, y1, 0f);
                vertexPointer[0].Uv = new Vector2(0, 0);
                vertexPointer[0].Color = color;

                vertexPointer[1].Position = new Vector3(x2, y1, 0f);
                vertexPointer[1].Uv = new Vector2(1, 0);
                vertexPointer[1].Color = color;

                vertexPointer[2].Position = new Vector3(x2, y2, 0f);
                vertexPointer[2].Uv = new Vector2(1, 1);
                vertexPointer[2].Color = color;

                vertexPointer[3].Position = new Vector3(x1, y2, 0f);
                vertexPointer[3].Uv = new Vector2(0, 1);
                vertexPointer[3].Color = color;
            }

            accumQuadCount++;
        }

        private void SetCurrentTexture(Texture2D texture, int slot = 0)
        {
            bindings.fs_image(0) = texture.IndexHandle;
            currentTexture = texture;
        }

        private void Flush()
        {
            if (batchQuadCount > 0)
            {
                int bufferIndex = 0;
                fixed (SokolVertex* vertexPointer = vertices)
                {
                    bufferIndex = sg_append_buffer(vertexBuffer, vertexPointer, (batchQuadCount) * 4 * SokolVertex.SizeInBytes);
                    var info = sg_query_buffer_info(vertexBuffer);
                }

                bindings.vertex_buffer_offset(0) = bufferIndex;

                sg_apply_bindings(ref bindings);
                sg_draw(0, batchQuadCount * 6, 1);
                drawCalls++;
            }

             batchQuadCount = 0;
        }

        void Graphics.Present()
        {
            sg_end_pass();
            sg_commit();

        }

        Texture2D Graphics.CreateTexture(Pixmap pixmap)
        {
            return ImplCreateTexture(pixmap);
        }

        private Texture2D ImplCreateTexture(Pixmap pixmap)
        {
            var imageDescr = new sg_image_desc()
            {
                usage = sg_usage.SG_USAGE_IMMUTABLE,
                type = sg_image_type.SG_IMAGETYPE_2D,
                width = pixmap.Width,
                height = pixmap.Height,
                depth = 1,
                num_mipmaps = 1,
                min_filter = sg_filter.SG_FILTER_NEAREST,
                mag_filter = sg_filter.SG_FILTER_NEAREST,
                wrap_u = sg_wrap.SG_WRAP_REPEAT,
                wrap_v = sg_wrap.SG_WRAP_REPEAT,
                pixel_format = sg_pixel_format.SG_PIXELFORMAT_RGBA8
            };

            ref var subImage = ref imageDescr.content.subimage(0, 0);
            subImage.ptr = (void*)pixmap.DataPtr;
            subImage.size = pixmap.Width * pixmap.Height * pixmap.Pitch;

            var sokolImage = sg_make_image(ref imageDescr);

            var texture2d = new SokolTexture(sokolImage, pixmap.Width, pixmap.Height);

            return texture2d;
        }

        (int width, int height) Graphics.GetRenderSize()
        {
            return ImplGetRenderSize();
        }

        void Graphics.ReleaseResources()
        {
            sg_shutdown();

            ImplReleaseResources();
        }
    }
}