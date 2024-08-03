using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Abraham.GalacticConquest
{
    public class PlanetBehaviour : MonoBehaviour
    {
        public List<PlanetSlot> planetSlots = new();

        private void Awake()
        {
            planetSlots = new List<PlanetSlot>(GetComponentsInChildren<PlanetSlot>());
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

        public PlanetSlot GetAvailablePlanetSlot()
        {
            foreach (PlanetSlot thisPlanetSlot in planetSlots)
            {
                if (thisPlanetSlot.IsSlotAvailable())
                {
                    return thisPlanetSlot;
                }
            }

            return null;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;

            foreach (PlanetSlot thisPlanetSlot in planetSlots)
            {
                Gizmos.DrawWireSphere(thisPlanetSlot.transform.position, 0.25f);
            }
        }
    }
}
