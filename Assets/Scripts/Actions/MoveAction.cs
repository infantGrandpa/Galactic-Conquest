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
        
        private int? _cachedApCost = null;

        public MoveAction(Moveable moveableObject, PlanetBehaviour targetPlanet)
        {
            _moveableObject = moveableObject;
            _targetPlanet = targetPlanet;
        }


        public int GetActionPointCost()
        {
            // If we've already calculated the AP cost for this action, just return that.
            // This could cause an issue if that action cost would change during runtime, but I don't think that is the case right now.
            if (_cachedApCost != null)
            {
                return _cachedApCost.Value;
            }

            if (!_moveableObject)
            {
                throw new InvalidOperationException(
                    "MoveableObject is required but was null. Ensure the game action was properly initialized.");
            }

            float distanceToTarget = _moveableObject.GetDistanceToTarget(_targetPlanet.transform.position);
            int ringLevel = MovementManager.Instance.GetRingLevelFromDistance(distanceToTarget);
            _cachedApCost = _moveableObject.CalculateMovementCost(ringLevel);
            return _cachedApCost.Value;
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
            int apCost = GetActionPointCost();
            if (!ActionPointManager.Instance.CanPerformAction(apCost))
            {
                //Not Enough AP. Cancel.
                GUIManager.Instance.AddActionLogMessage("INSUFFICIENT AP (" + apCost + "): Movement Cancelled.");
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

            int apCost = GetActionPointCost();
            ActionPointManager.Instance.DecreaseActionPoints(apCost);
            return true;
        }

        public bool UndoAction()
        {
            throw new System.NotImplementedException();
        }
    }
}