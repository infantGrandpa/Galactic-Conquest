using Abraham.GalacticConquest.Traits;
using Sirenix.OdinInspector;
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

        private TraitHandler _traitHandler;

        public string myName;
        [TextArea(4, 10)] public string myDesc;

        [Tooltip("The general type of this object.")]
        public string typeDescriptor = "Planet";

        [Button("Rebuild Planet Name")]
        private void RenameGameObject()
        {
            string planetTypeDesc = GetPlanetTypeDesc();

            if (string.IsNullOrEmpty(myName) && string.IsNullOrEmpty(planetTypeDesc))
            {
                return;
            }

            string separator = " - ";
            if (string.IsNullOrEmpty(myName) || string.IsNullOrEmpty(planetTypeDesc))
            {
                separator = "";
            }

            string gameObjectName = planetTypeDesc + separator + myName;
            gameObject.name = gameObjectName;
        }

        private string GetPlanetTypeDesc()
        {
            string planetType = typeDescriptor;
            if (!TryGetComponent(out _traitHandler))
            {
                return planetType;
            }

            Trait mostImportantTrait = _traitHandler.GetTraitWithHighestImportance();
            if (!mostImportantTrait)
            {
                return planetType;
            }

            return mostImportantTrait.traitName;
        }
    }
}