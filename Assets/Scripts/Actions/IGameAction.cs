namespace Abraham.GalacticConquest.Actions
{
    public interface IGameAction
    {
        int GetActionPointCost();
        bool CanExecuteAction();
        bool ExecuteAction();
        bool UndoAction();
    }
}
