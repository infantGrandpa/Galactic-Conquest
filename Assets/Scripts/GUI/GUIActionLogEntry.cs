using TMPro;
using UnityEngine;

namespace Abraham.GalacticConquest.GUI
{
    public class GUIActionLogEntry : MonoBehaviour
    {
        [SerializeField] private TMP_Text entryText;
        [SerializeField] private TMP_Text apChangeText;

        public void ClearEntry()
        {
            SetEntryText("");
            SetAPChangeText(null);
        }
        
        public void SetLogEntryValues(string message, int? apValue = null)
        {
            SetEntryText(message);
            SetAPChangeText(apValue);
        }

        private void SetEntryText(string message)
        {
            entryText.text = message;
        }

        private void SetAPChangeText(int? apValue)
        {
            if (apValue == null)
            {
                apChangeText.text = "";
                return;
            }
            
            string apString = GUIManager.ConvertAPIntToString(apValue.Value);
            apChangeText.text = apString;
        }

        
    }
}