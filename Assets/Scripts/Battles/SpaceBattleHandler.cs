using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class SpaceBattleHandler : MonoBehaviour
    {
        public void StartSpaceBattle(Battle battleInfo)
        {
            Faction attackingFaction = battleInfo.attackingFleet.FactionHandler.myFaction;
            Faction defendingFaction = battleInfo.defendingFleet.FactionHandler.myFaction;
            GUIManager.Instance.ShowBattleDialogBox(attackingFaction, defendingFaction, battleInfo.battlePlanet);
        }

    }
}
