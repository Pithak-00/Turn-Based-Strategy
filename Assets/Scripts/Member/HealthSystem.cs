using UnityEngine;
using UniRx;

namespace Member
{
    public class HealthSystem : MonoBehaviour
    {
        public ISubject<Unit> OnDead = new Subject<Unit>();
        public ISubject<int> OnDamaged = new Subject<int>();
        public ISubject<int> OnHealed = new Subject<int>();

        [SerializeField] private int health = 100;
        private int healthMax;

        private void Awake()
        {
            healthMax = health;
        }

        public void Damage(int damageAmount)
        {
            health -= damageAmount;

            if (health < 0)
            {
                health = 0;
            }

            OnDamaged.OnNext(damageAmount);

            if (health == 0)
            {
                Die();
            }
        }

        public void Heal(int healAmount)
        {
            health += healAmount;

            if (health > healthMax)
            {
                health = healthMax;
            }

            OnHealed.OnNext(healAmount);
        }

        private void Die()
        {
            OnDead.OnNext(Unit.Default);
        }

        public float GetHealthNormalized()
        {
            return (float)health / healthMax;
        }
    }
}