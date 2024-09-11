using System.Collections.Generic;
using Abraham.GalacticConquest.ActionPoints;
using Sirenix.OdinInspector;
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
        [Tooltip("Dictates the order to apply traits. Higher numbers = higher importance.")] public int traitOrder = 0;

        [ListDrawerSettings(ShowFoldout = true)]
        public List<TraitAspect> traitAspects = new List<TraitAspect>();

        [ShowIf("HasActionPointModifierAspect")]
        public int actionPointModifier = 0;

        public Sprite traitIcon;

        public void ApplyTrait(GameObject target)
        {
            foreach (TraitAspect thisTraitAspect in traitAspects) {
                switch (thisTraitAspect) {
                case TraitAspect.ActionPointModifier:
                    ApplyActionPointModifier(target);
                    break;
                case TraitAspect.CanBuildShips:
                    ApplyBuildShipsTrait(target);
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

        void ApplyBuildShipsTrait(GameObject target)
        {
            ShipyardBehaviour shipyardBehaviour = target.GetComponent<ShipyardBehaviour>();
            if (shipyardBehaviour != null) {
                Debug.LogWarning("Trait ApplyBuildShipsTrait(): Target " + target.name + " already has a shipyard behaviour.", this);
                return;
            }

            target.AddComponent<ShipyardBehaviour>();
        }

        private bool HasActionPointModifierAspect()
        {
            return traitAspects.Contains(TraitAspect.ActionPointModifier);
        }
    }
}
