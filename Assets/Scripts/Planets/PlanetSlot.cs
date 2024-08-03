using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class PlanetSlot : MonoBehaviour
    {
        private Moveable occupyingMoveableObject;

        public bool IsSlotAvailable()
        {
            if (occupyingMoveableObject == null)
            {
                return true;
            }

            return false;
        }

        public Transform SetOccupyingObject(Moveable newOccupier)
        {
            if (!IsSlotAvailable())
            {
                return null;
            }

            occupyingMoveableObject = newOccupier;
            return transform;
        }

        public void ClearOccupyingObject()
        {
            occupyingMoveableObject = null;
        }
    }
}
