using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Abraham.GalacticConquest
{
    public class GUIBattleHandler : MonoBehaviour
    {
        #region Variables
        [Header("UI Elements")]
        [SerializeField] Button attackerWonButton;
        private TMP_Text attackerButtonText;

        [SerializeField] Button defenderWonButton;
        private TMP_Text defenderButtonText;

        [SerializeField] TMP_Text descriptionText;

        [Header("Text")]
        [SerializeField] string spaceBattlePrefix = "Space Battle Over";
        [SerializeField] string groundBattlePrefix = "Ground Invasion of";


        [Header("Tweening")]
        [SerializeField] float secsToTweenScale;
        [SerializeField] Ease showBoxEasing;
        [SerializeField] Ease hideBoxEasing;
        private RectTransform battleHandlerTransform;
        #endregion

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
        public void ShowBattleDialogBox(Battle battleInfo)
        {
            if (battleInfo == null)
            {
                Debug.LogError("ERROR GUIBattleHandler ShowBattleDialogBox(): Provided battleInfo is null.");
                return;
            }

            Faction attackingFaction = GetAttackingFaction(battleInfo);
            Faction defendingFaction = GetDefendingFaction(battleInfo);

            SetText(battleInfo);
            SetButtons(attackingFaction, defendingFaction);

            StartShowTween();
        }

        #region Get Dialog Box Info
        private void SetText(Battle battleInfo)
        {
            string descPrefix = battleInfo.battleType == Battle.BattleType.SpaceBattle ? spaceBattlePrefix : groundBattlePrefix;
            descriptionText.text = descPrefix + " " + battleInfo.battlePlanet.planetName;
        }

        private void SetButtons(Faction attackingFaction, Faction defendingFaction)
        {
            attackerWonButton.image.color = attackingFaction.FactionColor;
            attackerButtonText.text = attackingFaction.FactionName;

            defenderWonButton.image.color = defendingFaction.FactionColor;
            defenderButtonText.text = defendingFaction.FactionName;
        }

        private Faction GetAttackingFaction(Battle battleInfo)
        {
            #region Validation
            if (battleInfo.attackingFleet == null)
            {
                Debug.LogError("ERROR GUIBattleHandler GetAttackingFaction(): Provided battleInfo has no attacking fleet.");
                return null;
            }

            if (battleInfo.attackingFleet.FactionHandler == null)
            {
                Debug.LogError("ERROR GUIBattleHandler GetAttackingFaction(): Provided attacking fleet has no faction handler.");
                return null;
            }

            if (battleInfo.attackingFleet.FactionHandler.myFaction == null)
            {
                Debug.LogError("ERROR GUIBattleHandler GetAttackingFaction(): Provided attacking fleet's faction handler is missing a faction.");
                return null;
            }
            #endregion

            return battleInfo.attackingFleet.FactionHandler.myFaction;
        }

        private Faction GetDefendingFaction(Battle battleInfo)
        {
            #region Validation
            if (battleInfo.battlePlanet == null)
            {
                Debug.LogError("ERROR GUIBattleHandler GetDefendingFaction(): Provided battleInfo has no battle planet.");
                return null;
            }

            if (battleInfo.battlePlanet.FactionHandler == null)
            {
                Debug.LogError("ERROR GUIBattleHandler GetDefendingFaction(): Provided battle planet has no faction handler.");
                return null;
            }

            if (battleInfo.battlePlanet.FactionHandler.myFaction == null)
            {
                Debug.LogError("ERROR GUIBattleHandler GetDefendingFaction(): Provided battle planet's faction handler is missing a faction.");
                return null;
            }
            #endregion

            if (battleInfo.battleType == Battle.BattleType.GroundBattle)
            {
                return battleInfo.battlePlanet.FactionHandler.myFaction;
            }

            return battleInfo.defendingFleet.FactionHandler.myFaction;
        }
        #endregion

        public void AttackerWon()
        {
            BattleManager.Instance.AttackerWon();
            HideDialogBox();
        }

        public void DefenderWon()
        {
            BattleManager.Instance.DefenderWon();
            HideDialogBox();
        }

        private void StartShowTween()
        {
            gameObject.SetActive(true);
            battleHandlerTransform.localScale = Vector2.zero;
            battleHandlerTransform.DOScale(1, secsToTweenScale).SetEase(showBoxEasing);
        }

        public void HideDialogBox()
        {
            battleHandlerTransform.DOScale(0, secsToTweenScale).SetEase(hideBoxEasing).OnComplete(
                () => gameObject.SetActive(false)       //Set inactive when tween is completed
            );
        }
    }
}
