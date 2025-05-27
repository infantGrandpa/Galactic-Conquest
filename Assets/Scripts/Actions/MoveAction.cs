using Abraham.GalacticConquest.ActionPoints;
using Abraham.GalacticConquest.GUI;
using Abraham.GalacticConquest.Planets;
using Abraham.GalacticConquest.UnitControl;
using UnityEngine;

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
            throw new System.NotImplementedException();
        }

        public bool CanExecuteAction()
        {
            if (_moveableObject == null)
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


            // Get Ring level and check AP cost 
            // This is last so we don't send a message about insufficient AP if you click on a planet the object is already at
            float distanceToTarget = _moveableObject.GetDistanceToTarget(_targetPlanet.transform.position);
            int ringLevel = MovementManager.Instance.GetRingLevelFromDistance(distanceToTarget);
            _apMoveCost = _moveableObject.CalculateMovementCost(ringLevel);

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