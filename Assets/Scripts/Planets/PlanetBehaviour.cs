using System.Collections.Generic;
using Abraham.GalacticConquest.Factions;
using Abraham.GalacticConquest.GUI;
using Abraham.GalacticConquest.Traits;
using Abraham.GalacticConquest.UnitControl;
using UnityEngine;

namespace Abraham.GalacticConquest.Planets
{
    public class PlanetBehaviour : MonoBehaviour
    {
        public GenericInfo PlanetInfo { get; private set; }
        public PlanetSlotHandler PlanetSlotHandler { get; private set; }
        public PlanetCombatBehaviour PlanetCombatBehaviour { get; private set; }
        public FactionHandler FactionHandler { get; private set; }
        public TraitHandler TraitHandler { get; private set; }

        [Header("Planet Label")] [SerializeField]
        private GameObject planetLabelPrefab;

        private PlanetLabelBehaviour _planetLabel;

        private void OnEnable()
        {
            LevelManager.Instance.planets.Add(this);
        }

        private void OnDisable()
        {
            if (LevelManager.Instance == null) {
                return;
            }

            LevelManager.Instance.planets.Remove(this);
        }
        
        private void Awake()
        {
            PlanetSlotHandler = GetComponent<PlanetSlotHandler>();
            PlanetCombatBehaviour = GetComponent<PlanetCombatBehaviour>();
            FactionHandler = GetComponent<FactionHandler>();
            TraitHandler = GetComponent<TraitHandler>();
            PlanetInfo = GetComponent<GenericInfo>();

            GameObject newPlanetLabel = Instantiate(planetLabelPrefab);

            _planetLabel = newPlanetLabel.GetComponent<PlanetLabelBehaviour>();
            if (_planetLabel == null) {
                Debug.LogError("ERROR PlanetBehaviour Start(): The planet label prefab is missing a PlanetLabelBehaviour component.");
                return;
            }
        }

        private void Start()
        {
            _planetLabel.InitLabel(PlanetInfo, FactionHandler.myFaction, TraitHandler, transform.position);
        }

        public void CapturePlanet() //Called by HealthSystem OnDeathEvent
        {
            Faction newFaction = PlanetCombatBehaviour.GetInvaderFaction();
            FactionHandler.SetFaction(newFaction);
            _planetLabel.UpdateLabelFaction(newFaction);

            PlanetCombatBehaviour.ResetPlanetAfterCapture();

            GUIManager.Instance.AddActionLogMessage(PlanetInfo.myName + " captured by " + newFaction.factionName + "!");

            LevelManager.Instance.CheckWinCondition();
        }

        public void OnSelectPlanet()
        {
            GUIManager.Instance.ShowActionListForPlanet(this);
        }

        public void OnDeselectPlanet()
        {
            if (GUIManager.Instance == null) {
                return;
            }
            
            GUIManager.Instance.HideActionList();
        }

        /// <summary>
        /// Determines if there are any forces in space above this planet that are not allied with the provided faction.
        /// </summary>
        /// <param name="currentFaction">The faction to check for enemies of.</param>
        /// <returns>True if there is at least 1 enemy fleet at this planet, otherwise false.</returns>
        public bool IsEnemyAtPlanet(Faction currentFaction)
        {
            List<Moveable> moveables = PlanetSlotHandler.GetAllMoveablesAtPlanet();
            foreach (Moveable moveable in moveables)
            {
                FactionHandler moveableFactionHandler = moveable.GetComponent<FactionHandler>();
                if (!moveableFactionHandler)
                {
                    continue;
                }

                bool isEnemyFaction = moveableFactionHandler.IsEnemyFaction(currentFaction);
                if (isEnemyFaction)
                {
                    return true;
                }
            }

            return false;
        }

        public void UpdateApLabel(int newAp)
        {
            _planetLabel?.UpdateAPLabel(newAp);
        }
    }
}
