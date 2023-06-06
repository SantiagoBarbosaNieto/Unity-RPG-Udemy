using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float MaxHealth = 100f;
        float CurrentHealth = 0f;

        public void Start()
        {
            CurrentHealth = MaxHealth;
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
            Debug.Log(CurrentHealth);  
        }
    }
}