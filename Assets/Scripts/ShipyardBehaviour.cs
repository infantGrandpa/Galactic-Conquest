using Abraham.GalacticConquest.Factions;
using Abraham.GalacticConquest.Planets;
using Abraham.GalacticConquest.UnitControl;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class ShipyardBehaviour : MonoBehaviour
    {
        private PlanetSlotHandler _planetSlotHandler;
        private FactionHandler _factionHandler;

        private GenericInfo _info;
        
        public string GetPlanetName() => _info.myName;
        public string GetFactionName() => _factionHandler.myFaction.factionName;

        private void Awake()
        {
            _planetSlotHandler = GetComponent<PlanetSlotHandler>();
            _factionHandler = GetComponent<FactionHandler>();
            _info = GetComponent<GenericInfo>();
        }

        public bool AreAnyPlanetSlotsAvailable()
        {
            return _planetSlotHandler.AreAnySlotsAvailable();
        }

        public Transform AddMoveableToPlanetSlot(Moveable moveable)
        {
            return _planetSlotHandler.AddMoveableToAvailableSlot(moveable);
        }

        public void PositionFleetAtPlanetSlot(Moveable moveableFleet, Transform slotTransform)
        {
            moveableFleet.transform.position = slotTransform.position;
            moveableFleet.ChangeCurrentPlanet(gameObject);
        }

        public void SetFactionForNewFleet(GameObject newFleet)
        {
            FactionHandler fleetFactionHandler = newFleet.GetComponent<FactionHandler>();
            if (!fleetFactionHandler)
            {
                throw new MissingComponentException($"New Fleet {newFleet.name} is missing a FactionHandler component.");
            }
            
            fleetFactionHandler.SetFaction(_factionHandler.myFaction);
        }

        /// <summary>
        /// Destroys a gameobject. This is required because IGameActions (like BuildFleetAction) can't destroy game objects.
        /// </summary>
        /// <param name="fleetToDestroy">The gameobject to destroy.</param>
        public void DestroyFleet(GameObject fleetToDestroy)
        {
            Destroy(fleetToDestroy);
        }
    }
}