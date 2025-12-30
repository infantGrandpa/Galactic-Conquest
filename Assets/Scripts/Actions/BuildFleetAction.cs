using Abraham.GalacticConquest.ActionPoints;
using Abraham.GalacticConquest.GUI;
using Abraham.GalacticConquest.UnitControl;
using UnityEngine;

namespace Abraham.GalacticConquest.Actions
{
    public class BuildFleetAction : GameAction
    {
        private readonly ShipyardBehaviour _shipyardBehaviour;
        private GameObject _builtFleet;

        public BuildFleetAction(ShipyardBehaviour shipyardBehaviour)
        {
            _shipyardBehaviour = shipyardBehaviour;
        }

        protected override int CalculateActionPointCost()
        {
            return ActionPointManager.Instance.buildShipApCost;
        }

        public override bool CanExecuteAction()
        {
            bool slotsAvailable = _shipyardBehaviour.AreAnyPlanetSlotsAvailable();
            if (!slotsAvailable)
            {
                return false;
            }

            int apCost = GetActionPointCost();
            return ActionPointManager.Instance.CanPerformAction(apCost);
        }

        public override bool ExecuteAction()
        {
            if (!CanExecuteAction())
            {
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

        public override bool UndoAction()
        {
            _shipyardBehaviour.DestroyFleet(_builtFleet);
            int apCost = GetActionPointCost();
            ActionPointManager.Instance.IncreaseActionPoints(apCost);
            GUIManager.Instance.AddActionLogMessage(
                $"Removed {_shipyardBehaviour.GetFactionName()} fleet that was built at {_shipyardBehaviour.GetPlanetName()}.", apCost);
            return true;
        }

        private bool CreateFleetAtShipyard()
        {
            string factionName = _shipyardBehaviour.GetFactionName();
            string planetName = _shipyardBehaviour.GetPlanetName();

            GameObject fleetPrefab = LevelManager.Instance.fleetPrefab;
            if (!fleetPrefab)
            {
                throw new MissingReferenceException("LevelManager's fleet prefab is null.");
            }

            _builtFleet = LevelManager.Instance.InstantiateOnDynamicTransform(fleetPrefab);

            Moveable moveable = _builtFleet.GetComponent<Moveable>();
            if (!moveable)
            {
                throw new MissingComponentException($"Fleet prefab {_builtFleet.gameObject.name} does not have a Moveable component.");
            }

            Transform slotTransform = _shipyardBehaviour.AddMoveableToPlanetSlot(moveable);
            if (!slotTransform)
            {
                GUIManager.Instance.AddActionLogMessage($"Unable to build a new fleet. No available slots at {planetName}.");
                _shipyardBehaviour.DestroyFleet(_builtFleet);
                return false;
            }

            _shipyardBehaviour.PositionFleetAtPlanetSlot(moveable, slotTransform);
            _shipyardBehaviour.SetFactionForNewFleet(_builtFleet);

            GUIManager.Instance.AddActionLogMessage($"{factionName} built a new fleet at {planetName}.", GetActionPointCost() * -1);
            return true;
        }
    }
}