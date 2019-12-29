using static VortexCore.SOKOL.sokol_gfx;

namespace VortexCore
{
    internal class SokolTexture : Texture2D
    {
        public sg_image sgImage {get; private set;}

        internal SokolTexture(sg_image sgImage)
        {
            this.sgImage = sgImage;
            IndexHandle = sgImage.id;
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