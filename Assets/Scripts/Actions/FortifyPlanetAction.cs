using Abraham.GalacticConquest.ActionPoints;
using Abraham.GalacticConquest.GUI;
using Abraham.GalacticConquest.Planets;

namespace Abraham.GalacticConquest.Actions
{
    public class FortifyPlanetAction : GameAction
    {
        private readonly PlanetBehaviour _planet;

        public FortifyPlanetAction(PlanetBehaviour planetToFortify)
        {
            _planet = planetToFortify;
        }

        protected override int CalculateActionPointCost()
        {
            return ActionPointManager.Instance.fortifyPlanetCost;
        }

        public override bool CanExecuteAction()
        {
            if (_planet.IsPlanetFortified())
            {
                Result = ActionResult.Failure(GetActionTypeName(), "Planet is already fortified.");
                return Result.WasSuccessful;
            }

            int apCost = GetActionPointCost();
            if (!ActionPointManager.Instance.CanPerformAction(apCost))
            {
                Result = ActionResult.Failure(GetActionTypeName(), $"Fortifying requires {apCost} AP.");
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

            _planet.FortifyPlanet();

            int apCost = GetActionPointCost();
            ActionPointManager.Instance.DecreaseActionPoints(apCost);
            GUIManager.Instance.AddActionLogMessage($"Fortified {_planet.PlanetInfo.myName}.", apCost * -1);

            Result = ActionResult.Success(GetActionTypeName(), apCost, $"Fortified {_planet.PlanetInfo.myName}.");
            return Result.WasSuccessful;
        }

        public override bool UndoAction()
        {
            _planet.UnfortifyPlanet();
            
            int apCost = Result.APCost;
            ActionPointManager.Instance.IncreaseActionPoints(apCost);
            GUIManager.Instance.AddActionLogMessage($"Removed fortifications from {_planet.PlanetInfo.myName}.", apCost);
            return true;
        }
    }
}