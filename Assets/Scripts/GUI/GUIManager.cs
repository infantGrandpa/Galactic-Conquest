using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class GUIManager : MonoBehaviour
    {
        public static GUIManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType(typeof(GUIManager)) as GUIManager;

                return instance;
            }
            set
            {
                instance = value;
            }
        }
        private static GUIManager instance;

        [SerializeField] GUITurnHandler turnHandler;
        [SerializeField] GUIActionPointHandler actionPointHandler;
        [SerializeField] GUIActionLogHandler actionLogHandler;

        public void ChangeTurn(string turnString)
        {
            turnHandler.ChangeTurn(turnString);
        }

        public void UpdateActionPoints(int newValue)
        {
            actionPointHandler.UpdateActionPoints(newValue);
        }

        public void AddActionLogMessage(string newMessage)
        {
            actionLogHandler.AddLogMessage(newMessage);
        }
    }
}
