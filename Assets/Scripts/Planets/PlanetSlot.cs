using Abraham.GalacticConquest.UnitControl;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    [System.Serializable]
    public class PlanetSlot
    {
        public Transform slotTransform;
        public Moveable occupyingMoveable;

        public PlanetSlot(Transform newSlotTransform, Moveable newOccupyingMovable)
        {
            slotTransform = newSlotTransform;
            occupyingMoveable = newOccupyingMovable;
        }
    }
}
