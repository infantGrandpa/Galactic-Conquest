using Abraham.GalacticConquest.Combat;
using UnityEngine;

namespace Abraham.GalacticConquest.GUI
{
    public class GUIManager : MonoBehaviour
    {
        public static GUIManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType(typeof(GUIManager)) as GUIManager;

                return _instance;
            }
            set => _instance = value;
        }
        private static GUIManager _instance;

        public Canvas mainCanvas;
        public Camera mainCamera;

        [Header("Non-Diegetic Elements")] 
        [SerializeField] GUITurnHandler turnHandler;
        [SerializeField] GUIActionPointHandler actionPointHandler;
        [SerializeField] GUIActionLogHandler actionLogHandler;
        [SerializeField] GUIBattleHandler guiBattleHandler;

        [Header("Spatial Elements")] [SerializeField]
        GUISpatialHandler spatialHandler;
        private void Awake()
        {
            mainCanvas = GetComponent<Canvas>();
            mainCamera = Camera.main;
            //Activate the battle handler so it can assign all it's variables
            guiBattleHandler.gameObject.SetActive(true);
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

        public void AddUIElementToSpatialCanvas(Transform transformToAdd)
        {
            spatialHandler.AddUIElement(transformToAdd);
        }
    }
}
