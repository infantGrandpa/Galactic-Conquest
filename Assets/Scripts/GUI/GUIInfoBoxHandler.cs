using System.Collections.Generic;
using Abraham.GalacticConquest.ActionPoints;
using Abraham.GalacticConquest.Traits;
using TMPro;
using UnityEngine;

namespace Abraham.GalacticConquest.GUI
{
    public class GUIInfoBoxHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descText;

        [SerializeField] private List<GUIActionPointEntry> apEntries;

        private void Awake()
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

        private void GetGenericInfo(GameObject target)
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

        private void GetTraitInfo(GameObject target)
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

        private void GetActionPointInfo(GameObject target)
        {
            ActionPointAggregator actionPointAggregator = target.GetComponent<ActionPointAggregator>();
            if (actionPointAggregator == null) {
                Debug.LogWarning("GUIInfoBoxHandler GetActionPointInfo(): Target " + target.name + " does not have an action point modifier.", this);
                return;
            }
            
            List<ActionPointModifier> modifiers = actionPointAggregator.APModifiers;
            for (int i = 0; i < modifiers.Count; i++)
            {
                ActionPointModifier thisModifier = modifiers[i];
                GUIActionPointEntry thisEntry = apEntries[i];
                thisEntry.UpdateApEntry(thisModifier.apModificationReason, thisModifier.apModificationValue);
            }
        }

        public void HideInfoBox()
        {
            gameObject.SetActive(false);
        }
    }
}
