using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Abraham.GalacticConquest
{
    public class GUIBattleHandler : MonoBehaviour
    {
        [SerializeField] Button attackerWonButton;
        private TMP_Text attackerButtonText;

        [SerializeField] Button defenderWonButton;
        private TMP_Text defenderButtonText;

        [SerializeField] TMP_Text descriptionText;

        private void Awake()
        {
            if (attackerWonButton == null)
            {
                Debug.LogError("ERROR GUIBattleHander Awake(): The attacker button is null. Please assign the attackerWonButton.");
                return;
            }

            if (defenderWonButton == null)
            {
                Debug.LogError("ERROR GUIBattleHander Awake(): The defender button is null. Please assign the defenderWonButton.");
                return;
            }

            attackerButtonText = attackerWonButton.GetComponentInChildren<TMP_Text>();
            defenderButtonText = defenderWonButton.GetComponentInChildren<TMP_Text>();
        }

        public void ShowBattleDialogBox(Faction attackingFaction, Faction defendingFaction, PlanetBehaviour planetBehaviour)
        {
            descriptionText.text = "Space Battle Over " + planetBehaviour.planetName;
            attackerWonButton.image.color = attackingFaction.FactionColor;
            attackerButtonText.text = attackingFaction.FactionName;

            defenderWonButton.image.color = defendingFaction.FactionColor;
            defenderButtonText.text = defendingFaction.FactionName;

            gameObject.SetActive(true);
        }

        public void AttackerWon()
        {
            BattleHandler.Instance.AttackerWon();
        }

        public void DefenderWon()
        {
            BattleHandler.Instance.DefenderWon();
        }
    }
}
