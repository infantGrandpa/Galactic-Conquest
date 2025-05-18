using TMPro;
using UnityEngine;

namespace Abraham.GalacticConquest.GUI
{
    public class GUIMovementCostIndicator : MonoBehaviour
    {
        [SerializeField] private TMP_Text costText;
        public void UpdateMovementCost(int newCost)
        {
            costText.text = newCost + " AP";
        }
    }
}
