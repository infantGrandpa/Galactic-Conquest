using Abraham.GalacticConquest.TurnManagement;
using UnityEngine;

namespace Abraham.GalacticConquest.GUI
{
    public class GUINextTurnButton : MonoBehaviour
    {
        //Called by Next Turn Button's OnClick event
        public void NextTurn()
        {
            BasicTurnManagement.Instance.NextTurn();
        }
    }
}
