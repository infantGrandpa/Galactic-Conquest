using UnityEngine;

namespace Abraham.GalacticConquest.Refs
{
    public static class LayerMaskRefs
    {
        public const int PlanetLayer = 9;

        public static LayerMask GetLayerMask(int layer)
        {
            return 1 << layer;
        }
    }
}
