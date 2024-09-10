using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class GenericInfo : MonoBehaviour
    {
        //This doesn't need to be a MonoBehaviour, but I decided to do this so I don't need to rely on other classes
        //The other ways I could think of doing this:
        //1. Use ScriptableObjects to hold data (which seemed excessive)
        //2. Store this as just a regular class inside MonoBehaviours.
        //Then, to get data from the GenericInfo class, I would have to check each MonoBehaviour that could hold the class.
        //That sucks. So I did this instead.  

        public string myName;
        [TextArea(4, 10)] public string myDesc;
    }
}
