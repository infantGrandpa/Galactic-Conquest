using System.Collections.Generic;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class FleetBehaviour : MonoBehaviour
    {
        public FactionHandler MyFaction { get; private set; }

        private void Awake()
        {
            MyFaction = GetComponent<FactionHandler>();
        }

        public void FleetArrivedAtPlanet(PlanetBehaviour targetPlanet)
        {
            List<Moveable> moveablesAtPlanet = targetPlanet.PlanetSlotHandler.GetAllMoveablesAtPlanet();

            foreach (Moveable thisMoveable in moveablesAtPlanet)
            {
                FactionHandler moveableFaction = thisMoveable.GetComponent<FactionHandler>();
                if (moveableFaction == null)
                {
                    //Moveable doesn't have a faction
                    continue;
                }

                if (!MyFaction.IsEnemyFaction(moveableFaction.myFaction))
                {
                    //Not enemy faction
                    continue;
                }

                if (!thisMoveable.TryGetComponent(out FleetBehaviour enemyFleetBehaviour))
                {
                    //not a fleet
                    continue;
                }

                GUIManager.Instance.AddActionLogMessage("Initiating space battle over " + targetPlanet.planetName + "...");
                BattleHandler.Instance.StartBattle(this, enemyFleetBehaviour, targetPlanet);
            }
        }
    }
}
