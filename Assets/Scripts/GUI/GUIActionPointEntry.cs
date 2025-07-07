using TMPro;
using UnityEngine;

namespace Abraham.GalacticConquest.GUI
{
    public class GUIActionPointEntry : MonoBehaviour
    {
        [SerializeField] private TMP_Text apText;
        [SerializeField] private TMP_Text apValue;

        public void UpdateApEntry(string text, int value)
        {
            apText.text = text;
            apValue.text = GUIManager.ConvertAPIntToString(value); 
        }
    }
}
