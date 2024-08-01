using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class GUIActionLogHandler : MonoBehaviour
    {
        [SerializeField] TMP_Text actionLogText;
        [SerializeField] int maxLogMessages = 5;

        private Queue<string> logMessages = new();

        public void AddLogMessage(string message)
        {
            logMessages.Enqueue(message);

            if (logMessages.Count > maxLogMessages)
            {
                logMessages.Dequeue();
            }

            UpdateLogText();
        }

        private void UpdateLogText()
        {
            actionLogText.text = string.Join("\n", logMessages.ToArray());
        }

        [ContextMenu("Test Action Log")]
        private void TestActionLog()
        {
            List<string> testMessages = new();

            testMessages.Add("This is a test.");
            testMessages.Add("Lorem ipsum dolor sit amet, consectetur adipiscing elit.");
            testMessages.Add("Mauris sed lectus quis mi interdum elementum vel vitae enim.");
            testMessages.Add("Test action added.");
            testMessages.Add("New action!");

            int newMessageIndex = Mathf.RoundToInt(Random.Range(0, testMessages.Count));
            string newMessage = testMessages[newMessageIndex];

            AddLogMessage(newMessage);
        }
    }
}
