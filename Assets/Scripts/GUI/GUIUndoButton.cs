using Abraham.GalacticConquest.Actions;
using UnityEngine;

namespace Abraham.GalacticConquest.GUI
{
    public class GUIUndoButton : MonoBehaviour
    {
        //Called by Next Turn Button's OnClick event
        public void UndoLastAction()
        {
            ActionManager.Instance.UndoLastAction();
        }
    }
}
