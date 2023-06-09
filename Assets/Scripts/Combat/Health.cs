using UnityEngine;
using RPG.Core;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float MaxHealth = 10f;
        float CurrentHealth = 0f;

        private bool isDead = false;

        //Expose variables inline
        public bool IsDead { get { return isDead; } }


        public void Start()
        {
            CurrentHealth = MaxHealth;
            
        }


        public void TakeDamage(float damage)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
            Debug.Log(CurrentHealth);  
            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("death");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<CapsuleCollider>().enabled = false;
        }
    }
}