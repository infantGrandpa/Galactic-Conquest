using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Abraham.GalacticConquest
{
    public class HealthSystem : MonoBehaviour, IDamageable
    {
        [Header("Health")]
        [SerializeField] int maxHealth;
        [ShowInInspector, ReadOnly] public float CurrentHealth { get; private set; }
        [SerializeField] bool destroyOn0Health = true;
        public bool isInvulnerable;

        [Header("Events")]
        [SerializeField] UnityEvent onTakeDamageEvent;
        [SerializeField] UnityEvent onDeathEvent;


        private void Start()
        {
            CurrentHealth = maxHealth;
        }

        public void TakeDamage(int damageTaken)
        {
            if (isInvulnerable)
            {
                return;
            }

            CurrentHealth -= damageTaken;

            onTakeDamageEvent?.Invoke();

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            onDeathEvent?.Invoke();

            if (destroyOn0Health)
            {
                Destroy(gameObject);
            }
        }
    }
}
