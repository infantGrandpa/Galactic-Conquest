using System;
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
            gameObject.SetActive(true);
            Debug.Log("Showing Info box for " + target.name, this);
        }

        public void HideInfoBox()
        {
            gameObject.SetActive(false);
        }
    }
}
