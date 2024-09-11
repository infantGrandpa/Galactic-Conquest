using System;
using Abraham.GalacticConquest.ActionPoints;
using Abraham.GalacticConquest.Factions;
using Abraham.GalacticConquest.Fleets;
using Abraham.GalacticConquest.GUI;
using Abraham.GalacticConquest.Planets;
using Abraham.GalacticConquest.UnitControl;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class ShipyardBehaviour : MonoBehaviour
    {
        PlanetSlotHandler planetSlotHandler;
        FactionHandler factionHandler;

        GenericInfo info;

        void Awake()
        {
            planetSlotHandler = GetComponent<PlanetSlotHandler>();
            factionHandler = GetComponent<FactionHandler>();
            info = GetComponent<GenericInfo>();
        }

        public bool CanBuildFleet()
        {
            bool slotsAvailable = planetSlotHandler.AreAnySlotsAvailable();
            if (!slotsAvailable) {
                return false;
            }

            int buildShipCost = ActionPointManager.Instance.buildShipApCost;
            bool enoughAp = ActionPointManager.Instance.CanPerformAction(buildShipCost);
            if (!enoughAp) {
                return false;
            }

            return true;
        }

        [ContextMenu("Build Fleet")]
        public void BuildFleet()
        {
            if (!CanBuildFleet()) {
                GUIManager.Instance.AddActionLogMessage("Unable to build a fleet at " + info.myName);
                return;
            }

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

            int buildShipCost = ActionPointManager.Instance.buildShipApCost;
            ActionPointManager.Instance.DecreaseActionPoints(buildShipCost);
        }
        void SetFleetFaction(GameObject newFleet)
        {
            FactionHandler fleetFactionHandler = newFleet.GetComponent<FactionHandler>();
            fleetFactionHandler.SetFaction(factionHandler.myFaction);
        }
    }
}
