using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class CombatantBehaviour : MonoBehaviour
    {
        [SerializeField] protected int damageToDeal;

        public void DamageTarget(IDamageable damageableTarget)
        {
            if (damageableTarget == null)
            {
                Debug.LogError("ERROR CombatBehaviour DamageTarget(): Provided damageableTarget is null.");
                return;
            }

            damageableTarget.TakeDamage(damageToDeal);
        }

        public void DamageTarget(CombatantBehaviour target)
        {
            if (target == null)
            {
                Debug.LogError("ERROR CombatBehaviour DamageTarget(): Target Fleet is null.");
                return;
            }

            if (!target.TryGetComponent(out HealthSystem targetHealth))
            {
                Debug.LogError("ERROR CombatBehaviour DamageTarget(): Target (" + target.gameObject.name + ") is missing an IDamagable component.");
                return;
            }

            DamageTarget(targetHealth);
        }
    }
}
