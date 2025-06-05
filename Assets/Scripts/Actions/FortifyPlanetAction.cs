using Abraham.GalacticConquest.ActionPoints;
using Abraham.GalacticConquest.GUI;
using Abraham.GalacticConquest.Planets;

namespace Abraham.GalacticConquest.Actions
{
    public class FortifyPlanetAction : IGameAction
    {
        private readonly PlanetBehaviour _planet;
        
        public FortifyPlanetAction(PlanetBehaviour planetToFortify)
        {
            _planet = planetToFortify;
        }
        
        public int GetActionPointCost()
        {
            return ActionPointManager.Instance.fortifyPlanetCost;
        }

        public bool CanExecuteAction()
        {
            if (_planet.IsPlanetFortified())
            {
                return false;
            }
            
            return ActionPointManager.Instance.CanPerformAction(GetActionPointCost());
        }

        public bool ExecuteAction()
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

        public bool UndoAction()
        {
            _planet.UnfortifyPlanet();

            int apCost = GetActionPointCost();
            ActionPointManager.Instance.IncreaseActionPoints(apCost);
            GUIManager.Instance.AddActionLogMessage($"Removed fortifications from {_planet.PlanetInfo.myName}.", apCost);
            return true;
        }
    }
}
