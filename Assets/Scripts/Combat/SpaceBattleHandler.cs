using Abraham.GalacticConquest.GUI;
using UnityEngine;

namespace Abraham.GalacticConquest.Combat
{
    public class SpaceBattleHandler : MonoBehaviour
    {
        public void StartSpaceBattle(Battle battleInfo)
        {
            GUIManager.Instance.ShowBattleDialogBox(battleInfo);
        }

    }
}
