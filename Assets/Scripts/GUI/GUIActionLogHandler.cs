using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Abraham.GalacticConquest.GUI
{
    public class GUIActionLogHandler : MonoBehaviour
    {
        [SerializeField] GameObject actionLogPrefab;
        [SerializeField] List<TMP_Text> actionLogTextObjects = new();
        [SerializeField] Transform actionLogContainer;
        [SerializeField] int maxLogMessages = 5;

        [Header("Tweening")]
        [SerializeField] float secsToTween = 0.5f;

        private Queue<string> logMessages = new();

        private void Awake()
        {
            for (int i = 0; i < maxLogMessages; i++)
            {
                GameObject newLogObject = Instantiate(actionLogPrefab, actionLogContainer);
                if (newLogObject.TryGetComponent(out TMP_Text newLogText))
                {
                    actionLogTextObjects.Add(newLogText);
                    newLogText.text = "";
                    newLogObject.name = "Log Object " + i;
                }
            }
        }

        private void Start()
        {
            AddLogMessage("Game Started.");
        }

        public void AddLogMessage(string message)
        {
            logMessages.Enqueue(message);

            if (logMessages.Count > maxLogMessages)
            {
                logMessages.Dequeue();
            }

            UpdateLogText(message);
        }

        private void UpdateLogText(string message)
        {
            //Get first child (first will be the highest one)
            TMP_Text logToUpdate = actionLogTextObjects[0];

            //logToUpdate.text = message;

            logToUpdate.transform.SetAsLastSibling();
            TweenLogText(logToUpdate, message);

            MoveListObjectToEnd(0);
        }

        private void TweenLogText(TMP_Text textToTween, string message)
        {
            textToTween.text = "";
            textToTween.DOText(message, secsToTween, false);
        }

        [ContextMenu("Test Action Log")]
        public void TestActionLog()
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

        private void MoveListObjectToEnd(int indexToMove)
        {
            TMP_Text itemToMove = actionLogTextObjects[indexToMove];
            actionLogTextObjects.RemoveAt(indexToMove);
            actionLogTextObjects.Add(itemToMove);
        }
    }
}
