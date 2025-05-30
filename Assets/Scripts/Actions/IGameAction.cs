namespace Abraham.GalacticConquest.Actions
{
    public interface IGameAction
    {
        int GetActionPointCost();
        bool CanExecuteAction();
        //TODO: Instead of returning a bool, return an ActionResult class that provides details on the result 
        bool ExecuteAction();
        bool UndoAction();
    }
}
