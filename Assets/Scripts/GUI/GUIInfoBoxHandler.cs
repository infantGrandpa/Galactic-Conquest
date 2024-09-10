using System;
using Abraham.GalacticConquest.ActionPoints;
using Abraham.GalacticConquest.Traits;
using TMPro;
using UnityEngine;

namespace Abraham.GalacticConquest.GUI
{
    public class GUIInfoBoxHandler : MonoBehaviour
    {
        [SerializeField] TMP_Text titleText;
        [SerializeField] TMP_Text descText;
        [SerializeField] TMP_Text apPerTurnText;

        void Awake()
        {
            HideInfoBox();
        }

        public void ShowInfoBox(GameObject target)
        {
            GetTraitInfo(target);
            GetActionPointInfo(target);

            gameObject.SetActive(true);
        }

        void GetTraitInfo(GameObject target)
        {
            TraitHandler targetTraitHandler = target.GetComponent<TraitHandler>();
            if (targetTraitHandler == null) {
                Debug.LogWarning("GUIInfoBoxHandler ShowInfoBox(): Target " + target.name + " does not have a trait handler.", this);
                return;
            }

            string testString = "";

            foreach (Trait thisTrait in targetTraitHandler.traits) {
                testString += thisTrait.traitName + "\n";
            }

            descText.text = testString;
        }

        void GetActionPointInfo(GameObject target)
        {
            ActionPointAdjuster actionPointAdjuster = target.GetComponent<ActionPointAdjuster>();
            if (actionPointAdjuster == null) {
                Debug.LogWarning("GUIInfoBoxHandler GetActionPointInfo(): Target " + target.name + " does not have an action point adjuster.", this);
                return;
            }

            int apValue = actionPointAdjuster.TotalApPerTurn;
            string apString = apValue < 0 ? apValue.ToString() : "+" + apValue.ToString();

            apPerTurnText.text = apString;
        }

        public void HideInfoBox()
        {
            gameObject.SetActive(false);
        }
    }
}
