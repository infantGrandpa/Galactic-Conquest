using System.Collections.Generic;
using UnityEngine;

namespace Abraham.GalacticConquest.Traits
{
    public class TraitHandler : MonoBehaviour
    {
        public List<Trait> traits = new List<Trait>();

        void Start()
        {
            SortTraits();
            ApplyTraits();
        }

        void SortTraits()
        {
            traits.Sort((trait1, trait2) => trait2.traitOrder.CompareTo(trait1.traitOrder));
        }

        void ApplyTraits()
        {
            foreach (Trait thisTrait in traits) {
                thisTrait.ApplyTrait(gameObject);
            }
        }

        public bool CanBuildShips()
        {
            foreach (Trait thisTrait in traits) {
                if (thisTrait.traitAspects.Contains(TraitAspect.CanBuildShips)) {
                    return true;
                }
            }

            return false;
        }
    }
}
