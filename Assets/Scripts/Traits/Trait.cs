using System;
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
        RequiredToWin,
        Fortified
    }

    [CreateAssetMenu(fileName = "NewTrait", menuName = "Game/Trait")]
    public class Trait : ScriptableObject
    {
        public string traitName;
        public string traitDesc;

        [Tooltip("Dictates the order to apply traits. Higher numbers = higher importance.")]
        public int traitOrder = 0;

        [ListDrawerSettings(ShowFoldout = true)]
        public List<TraitAspect> traitAspects = new();

        [ShowIf("HasActionPointModifierAspect")]
        public int actionPointModifier = 0;

        public Sprite traitIcon;

        public void ApplyTrait(GameObject target)
        {
            foreach (TraitAspect thisTraitAspect in traitAspects)
            {
                switch (thisTraitAspect)
                {
                    case TraitAspect.ActionPointModifier:
                        ApplyActionPointModifiers(target);
                        break;
                    case TraitAspect.CanBuildShips:
                        ApplyBuildShipsTraitAspect(target);
                        break;
                    case TraitAspect.Fortified:
                        ApplyFortifiedTraitAspect(target);
                        break;
                    case TraitAspect.RequiredToWin:
                        ApplyRequiredToWinTraitAspect(target);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void ApplyActionPointModifiers(GameObject target)
        {
            ActionPointAggregator apAggregator = target.GetComponent<ActionPointAggregator>();
            if (apAggregator == null)
            {
                apAggregator = target.AddComponent<ActionPointAggregator>();
            }

            apAggregator.AddApModifier(traitName, actionPointModifier);
        }

        private void ApplyBuildShipsTraitAspect(GameObject target)
        {
            ShipyardBehaviour shipyardBehaviour = target.GetComponent<ShipyardBehaviour>();
            if (shipyardBehaviour != null)
            {
                Debug.LogWarning("Trait ApplyBuildShipsTrait(): Target " + target.name + " already has a shipyard behaviour.", this);
                return;
            }

            target.AddComponent<ShipyardBehaviour>();
        }

        private void ApplyFortifiedTraitAspect(GameObject target)
        {
            // As of right now, we do nothing. 
            // The fortified trait only changes things in a SWBFII game, not in our game
        }

        private void ApplyRequiredToWinTraitAspect(GameObject target)
        {
            // TODO: Implement Required to Win Trait
        }

        private bool HasActionPointModifierAspect()
        {
            return traitAspects.Contains(TraitAspect.ActionPointModifier);
        }
    }
}