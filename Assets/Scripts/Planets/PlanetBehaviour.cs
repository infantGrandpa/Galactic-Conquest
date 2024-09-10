using System;
using Abraham.GalacticConquest.Factions;
using Abraham.GalacticConquest.GUI;
using Abraham.GalacticConquest.Traits;
using UnityEngine;

namespace Abraham.GalacticConquest.Planets
{
    public class PlanetBehaviour : MonoBehaviour
    {
        public Planet planet;
        
        public PlanetSlotHandler PlanetSlotHandler { get; private set; }
        public PlanetCombatBehaviour PlanetCombatBehaviour { get; private set; }
        public FactionHandler FactionHandler { get; private set; }
        public TraitHandler TraitHandler { get; private set; }

        [Header("Planet Label")] [SerializeField]
        GameObject planetLabelPrefab;
        PlanetLabelBehaviour planetLabel;

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

            GameObject newPlanetLabel = Instantiate(planetLabelPrefab);

            planetLabel = newPlanetLabel.GetComponent<PlanetLabelBehaviour>();
            if (planetLabel == null) {
                Debug.LogError("ERROR PlanetBehaviour Start(): The planet label prefab is missing a PlanetLabelBehaviour component.");
                return;
            }
        }

        void Start()
        {
            planetLabel.InitLabel(planet, FactionHandler.myFaction, TraitHandler, transform.position);
        }

        public void CapturePlanet() //Called by HealthSystem OnDeathEvent
        {
            Faction newFaction = PlanetCombatBehaviour.GetInvaderFaction();
            FactionHandler.SetFaction(newFaction);
            planetLabel.UpdateLabelFaction(newFaction);

            PlanetCombatBehaviour.ResetPlanetAfterCapture();

            GUIManager.Instance.AddActionLogMessage("Planet captured by " + newFaction.factionName + "!");

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

        public void UpdateApLabel(int newAp)
        {
            planetLabel.UpdateAPLabel(newAp);
        }
    }
}
