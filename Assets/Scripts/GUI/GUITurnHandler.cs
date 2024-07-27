using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Abraham.GalacticConquest
{
    public class GUITurnHandler : MonoBehaviour
    {
        [SerializeField] Image turnBackground;
        [SerializeField] TMP_Text turnText;

        public void ChangeTurn(string turnString)
        {
            if (turnText == null)
            {
                Debug.LogError("ERROR GUITurnHandler ChangeTurn(): turnText is null.", this);
                return;
            }

            turnText.text = turnString;
        }
    }
}
