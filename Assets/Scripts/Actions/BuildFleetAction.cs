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
                Result = ActionResult.Failure(GetActionTypeName(), $"There are no available slots for new ships at {_shipyardBehaviour.GetPlanetName()}");
                return Result.WasSuccessful;
            }

            int apCost = GetActionPointCost();
            if (!ActionPointManager.Instance.CanPerformAction(apCost))
            {
                Result = ActionResult.Failure(GetActionTypeName(), $"Building a new fleet requires {apCost} AP.");
                return Result.WasSuccessful;
            }

            return true;
        }

        public override bool ExecuteAction()
        {
            if (!CanExecuteAction())
            {
                GUIManager.Instance.AddActionLogMessage(Result.Message);
                return Result.WasSuccessful;
            }

            bool success = CreateFleetAtShipyard();
            if (!success)
            {
                GUIManager.Instance.AddActionLogMessage(Result.Message);
                return Result.WasSuccessful;
            }

            int apCost = GetActionPointCost();
            ActionPointManager.Instance.DecreaseActionPoints(apCost);

            string successMsg = $"{_shipyardBehaviour.GetFactionName()} built a new fleet at {_shipyardBehaviour.GetPlanetName()}."; 
            GUIManager.Instance.AddActionLogMessage(successMsg, apCost * -1);

            Result = ActionResult.Success(GetActionTypeName(), apCost, successMsg);
            return Result.WasSuccessful;
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
                Result = ActionResult.Failure(GetActionTypeName(),
                    $"There are no available slots for new ships at {_shipyardBehaviour.GetPlanetName()}");
                _shipyardBehaviour.DestroyFleet(_builtFleet);
                return Result.WasSuccessful;
            }

            _shipyardBehaviour.PositionFleetAtPlanetSlot(moveable, slotTransform);
            _shipyardBehaviour.SetFactionForNewFleet(_builtFleet);
            return true;
        }
    }
}