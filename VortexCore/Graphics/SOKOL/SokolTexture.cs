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

using static VortexCore.SOKOL.sokol_gfx;

namespace VortexCore
{
    internal class SokolTexture : Texture2D
    {
        public sg_image sgImage {get; private set;}

        internal SokolTexture(sg_image sgImage, int width, int height)
        {
            this.sgImage = sgImage;
            IndexHandle = sgImage.id;
            Width = width;
            Height = height;
    
        }

        internal override void Dispose()
        {
            if(sg_query_image_state(sgImage) == sg_resource_state.SG_RESOURCESTATE_VALID) 
            {
                sg_destroy_image(sgImage);
            }
        }
    }
}