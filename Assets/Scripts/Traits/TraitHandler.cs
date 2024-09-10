using System;
using System.Collections.Generic;
using UnityEngine;

namespace Abraham.GalacticConquest.Traits
{
    public class TraitHandler : MonoBehaviour
    {
        public List<Trait> traits = new List<Trait>();

        void Start()
        {
            ApplyTraits();
        }

        void ApplyTraits()
        {
            foreach (Trait thisTrait in traits) {
                thisTrait.ApplyTrait(gameObject);
            }
        }
    }
}
