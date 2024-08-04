using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Abraham.GalacticConquest
{
    public class GUIBattleHandler : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] Button attackerWonButton;
        private TMP_Text attackerButtonText;

        [SerializeField] Button defenderWonButton;
        private TMP_Text defenderButtonText;

        [SerializeField] TMP_Text descriptionText;


        [Header("Tweening")]
        [SerializeField] float secsToTweenScale;
        [SerializeField] Ease showBoxEasing;
        [SerializeField] Ease hideBoxEasing;
        private RectTransform battleHandlerTransform;

        private void Awake()
        {
            battleHandlerTransform = gameObject.GetComponent<RectTransform>();

            if (attackerWonButton == null)
            {
                Debug.LogError("ERROR GUIBattleHander Awake(): The attacker button is null. Please assign the attackerWonButton.", this);
                return;
            }

            if (defenderWonButton == null)
            {
                Debug.LogError("ERROR GUIBattleHander Awake(): The defender button is null. Please assign the defenderWonButton.", this);
                return;
            }

            attackerButtonText = attackerWonButton.GetComponentInChildren<TMP_Text>();
            defenderButtonText = defenderWonButton.GetComponentInChildren<TMP_Text>();

            gameObject.SetActive(false); //Hide the gui battle handler
        }

        public void ShowBattleDialogBox(Faction attackingFaction, Faction defendingFaction, PlanetBehaviour planetBehaviour)
        {
            if (attackingFaction == null)
            {
                Debug.LogError("ERROR GUIBattleHander ShowBattleDialogBox(): The attacking faction is null.", this);
                return;
            }

            if (defendingFaction == null)
            {
                Debug.LogError("ERROR GUIBattleHander ShowBattleDialogBox(): The defending faction is null.", this);
                return;
            }

            descriptionText.text = "Space Battle Over " + planetBehaviour.planetName;
            attackerWonButton.image.color = attackingFaction.FactionColor;
            attackerButtonText.text = attackingFaction.FactionName;

            defenderWonButton.image.color = defendingFaction.FactionColor;
            defenderButtonText.text = defendingFaction.FactionName;

            gameObject.SetActive(true);
            battleHandlerTransform.localScale = Vector2.zero;
            battleHandlerTransform.DOScale(1, secsToTweenScale).SetEase(showBoxEasing);
        }

        public void AttackerWon()
        {
            BattleHandler.Instance.AttackerWon();
            HideDialogBox();
        }

        public void DefenderWon()
        {
            BattleHandler.Instance.DefenderWon();
            HideDialogBox();
        }

        public void HideDialogBox()
        {
            battleHandlerTransform.DOScale(0, secsToTweenScale).SetEase(hideBoxEasing).OnComplete(
                () => gameObject.SetActive(false)       //Set inactive when tween is completed
            );
        }
    }
}
