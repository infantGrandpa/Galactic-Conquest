using System.Collections.Generic;
using Abraham.GalacticConquest.Factions;
using Abraham.GalacticConquest.GUI;
using Abraham.GalacticConquest.Planets;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType(typeof(LevelManager)) as LevelManager;

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        private static LevelManager _instance;

        public Transform DynamicTransform { get; private set; }
        public Camera MainCamera { get; private set; }
        public Vector3 planetPlanePosition = new Vector3(0, 0, 0);

        public List<PlanetBehaviour> planets = new List<PlanetBehaviour>();

        [Tooltip("The prefab to use when shipyards build fleets.")]
        public GameObject fleetPrefab;
        
        private void Awake()
        {
            CreateDynamicTransform();

            MainCamera = Camera.main;
        }

        private void CreateDynamicTransform()
        {
            GameObject dynamicGameObject = new() { name = "_Dynamic" };
            DynamicTransform = dynamicGameObject.transform;
        }

        private void OnDrawGizmosSelected()
        {
            DrawPlaneGizmo();
        }

        private void DrawPlaneGizmo()
        {
            /*
            I copied this code from Stop the Sniper.
            I have no idea why the planeNormal matters.
            I'm keeping it because it works.
            */
            Gizmos.color = Color.blue;

            Vector3 planeNormal = Vector3.up;

            float planeDistance = 10f;
            float shortDistance = 0f;
            Vector3 cubeSizeVector;
            if (planeNormal == Vector3.up || planeNormal == Vector3.down)
            {
                cubeSizeVector = new Vector3(planeDistance, shortDistance, planeDistance);
            }
            else if (planeNormal == Vector3.right || planeNormal == Vector3.left)
            {
                cubeSizeVector = new Vector3(shortDistance, planeDistance, planeDistance);
            }
            else
            {
                cubeSizeVector = new Vector3(planeDistance, planeDistance, shortDistance);
            }

            Gizmos.DrawWireCube(transform.position, cubeSizeVector);
        }

        public void CheckWinCondition()
        {
            //Check if all planets have been captured
            Faction previousPlanetFaction = null;
            bool winConditionMet = true;
            foreach (PlanetBehaviour thisPlanet in planets) {
                FactionHandler factionHandler = thisPlanet.GetComponent<FactionHandler>();
                if (factionHandler == null) {
                    continue;
                }

                Faction currentPlanetFaction = factionHandler.myFaction;
                if (previousPlanetFaction == null) {
                    previousPlanetFaction = currentPlanetFaction; //Test all future planets against this
                    continue;
                }

                //Check if this planet's faction matches the last faction
                //If any aren't the same, then not every planet has been captured
                if (currentPlanetFaction == previousPlanetFaction) {
                    //this faction matches, move on to the next
                    continue;
                }

                //This planet doesn't match the last planet. Win condition hasn't been met
                winConditionMet = false;
                break;
            }

            if (winConditionMet) {
                GUIManager.Instance.AddActionLogMessage("WIN CONDITION MET! The " + previousPlanetFaction?.factionName + " has won!");
            }
        }

    }
}