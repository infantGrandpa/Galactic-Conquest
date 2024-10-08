using Abraham.GalacticConquest.Combat;
using Abraham.GalacticConquest.Planets;
using Sirenix.OdinInspector;
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

        [ReadOnly] public Canvas mainCanvas;
        [ReadOnly] public Camera mainCamera;

        [Header("Non-Diegetic Elements")] 
        [SerializeField] GUITurnHandler turnHandler;
        [SerializeField] GUIActionPointHandler actionPointHandler;
        [SerializeField] GUIActionLogHandler actionLogHandler;
        [SerializeField] GUIBattleHandler guiBattleHandler;
        [SerializeField] GUIInfoBoxHandler infoBoxHandler;

        [Header("Spatial Elements")] [SerializeField]
        GUISpatialHandler spatialHandler;
        [SerializeField] GUIActionListHandler actionListHandler;
        [SerializeField] GUIMovementCostIndicator costIndicator;
        
        private void Awake()
        {
            mainCanvas = GetComponent<Canvas>();
            mainCamera = Camera.main;

            //Activate the battle handler so it can assign all its variables
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

        public void ShowActionListForPlanet(PlanetBehaviour planet)
        {
            actionListHandler.ShowActionList(planet);
        }

        public void HideActionList()
        {
            actionListHandler.HideActionList();
        }

        public void UpdateMovementCostIndicator(int newCost)
        {
            costIndicator.UpdateMovementCost(newCost);
        }

        public void ShowInfoBox(GameObject target)
        {
            infoBoxHandler.ShowInfoBox(target);
        }

        public void HideInfoBox()
        {
            infoBoxHandler.HideInfoBox();
        }
    }
}
