using System;
using System.Numerics;
using VortexCore.SOKOL;
using static VortexCore.SOKOL.sokol_gfx;

namespace VortexCore
{
    internal unsafe partial class SokolGraphics : Graphics
    {
        public static readonly int MaxVertexCount = 4;

        private GraphicsInfo info;

        private readonly SgDevice graphicsDevice;

        private readonly SgPipeline pipeline;

        private readonly SgBindings bindings = new SgBindings();

        private readonly SgBuffer vertexBuffer;

        private readonly SgBuffer indexBuffer;

        private SgShader shader;

        private sg_pass_action clearAction;

        private SokolVertex[] vertices;

        private int vertexIndex = 0;

        private Texture2D onePxTexture;

        private Texture2D currentTexture;

        public GraphicsInfo Info => info;

        internal SokolGraphics(IntPtr windowHandle, int width, int height)
        {
            var graphicsDeviceDescr = new SgDeviceDescription();

            ImplInitialize(ref graphicsDeviceDescr, windowHandle);

            graphicsDevice = new SgDevice(ref graphicsDeviceDescr);

            this.info.Driver = graphicsDevice.GraphicsBackend.ToString();

            vertices = new SokolVertex[MaxVertexCount];
            vertexBuffer = new SgBuffer<SokolVertex>(SgBufferType.Vertex, SgBufferUsage.Dynamic, vertices);

            var indices = new ushort[MaxVertexCount / 4 * 6];

            ushort indice_i = 0;

            for (var i = 0; i < indices.Length; i += 6, indice_i += 4)
            {
                indices[i + 0] = (ushort)(indice_i + 0);
                indices[i + 1] = (ushort)(indice_i + 1);
                indices[i + 2] = (ushort)(indice_i + 2);
                indices[i + 3] = (ushort)(indice_i + 0);
                indices[i + 4] = (ushort)(indice_i + 2);
                indices[i + 5] = (ushort)(indice_i + 3);
            }

            indexBuffer = new SgBuffer<ushort>(SgBufferType.Index, SgBufferUsage.Immutable, indices);

            bindings.SetVertexBuffer(0, vertexBuffer);
            bindings.SetIndexBuffer(indexBuffer);
            
            ImplInitializeShaders();

            pipeline = new SgPipeline(shader, new[]
            {
                sg_vertex_format.SG_VERTEXFORMAT_FLOAT3,
                sg_vertex_format.SG_VERTEXFORMAT_FLOAT2,
                sg_vertex_format.SG_VERTEXFORMAT_FLOAT4
            }, sg_index_type.SG_INDEXTYPE_UINT16);

            clearAction = new sg_pass_action();
            clearAction.GetColors()[0] = new sg_color_attachment_action()
            {
                action = sg_action.SG_ACTION_CLEAR,
                val = Color.Orange
            };

            var pixmap = Pixmap.CreateFilled(1, 1, Color.White);
            onePxTexture = (SokolTexture) ImplCreateTexture(pixmap);
            pixmap.Dispose();

            bindings.SetFragShaderImage(onePxTexture.IndexHandle);

        }

        void Graphics.Begin()
        {
            var (width, height) = ImplGetRenderSize();
            
            sg_begin_default_pass(ref clearAction, width, height);

            pipeline.Apply();
            bindings.Apply();
        }

        void Graphics.End()
        {
            Flush();
            sg_end_pass();
        }

        private void Flush() 
        {
            if(vertexIndex > 0) 
            {
                vertexIndex = 0;
                sg_draw(0, vertexIndex, 1);
            }
        }

        void Graphics.DrawQuad(Texture2D texture, ref Quad quad)
        {
            if (currentTexture.IndexHandle != texture.IndexHandle) 
            {
                Flush();
                bindings.SetFragShaderImage(texture.IndexHandle);
                currentTexture = texture;
            }

            int vtxIdx = vertexIndex;

            float x1 = quad.X;
            float y1 = quad.Y;
            float x2 = quad.X + quad.Width;
            float y2 = quad.Y + quad.Height;

            float invTexWidth = 1/texture.Width;
            float invTexHeight = 1/texture.Height;

            float u = quad.SrcX * invTexWidth;
            float v = quad.SrcY * invTexHeight;
            float u2 = (quad.SrcX + quad.SrcWidth) * invTexWidth;
            float v2 = (quad.SrcY + quad.SrcHeight) * invTexHeight;

            this.vertices[vtxIdx] = new SokolVertex
            (
                new Vector3(x1, y1, 0.0f),
                Color.White,
                new Vector2(u, v)    
            );
            this.vertices[vtxIdx+1] = new SokolVertex
            (
                new Vector3(x2, y1, 0.0f),
                Color.White,
                new Vector2(u2, v)    
            );
            this.vertices[vtxIdx+2] = new SokolVertex
            (
                new Vector3(x2, y2, 0.0f),
                Color.White,
                new Vector2(u2, v2)    
            );
            this.vertices[vtxIdx+3] = new SokolVertex
            (
                new Vector3(x1, y2, 0.0f),
                Color.White,
                new Vector2(u, v2)    
            );
            this.vertexIndex += 4;
        }

        void Graphics.FillRect(float x, float y, float w, float h, Color color)
        {

            int vtxIdx = vertexIndex;

            this.vertices[vtxIdx] = new SokolVertex
            (
                new Vector3(x, y, 0.0f),
                Color.White,
                new Vector2(0.0f, 0.0f)    
            );
            this.vertices[vtxIdx+1] = new SokolVertex
            (
                new Vector3(x+w, y, 0.0f),
                Color.White,
                new Vector2(0.0f, 1.0f)    
            );
            this.vertices[vtxIdx+2] = new SokolVertex
            (
                new Vector3(x+w, y+h, 0.0f),
                Color.White,
                new Vector2(1.0f, 1.0f)    
            );
            this.vertices[vtxIdx+3] = new SokolVertex
            (
                new Vector3(x, y+h, 0.0f),
                Color.White,
                new Vector2(0.0f, 1.0f)    
            );
            this.vertexIndex += 4;
        }

        void Graphics.Present()
        {
            sg_commit();
        }

        Texture2D Graphics.CreateTexture(Pixmap pixmap)
        {
            return ImplCreateTexture(pixmap);
        }

        private Texture2D ImplCreateTexture(Pixmap pixmap) 
        {
            var sokolTextureContent = new sg_image_content();
            sokolTextureContent.GetSubimage()[0].ptr = (void*)pixmap.DataPtr;
            sokolTextureContent.GetSubimage()[0].size = pixmap.Width * pixmap.Height * pixmap.Pitch;
            
            var sokolTexDescr = new sg_image_desc() 
            {
                width = pixmap.Width,
                height = pixmap.Height,
                content = sokolTextureContent
            };
            var sokolTexture = sg_make_image(ref sokolTexDescr);

            var texture2d = new SokolTexture(sokolTexture);

            return texture2d;
        }

        (int width, int height) Graphics.GetRenderSize()
        {
            return ImplGetRenderSize();
        }

        void Graphics.ReleaseResources()
        {
            ImplReleaseResources();
        }
    }
}