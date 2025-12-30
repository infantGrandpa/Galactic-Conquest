using Sirenix.OdinInspector;

namespace Abraham.GalacticConquest.Actions
{
    public abstract class GameAction
    {
        protected int CachedApCost;
        [ShowInInspector, ReadOnly] protected ActionResult Result;

        public int GetActionPointCost()
        {
            return CachedApCost == 0 ? CalculateActionPointCost() : CachedApCost;
        }

        protected abstract int CalculateActionPointCost();
        public abstract bool CanExecuteAction();
        public abstract bool ExecuteAction();
        public abstract bool UndoAction();
    }
}