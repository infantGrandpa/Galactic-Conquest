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
        public FactionHandler FactionHandler { get; private set; }
        
        private PlanetCombatBehaviour _planetCombatBehaviour;
        private TraitHandler _traitHandler;

        [Header("Planet Label")] [SerializeField]
        private GameObject planetLabelPrefab;

        private PlanetLabelBehaviour _planetLabel;

        private void OnEnable()
        {
            LevelManager.Instance.planets.Add(this);
        }

        private void OnDisable()
        {
            // Needed to avoid errors in editor
            if (LevelManager.Instance == null) {
                return;
            }

            LevelManager.Instance.planets.Remove(this);
        }

        private void Awake()
        {
            PlanetSlotHandler = GetComponent<PlanetSlotHandler>();
            _planetCombatBehaviour = GetComponent<PlanetCombatBehaviour>();
            FactionHandler = GetComponent<FactionHandler>();
            _traitHandler = GetComponent<TraitHandler>();
            PlanetInfo = GetComponent<GenericInfo>();

            GameObject newPlanetLabel = Instantiate(planetLabelPrefab);

            _planetLabel = newPlanetLabel.GetComponent<PlanetLabelBehaviour>();
            if (!_planetLabel)
            {
                throw new MissingComponentException(
                    "The planet label prefab is missing a PlanetLabelBehaviour component.");
            }
        }

        private void Start()
        {
            _planetLabel.InitLabel(PlanetInfo, FactionHandler.myFaction, _traitHandler, transform.position);
        }

        public void CapturePlanet() //Called by HealthSystem OnDeathEvent
        {
            Faction newFaction = _planetCombatBehaviour.GetInvaderFaction();
            FactionHandler.SetFaction(newFaction);
            _planetLabel.UpdateLabelFaction(newFaction);

            _planetCombatBehaviour.ResetPlanetAfterCapture();

            GUIManager.Instance.AddActionLogMessage(PlanetInfo.myName + " captured by " + newFaction.factionName + "!");

            LevelManager.Instance.CheckWinCondition();
        }

        public void OnSelectPlanet()
        {
            GUIManager.Instance.ShowActionListForPlanet(this);
        }

        public void OnDeselectPlanet()
        {
            // Needed to avoid errors in editor
            if (GUIManager.Instance == null) {
                return;
            }

            GUIManager.Instance.HideActionList();
        }

        /// <summary>
        /// Returns the first enemy gameobject in space above this planet that is not allied with the provided faction.
        /// </summary>
        /// <param name="currentFaction">The faction that we're looking for any enemies of.</param>
        /// <returns>The gameobject of the first enemy found, null if none were found.</returns>
        public GameObject GetEnemyAtPlanet(Faction currentFaction)
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
                    return moveable.gameObject;
                }
            }

            return null;
        }

        public void UpdateApLabel(int newAp)
        {
            _planetLabel?.UpdateAPLabel(newAp);
        }
    }
}