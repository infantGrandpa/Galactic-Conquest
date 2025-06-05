using System;
using Abraham.GalacticConquest.ActionPoints;
using Abraham.GalacticConquest.GUI;
using Abraham.GalacticConquest.Planets;
using Abraham.GalacticConquest.UnitControl;

namespace Abraham.GalacticConquest.Actions
{
    public class MoveAction : IGameAction
    {
        private readonly Moveable _moveableObject;
        private readonly PlanetBehaviour _targetPlanet;

        private int? _apCost;

        public MoveAction(Moveable moveableObject, PlanetBehaviour targetPlanet)
        {
            _moveableObject = moveableObject;
            _targetPlanet = targetPlanet;
        }


        public int GetActionPointCost()
        {
            if (_apCost != null)
            {
                return _apCost.Value;
            }

            if (!_moveableObject)
            {
                throw new InvalidOperationException(
                    "MoveableObject is required but was null. Ensure the game action was properly initialized.");
            }

            float distanceToTarget = _moveableObject.GetDistanceToTarget(_targetPlanet.transform.position);
            int ringLevel = MovementManager.Instance.GetRingLevelFromDistance(distanceToTarget);
            _apCost = _moveableObject.CalculateMovementCost(ringLevel);
            return _apCost.Value;
        }

        public bool CanExecuteAction()
        {
            if (!_moveableObject)
            {
                return false;
            }

            if (!_targetPlanet)
            {
                //Didn't click on a planet. Cancel.
                return false;
            }

            bool canMove = _moveableObject.CanMoveToTarget(_targetPlanet);
            if (!canMove)
            {
                //Moveable object already at planet. Cancel.
                return false;
            }
            
            _apCost = GetActionPointCost();
            return ActionPointManager.Instance.CanPerformAction(_apCost.Value);
        }

        public bool ExecuteAction()
        {
            if (!CanExecuteAction())
            {
                return false;
            }

            //Send Move Command to moveable object
            bool moveSuccessful = _moveableObject.MoveToPlanet(_targetPlanet);
            if (!moveSuccessful)
            {
                //Move cancelled by moveable object.
                return false;
            }

            int apCost = GetActionPointCost();
            ActionPointManager.Instance.DecreaseActionPoints(apCost);
            GUIManager.Instance.AddActionLogMessage($"Moved {_moveableObject.gameObject.name} to {_targetPlanet.PlanetInfo.myName}.",
                GetActionPointCost() * -1);
            return true;
        }

        public bool UndoAction()
        {
            throw new NotImplementedException();
        }
    }
}