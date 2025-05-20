namespace Abraham.GalacticConquest.Actions
{
    public interface IGameAction
    {
        int GetActionPointCost();
        bool Execute();
        bool Undo();
    }
}
