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

        private int _apMoveCost = 0;

        public MoveAction(Moveable moveableObject, PlanetBehaviour targetPlanet)
        {
            _moveableObject = moveableObject;
            _targetPlanet = targetPlanet;
        }


        public int GetActionPointCost()
        {
            if (!_moveableObject)
            {
                throw new InvalidOperationException(
                    "MoveableObject is required but was null. Ensure the game action was properly initialized.");
            }
            
            float distanceToTarget = _moveableObject.GetDistanceToTarget(_targetPlanet.transform.position);
            int ringLevel = MovementManager.Instance.GetRingLevelFromDistance(distanceToTarget);
            return _moveableObject.CalculateMovementCost(ringLevel);
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

            
            // This is last so we don't send a message about insufficient AP if you click on a planet the object is already at
            _apMoveCost = GetActionPointCost();
            if (!ActionPointManager.Instance.CanPerformAction(_apMoveCost))
            {
                //Not Enough AP. Cancel.
                GUIManager.Instance.AddActionLogMessage("INSUFFICIENT AP (" + _apMoveCost + "): Movement Cancelled.");
                return false;
            }

            return true;
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

            ActionPointManager.Instance.DecreaseActionPoints(_apMoveCost);
            return true;
        }

        public bool UndoAction()
        {
            throw new System.NotImplementedException();
        }
    }
}