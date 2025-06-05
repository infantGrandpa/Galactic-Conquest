using TMPro;
using UnityEngine;

namespace Abraham.GalacticConquest.GUI
{
    public class GUIMovementCostIndicator : MonoBehaviour
    {
        [SerializeField] private TMP_Text costText;
        public void UpdateMovementCost(int newCost)
        {
            ShowMovementCost();
            costText.text = newCost + " AP";
        }

        public void HideMovementCost()
        {
            gameObject.SetActive(false);
        }

        public void ShowMovementCost()
        {
            gameObject.SetActive(true);
        }
    }
}
