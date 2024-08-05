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
        [SerializeField] GUIBattleHandler guiBattleHandler;

        private void Awake()
        {
            guiBattleHandler.gameObject.SetActive(true);        //Activate the battle handler so it can assign all it's variables
        }

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

        public void ShowBattleDialogBox(Battle battleInfo)
        {
            guiBattleHandler.ShowBattleDialogBox(battleInfo);
        }
    }
}
