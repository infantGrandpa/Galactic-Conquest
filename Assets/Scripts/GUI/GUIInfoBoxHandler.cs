using System;
using System.Collections.Generic;
using Abraham.GalacticConquest.ActionPoints;
using TMPro;
using UnityEngine;

namespace Abraham.GalacticConquest.GUI
{
    public class GUIInfoBoxHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descText;

        [Header("AP Entries")]
        [SerializeField] private GameObject apEntryPrefab;
        [SerializeField] private Transform apListParent;
        [SerializeField, Tooltip("The number of list entries created on Awake")] private int startingListEntries = 3;
        [SerializeField] private List<GUIActionPointEntry> apEntries;


        private void Awake()
        {
            HideInfoBox();
            for (int i = 0; i < startingListEntries; i++)
            {
                CreateNewApEntry();
            }
        }

        public void ShowInfoBox(GameObject target)
        {
            GetGenericInfo(target);
            GetActionPointInfo(target);

            gameObject.SetActive(true);
        }

        private void GetGenericInfo(GameObject target)
        {
            GenericInfo targetInfo = target.GetComponent<GenericInfo>();
            if (!targetInfo) {
                Debug.LogWarning("GUIInfoBoxHandler ShowInfoBox(): Target " + target.name + " does not have generic info.", this);
                titleText.text = "Unknown Name";
                descText.text = "";
                return;
            }

            titleText.text = targetInfo.myName;
            descText.text = targetInfo.myDesc;
        }

        private void GetActionPointInfo(GameObject target)
        {
            ActionPointAggregator actionPointAggregator = target.GetComponent<ActionPointAggregator>();
            if (actionPointAggregator == null) {
                Debug.LogWarning("GUIInfoBoxHandler GetActionPointInfo(): Target " + target.name + " does not have an action point modifier.", this);
                return;
            }
            
            List<ActionPointModifier> modifiers = actionPointAggregator.APModifiers;

            int listLength = Math.Max(modifiers.Count, apEntries.Count);
            
            for (int i = 0; i < listLength; i++)
            {
                GUIActionPointEntry thisEntry = i >= apEntries.Count ? CreateNewApEntry() : apEntries[i];
                
                if (i >= modifiers.Count)
                {
                    thisEntry.gameObject.SetActive(false);
                    continue;
                }
                
                ActionPointModifier thisModifier = modifiers[i];

                thisEntry.gameObject.SetActive(true);
                thisEntry.UpdateApEntry(thisModifier.apModificationReason, thisModifier.apModificationValue);
            }
        }

        private GUIActionPointEntry CreateNewApEntry()
        {
            GameObject newEntryObject = Instantiate(apEntryPrefab, apListParent);
            GUIActionPointEntry entry = newEntryObject.GetComponent<GUIActionPointEntry>();
            if (!entry)
            {
                throw new MissingComponentException($"Prefab {apEntryPrefab.name} is missing a GUIActionPointEntry component.");
            }
            
            apEntries.Add(entry);
            return entry;
        }

        public void HideInfoBox()
        {
            gameObject.SetActive(false);
        }
    }
}
