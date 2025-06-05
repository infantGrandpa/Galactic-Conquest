namespace Abraham.GalacticConquest.Actions
{
    public interface IGameAction
    {
        int GetActionPointCost();
        bool CanExecuteAction();
        // TODO: Instead of returning a bool, return an ActionResult class that provides details on the result 
        //  This would include a message, the AP cost, if it was a success, and possibly more. 
        bool ExecuteAction();
        bool UndoAction();
    }
}
