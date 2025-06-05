using UnityEngine;

namespace Abraham.GalacticConquest.Traits
{
    public class TraitManager : MonoBehaviour
    {
        public static TraitManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType(typeof(TraitManager)) as TraitManager;

                return _instance;
            }
            set { _instance = value; }
        }
        
        private static TraitManager _instance;
        
        // As of right now, this is literally just to hold a reference to the Fortified Trait scriptable object.
        // Because this is a scriptable object, it's difficult to get references in code without using the editor.
        // The easiest fix I could think of was this--hopefully we'll have another use for it in the future.
        public Trait fortifiedTrait;
    }
}
