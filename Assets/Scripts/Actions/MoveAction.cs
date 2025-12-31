using System;
using Abraham.GalacticConquest.ActionPoints;
using Abraham.GalacticConquest.GUI;
using Abraham.GalacticConquest.Planets;
using Abraham.GalacticConquest.UnitControl;

namespace Abraham.GalacticConquest.Actions
{
    public class MoveAction : GameAction
    {
        private readonly Moveable _moveableObject;
        private readonly PlanetBehaviour _targetPlanet;

        private PlanetBehaviour _startingPlanet;

        public MoveAction(Moveable moveableObject, PlanetBehaviour targetPlanet)
        {
            _moveableObject = moveableObject;
            _targetPlanet = targetPlanet;
        }


        protected override int CalculateActionPointCost()
        {
            if (!_moveableObject)
            {
                throw new InvalidOperationException(
                    "MoveableObject is required but was null. Ensure the game action was properly initialized.");
            }

            float distanceToTarget = _moveableObject.GetDistanceToTarget(_targetPlanet.transform.position);
            int ringLevel = MovementManager.Instance.GetRingLevelFromDistance(distanceToTarget);
            int apCost = _moveableObject.CalculateMovementCost(ringLevel);
            return apCost;
        }

        public override bool CanExecuteAction()
        {
            if (!_moveableObject)
            {
                Result = ActionResult.Failure(GetActionTypeName(), "A moveable object is not selected.");
                return Result.WasSuccessful;
            }

            if (!_targetPlanet)
            {
                Result = ActionResult.Failure(GetActionTypeName(), "The user didn't click on a planet.");
                return Result.WasSuccessful;
            }
            
            bool canMove = _moveableObject.CanMoveToTarget(_targetPlanet);
            if (!canMove)
            {
                Result = ActionResult.Failure(GetActionTypeName(), $"Move prevented by moveable object.");
                return Result.WasSuccessful;
            }
            
            int apCost = GetActionPointCost();
            if (!ActionPointManager.Instance.CanPerformAction(apCost))
            {
                string msg = $"This move action requires {apCost} AP.";
                GUIManager.Instance.AddActionLogMessage(msg);
                
                Result = ActionResult.Failure(GetActionTypeName(), msg);
                return Result.WasSuccessful;
            }

            return true;
        }

        public override bool ExecuteAction()
        {
            if (!CanExecuteAction())
            {
                return Result.WasSuccessful;
            }

            _startingPlanet = _moveableObject.currentPlanet;
            
            //Send Move Command to moveable object
            bool moveSuccessful = _moveableObject.MoveToPlanet(_targetPlanet);
            if (!moveSuccessful)
            {
                Result = ActionResult.Failure(GetActionTypeName(), $"Move cancelled by moveable object.");
                return Result.WasSuccessful;
            }

            int apCost = GetActionPointCost();
            ActionPointManager.Instance.DecreaseActionPoints(apCost);

            string msg = $"Moved {_moveableObject.gameObject.name} to {_targetPlanet.PlanetInfo.myName}.";
            GUIManager.Instance.AddActionLogMessage(msg, apCost * -1);

            Result = ActionResult.Success(GetActionTypeName(), apCost, msg);
            return Result.WasSuccessful;
        }

        public override bool UndoAction()
        {
            if (!_startingPlanet)
            {
                return false;
            }

            bool moveSuccessful = _moveableObject.MoveToPlanet(_startingPlanet);
            if (!moveSuccessful)
            {
                return false;
            }
            
            int apCost = GetActionPointCost();
            ActionPointManager.Instance.IncreaseActionPoints(apCost);
            GUIManager.Instance.AddActionLogMessage($"{_moveableObject.gameObject.name} returned to {_startingPlanet.PlanetInfo.myName}.", apCost);
            return true;
        }
    }
}