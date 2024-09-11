using System;
using Abraham.GalacticConquest.Factions;
using Abraham.GalacticConquest.Fleets;
using Abraham.GalacticConquest.Planets;
using Abraham.GalacticConquest.UnitControl;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class ShipyardBehaviour : MonoBehaviour
    {
        PlanetSlotHandler planetSlotHandler;
        FactionHandler factionHandler;

        void Awake()
        {
            planetSlotHandler = GetComponent<PlanetSlotHandler>();
            factionHandler = GetComponent<FactionHandler>();
        }

        [ContextMenu("Build Fleet")]
        public void BuildFleet()
        {
            //TODO: Check if we can build a fleet at this planet

            GameObject fleetToBuild = LevelManager.Instance.fleetPrefab;
            if (fleetToBuild == null) {
                Debug.LogError("ERROR ShipyardBehaviour BuildFleet(): LevelManager's fleet prefab is null.", this);
                return;
            }

            GameObject newFleet = Instantiate(fleetToBuild, LevelManager.Instance.DynamicTransform);

            Moveable moveable = newFleet.GetComponent<Moveable>();
            Transform slotTransform = planetSlotHandler.AddMoveableToAvailableSlot(moveable);

            if (slotTransform == null) {
                Debug.LogError("ERROR ShipyardBehaviour BuildFleet(): No available planet slots at " + gameObject.name, this);
                return;
            }
            newFleet.transform.position = slotTransform.position;
            moveable.ChangeCurrentPlanet(gameObject);

            SetFleetFaction(newFleet);
        }
        void SetFleetFaction(GameObject newFleet)
        {
            FactionHandler fleetFactionHandler = newFleet.GetComponent<FactionHandler>();
            fleetFactionHandler.SetFaction(factionHandler.myFaction);
        }
    }
}
