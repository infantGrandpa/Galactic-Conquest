using UnityEngine;
using System.Collections.Generic;
using Abraham.GalacticConquest.UnitControl;
using Sirenix.OdinInspector;

namespace Abraham.GalacticConquest
{
    public class PlanetSlotHandler : MonoBehaviour
    {
        public List<Transform> planetSlotTransforms = new();
        [ReadOnly] public List<PlanetSlot> planetSlots = new();

        private void Awake()
        {
            foreach (Transform thisSlotTransform in planetSlotTransforms)
            {
                PlanetSlot newPlanetSlot = new PlanetSlot(thisSlotTransform, null);
                planetSlots.Add(newPlanetSlot);
            }
        }

        public Transform AddMoveableToAvailableSlot(Moveable moveableToAdd)
        {
            foreach (PlanetSlot thisPlanetSlot in planetSlots)     //Can't use a foreach loop because thisPlanetSlot is readonly in a foreach loop
            {
                if (thisPlanetSlot.occupyingMoveable != null)
                {
                    //Occupied
                    continue;
                }

                thisPlanetSlot.occupyingMoveable = moveableToAdd;
                return thisPlanetSlot.slotTransform;
            }

            //No slots were available;
            return null;
        }

        public void RemoveMoveableFromSlot(Moveable moveableToRemove)
        {
            foreach (PlanetSlot thisPlanetSlot in planetSlots)
            {
                if (thisPlanetSlot.occupyingMoveable == moveableToRemove)
                {
                    thisPlanetSlot.occupyingMoveable = null;
                    return;
                }
            }
        }

        public bool AreAnySlotsAvailable()
        {
            foreach (PlanetSlot thisPlanetSlot in planetSlots)
            {
                if (thisPlanetSlot.occupyingMoveable == null)
                {
                    return true;
                }
            }

            return false;
        }

        public List<Moveable> GetAllMoveablesAtPlanet()
        {
            List<Moveable> moveables = new();

            foreach (PlanetSlot thisPlanetSlot in planetSlots)
            {
                if (thisPlanetSlot.occupyingMoveable == null)
                {
                    //Slot empty
                    continue;
                }

                moveables.Add(thisPlanetSlot.occupyingMoveable);
            }

            return moveables;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;

            foreach (Transform thisSlotTransform in planetSlotTransforms)
            {
                Gizmos.DrawWireSphere(thisSlotTransform.position, 0.25f);
            }
        }
    }
}
