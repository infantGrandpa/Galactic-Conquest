using System.Collections.Generic;
using UnityEngine;

namespace Abraham.GalacticConquest.GUI
{
    public class GUIActionLogHandler : MonoBehaviour
    {
        [SerializeField] private GameObject actionLogPrefab;
        [SerializeField] private Transform actionLogContainer;
        [SerializeField] private int maxLogMessages = 5;

        private readonly List<GUIActionLogEntry> _actionLogEntries = new();

        private readonly Queue<string> _logMessages = new();

        private void Awake()
        {
            for (int i = 0; i < maxLogMessages; i++)
            {
                GameObject newLogObject = Instantiate(actionLogPrefab, actionLogContainer);
                GUIActionLogEntry newLogText = newLogObject.GetComponent<GUIActionLogEntry>();

                newLogObject.name = "Log Object " + i;
                newLogText.ClearEntry();

                _actionLogEntries.Add(newLogText);
            }
        }

        public void AddLogMessage(string message, int? apValue = null)
        {
            _logMessages.Enqueue(message);

            if (_logMessages.Count > maxLogMessages)
            {
                _logMessages.Dequeue();
            }

            UpdateLogText(message, apValue);
        }

        private void UpdateLogText(string message, int? apValue = null)
        {
            //Get first child (first will be the highest one)
            GUIActionLogEntry logToUpdate = _actionLogEntries[0];
            logToUpdate.SetLogEntryValues(message, apValue);

            logToUpdate.transform.SetAsLastSibling();
            MoveListObjectToEnd(0);
        }

        private void MoveListObjectToEnd(int indexToMove)
        {
            GUIActionLogEntry itemToMove = _actionLogEntries[indexToMove];
            _actionLogEntries.RemoveAt(indexToMove);
            _actionLogEntries.Add(itemToMove);
        }
    }
}