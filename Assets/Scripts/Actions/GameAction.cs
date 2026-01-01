using Sirenix.OdinInspector;

namespace Abraham.GalacticConquest.Actions
{
    public abstract class GameAction
    {
        protected int CachedApCost;
        [ShowInInspector, ReadOnly, HideLabel] protected ActionResult Result;

        public string GetActionTypeName()
        {
            string className = GetType().Name;
            
            //Remove "Action" from class name
            if (className.EndsWith("Action"))
            {
                className = className.Substring(0, className.Length - 6);
            }
            
            // Convert into multiple words
            string result = "";
            for (int i = 0; i < className.Length; i++)
            {
                if (i > 0 && char.IsUpper(className[i]))
                {
                    result += " ";
                }
                result += className[i];
            }
            
            return result;
        }
        
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