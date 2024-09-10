using System.Collections.Generic;
using Abraham.GalacticConquest.ActionPoints;
using UnityEngine;

namespace Abraham.GalacticConquest.Traits
{
    public enum TraitAspect
    {
        ActionPointModifier,
        CanBuildShips,
        RequiredToWin
    }

    [CreateAssetMenu(fileName = "NewTrait", menuName = "Game/Trait")]
    public class Trait : ScriptableObject
    {
        public string traitName;
        public string traitDesc;

        public List<TraitAspect> traitAspects = new List<TraitAspect>();
        public int actionPointModifier = 0;

        public void ApplyTrait(GameObject target)
        {
            foreach (TraitAspect thisTraitAspect in traitAspects) {
                switch (thisTraitAspect) {
                case TraitAspect.ActionPointModifier:
                    ApplyActionPointModifier(target);
                    break;
                case TraitAspect.CanBuildShips:
                    Debug.Log("Can Build Ships Trait.", this);
                    break;
                case TraitAspect.RequiredToWin:
                    Debug.Log("Required to Win Trait.", this);
                    break;

                }

            }
        }

        private void ApplyActionPointModifier(GameObject target)
        {
            ActionPointAdjuster apAdjuster = target.GetComponent<ActionPointAdjuster>();
            if (apAdjuster == null) {
                apAdjuster = target.AddComponent<ActionPointAdjuster>();
            }

            apAdjuster.AddApAdjustment(actionPointModifier, traitName);
        }
    }
}
