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
                return false;
            }
            
            return ActionPointManager.Instance.CanPerformAction(GetActionPointCost());
        }

        public override bool ExecuteAction()
        {
            if (!CanExecuteAction())
            {
                return false;
            }
            
            _planet.FortifyPlanet();

            int apCost = GetActionPointCost();
            ActionPointManager.Instance.DecreaseActionPoints(apCost);
            GUIManager.Instance.AddActionLogMessage($"Fortified {_planet.PlanetInfo.myName}.", apCost * -1);
            return true;
        }

        public override bool UndoAction()
        {
            _planet.UnfortifyPlanet();

            int apCost = GetActionPointCost();
            ActionPointManager.Instance.IncreaseActionPoints(apCost);
            GUIManager.Instance.AddActionLogMessage($"Removed fortifications from {_planet.PlanetInfo.myName}.", apCost);
            return true;
        }
    }
}
