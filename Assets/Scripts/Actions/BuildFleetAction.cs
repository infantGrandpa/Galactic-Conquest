using Abraham.GalacticConquest.ActionPoints;
using Abraham.GalacticConquest.GUI;
using Abraham.GalacticConquest.UnitControl;
using UnityEngine;

namespace Abraham.GalacticConquest.Actions
{
    public class BuildFleetAction : IGameAction
    {
        private readonly ShipyardBehaviour _shipyardBehaviour;

        private int? _apCost;
        

        public BuildFleetAction(ShipyardBehaviour shipyardBehaviour)
        {
            _shipyardBehaviour = shipyardBehaviour;
        }
        
        public int GetActionPointCost()
        {
            return ActionPointManager.Instance.buildShipApCost;
        }
        
        public bool CanExecuteAction()
        {
            bool slotsAvailable = _shipyardBehaviour.AreAnyPlanetSlotsAvailable();
            if (!slotsAvailable) {
                return false;
            }

            _apCost = GetActionPointCost();
            return ActionPointManager.Instance.CanPerformAction(_apCost.Value);
        }

        public bool ExecuteAction()
        {
            if (!CanExecuteAction()) {
                GUIManager.Instance.AddActionLogMessage("Unable to build a fleet at " + _shipyardBehaviour.GetPlanetName());
                return false;
            }
            
            bool success = CreateFleetAtShipyard();
            if (!success)
            {
                return false;
            }
            
            ActionPointManager.Instance.DecreaseActionPoints(GetActionPointCost());
            return true;
        }

        public bool UndoAction()
        {
            throw new System.NotImplementedException();
        }

        private bool CreateFleetAtShipyard()
        {
            string factionName = _shipyardBehaviour.GetFactionName();
            string planetName = _shipyardBehaviour.GetPlanetName();
            
            GameObject fleetToBuild = LevelManager.Instance.fleetPrefab;
            if (!fleetToBuild)
            {
                throw new MissingReferenceException("LevelManager's fleet prefab is null.");
            }

            GameObject newFleet = LevelManager.Instance.InstantiateOnDynamicTransform(fleetToBuild);

            Moveable moveable = newFleet.GetComponent<Moveable>();
            if (!moveable)
            {
                throw new MissingComponentException($"Fleet prefab {newFleet.gameObject.name} does not have a Moveable component.");
            }
            
            Transform slotTransform = _shipyardBehaviour.AddMoveableToPlanetSlot(moveable);
            if (!slotTransform) {
                GUIManager.Instance.AddActionLogMessage($"Unable to build a new fleet. No available slots at {planetName}.");    
                return false;
            }

            _shipyardBehaviour.PositionFleetAtPlanetSlot(moveable, slotTransform);
            _shipyardBehaviour.SetFactionForNewFleet(newFleet);
            
            GUIManager.Instance.AddActionLogMessage($"{factionName} built a new fleet at {planetName}.");
            return true;
            
        }
    }
}
