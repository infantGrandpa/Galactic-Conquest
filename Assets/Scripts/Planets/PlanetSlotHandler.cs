using UnityEngine;
using System.Collections.Generic;
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

        private void OnEnable()
        {
            MovementManager.Instance.planets.Add(this);
        }

        private void OnDisable()
        {
            if (MovementManager.Instance == null)
            {
                return;
            }

            MovementManager.Instance.planets.Remove(this);
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
