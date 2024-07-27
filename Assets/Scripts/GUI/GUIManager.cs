using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class GUIManager : MonoBehaviour
    {
        public static GUIManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType(typeof(GUIManager)) as GUIManager;

                return instance;
            }
            set
            {
                instance = value;
            }
        }
        private static GUIManager instance;

        [SerializeField] GUITurnHandler turnHandler;

        public void ChangeTurn(string turnString)
        {
            turnHandler.ChangeTurn(turnString);
        }
    }
}
