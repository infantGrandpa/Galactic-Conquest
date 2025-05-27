using Abraham.GalacticConquest.Combat;
using Abraham.GalacticConquest.Factions;
using Abraham.GalacticConquest.GUI;
using UnityEngine;

namespace Abraham.GalacticConquest.Planets
{
    public class PlanetCombatBehaviour : CombatantBehaviour
    {
        private HealthSystem _healthSystem;
        private CombatantBehaviour _invader;

        private void Awake()
        {
            _healthSystem = GetComponent<HealthSystem>();
        }

        public void ResetPlanetAfterCapture()
        {
            _healthSystem.HealFully();
            _invader = null;
        }

        public void PrepareForInvasion(CombatantBehaviour newInvader)
        {
            this._invader = newInvader;
        }

        public Faction GetInvaderFaction()
        {
            if (_invader == null)
            {
                Debug.LogError("ERROR PlanetCombatBehaviour GetInvaderFaction(): Invader is null.", this);
                return null;
            }

            if (!_invader.TryGetComponent(out FactionHandler invaderFactionHandler))
            {
                Debug.LogError("ERROR PlanetCombatBehaviour GetInvaderFaction(): Invader doesn't have a FactionHandler component.", this);
                return null;
            }

            return invaderFactionHandler.myFaction;
        }
    }
}
