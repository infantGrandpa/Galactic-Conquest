using System;
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

        [Tooltip("The general type of this object.")]
        public string typeDescriptor;

        void Awake()
        {
            BuildGameObjectName();
        }

        private void BuildGameObjectName()
        {
            if (string.IsNullOrEmpty(myName) && string.IsNullOrEmpty(typeDescriptor)) {
                return;
            }

            string separator = " - ";
            if (string.IsNullOrEmpty(myName) || string.IsNullOrEmpty(typeDescriptor)) {
                separator = "";
            }

            string gameObjectName = typeDescriptor + separator + myName;
            gameObject.name = gameObjectName;
        }
    }
}
