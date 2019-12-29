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
using static VortexCore.SOKOL.sokol_gfx;

namespace VortexCore.SOKOL
{
    public sealed class SgBindings
    {
        private sg_bindings _bindings;

        public SgBindings()
        {
        }

        public void SetFragShaderImage(uint sokolImageHandle, int slot = 0) 
        {
            unsafe 
            {
                _bindings.fs_images[slot] = sokolImageHandle;
            }
        }

        public void SetVertexBuffer(int bufferIndex, SgBuffer buffer, int bufferOffset = 0)
        {
            if (bufferIndex > SG_MAX_SHADERSTAGE_BUFFERS)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferIndex), bufferIndex, null);
            }
            
            if (buffer.Handle.id == 0)
            {
                throw new ArgumentException(nameof(buffer));
            }

            unsafe
            {
                _bindings.vertex_buffers[bufferIndex] = buffer.Handle;
                _bindings.vertex_buffer_offsets[bufferIndex] = bufferOffset;   
            }
        }
        
        
        public void SetVertexBufferOffset(int bufferIndex, int bufferOffset)
        {
            if (bufferIndex > SG_MAX_SHADERSTAGE_BUFFERS)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferIndex), bufferIndex, null);
            }

            unsafe
            {
                _bindings.vertex_buffer_offsets[bufferIndex] = bufferOffset;   
            }
        }

        public void SetIndexBuffer(SgBuffer buffer)
        {
            if (buffer.Handle.id == 0)
            {
                throw new ArgumentException(nameof(buffer));
            }
            
            _bindings.index_buffer = buffer.Handle;
            _bindings.index_buffer_offset = 0;
        }
        
        public void SetIndexBufferOffset(int bufferOffset)
        {
            _bindings.index_buffer_offset = bufferOffset;
        }

        public void Apply()
        {
            sg_apply_bindings(ref _bindings);
        }
    }
}