using UnityEngine;

namespace Abraham.GalacticConquest
{
    public static class LayerMaskRefs
    {
        public static int planetLayer = 9;

        public static LayerMask GetLayerMask(int layer)
        {
            return 1 << layer;
        }
    }
}
