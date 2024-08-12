using TMPro;
using UnityEngine;

namespace Abraham.GalacticConquest.GUI
{
    public class GUIActionPointHandler : MonoBehaviour
    {
        [SerializeField] TMP_Text actionPointText;

        public void UpdateActionPoints(int newValue)
        {
            actionPointText.text = newValue.ToString();
        }

        public void IncreaseAPOnClick()
        {
            ActionPointManager.Instance.IncreaseActionPoints(1);
        }

        public void DecreaseAPOnClick()
        {
            ActionPointManager.Instance.DecreaseActionPoints(1);
        }
    }
}
