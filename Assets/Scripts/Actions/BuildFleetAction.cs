using Abraham.GalacticConquest.ActionPoints;
using Abraham.GalacticConquest.GUI;
using Abraham.GalacticConquest.Planets;
using UnityEngine;

namespace Abraham.GalacticConquest.Actions
{
    public class BuildFleetAction : IGameAction
    {
        private readonly ShipyardBehaviour _shipyardBehaviour;
        
        private PlanetSlotHandler _planetSlotHandler;

        public BuildFleetAction(ShipyardBehaviour shipyardBehaviour)
        {
            _shipyardBehaviour = shipyardBehaviour;
            _planetSlotHandler = GetPlanetSlotHandler();
        }
        
        public int GetActionPointCost()
        {
            return ActionPointManager.Instance.buildShipApCost;
        }

        private PlanetSlotHandler GetPlanetSlotHandler()
        {
            if (_planetSlotHandler)
            {
                return _planetSlotHandler;
            }

            PlanetSlotHandler handler = _shipyardBehaviour.GetComponent<PlanetSlotHandler>();
            if (handler)
            {
                return handler;
            }

            throw new MissingComponentException(
                $"PlanetSlotHandler component missing from {_shipyardBehaviour.gameObject.name}");
        } 
        
        public bool CanExecuteAction()
        {
            _planetSlotHandler = GetPlanetSlotHandler();
            bool slotsAvailable = _planetSlotHandler.AreAnySlotsAvailable();
            if (!slotsAvailable) {
                return false;
            }
            
            return ActionPointManager.Instance.CanPerformAction(GetActionPointCost());
        }

        public bool ExecuteAction()
        {
            if (!CanExecuteAction()) {
                // TODO: This should probably be the planet's name from GenericInfo
                GUIManager.Instance.AddActionLogMessage("Unable to build a fleet at " + _shipyardBehaviour.gameObject.name);
                return false;
            }
            
            bool success = _shipyardBehaviour.BuildFleet();
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
    }
}
