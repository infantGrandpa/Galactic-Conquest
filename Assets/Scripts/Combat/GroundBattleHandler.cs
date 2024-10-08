using Abraham.GalacticConquest.GUI;
using UnityEngine;

namespace Abraham.GalacticConquest.Combat
{
    public class GroundBattleHandler : MonoBehaviour
    {
        public void StartGroundBattle(Battle battleInfo)
        {
            GUIManager.Instance.ShowBattleDialogBox(battleInfo);
        }
    }
}
