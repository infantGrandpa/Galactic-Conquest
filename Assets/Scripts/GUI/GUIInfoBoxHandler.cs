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
            GetGenericInfo(target);
            GetTraitInfo(target);
            GetActionPointInfo(target);

            gameObject.SetActive(true);
        }
        void GetGenericInfo(GameObject target)
        {
            GenericInfo targetInfo = target.GetComponent<GenericInfo>();
            if (targetInfo == null) {
                Debug.LogWarning("GUIInfoBoxHandler ShowInfoBox(): Target " + target.name + " does not have generic info.", this);
                titleText.text = "Unknown Name";
                descText.text = "";
                return;
            }

            titleText.text = targetInfo.myName;
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
            ActionPointModifier actionPointModifier = target.GetComponent<ActionPointModifier>();
            if (actionPointModifier == null) {
                Debug.LogWarning("GUIInfoBoxHandler GetActionPointInfo(): Target " + target.name + " does not have an action point modifier.", this);
                return;
            }

            int apValue = actionPointModifier.TotalApPerTurn;
            //Add plus sign if the apValue positive; minus is always shown
            string apString = apValue < 0 ? apValue.ToString() : "+" + apValue;     

            apPerTurnText.text = apString;
        }

        public void HideInfoBox()
        {
            gameObject.SetActive(false);
        }
    }
}
