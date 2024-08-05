using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class SpaceBattleHandler : MonoBehaviour
    {
        public void StartSpaceBattle(Battle battleInfo)
        {
            GUIManager.Instance.ShowBattleDialogBox(battleInfo);
        }

    }
}
