using System.Collections.Generic;
using UnityEngine;

namespace Abraham.GalacticConquest.Traits
{
    public class TraitHandler : MonoBehaviour
    {
        public List<Trait> traits = new List<Trait>();

        private void Awake()
        {
            SortTraits();
            ApplyTraits();
        }

        private void SortTraits()
        {
            traits.Sort((trait1, trait2) => trait2.traitOrder.CompareTo(trait1.traitOrder));
        }

        private void ApplyTraits()
        {
            foreach (Trait thisTrait in traits)
            {
                thisTrait.ApplyTrait(gameObject);
            }
        }

        public bool CanBuildShips()
        {
            foreach (Trait thisTrait in traits)
            {
                if (thisTrait.traitAspects.Contains(TraitAspect.CanBuildShips))
                {
                    return true;
                }
            }

            return false;
        }

        public Trait GetTraitWithHighestImportance()
        {
            float maxImportanceSoFar = Mathf.NegativeInfinity;
            Trait mostImportantTrait = null;
            foreach (Trait thisTrait in traits)
            {
                if (thisTrait.traitOrder <= maxImportanceSoFar)
                {
                    continue;
                }

                maxImportanceSoFar = thisTrait.traitOrder;
                mostImportantTrait = thisTrait;
            }

            return mostImportantTrait;
        }
    }
}