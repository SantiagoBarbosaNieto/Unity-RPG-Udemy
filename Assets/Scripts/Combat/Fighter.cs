using UnityEngine;

using RPG.Core;
using RPG.Movement;

namespace RPG.Combat
{
    public class Fighter: MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float attackSpeed = 1f;
        [SerializeField] float weaponDamage = 5f;

        Transform target = null;

        float timeSinceLastAttack = 0f; 
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if(target == null) return;
        
            bool isInRange = Vector3.Distance(transform.position, target.position) < weaponRange;
            if(!isInRange)
            {
                GetComponent<Mover>().MoveTarget(target.position);
            }
            else
            {
                GetComponent<Mover>().Stop();
                GetComponent<ActionScheduler>().StartAction(this);
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            float timeBetweenAttacks = 1/attackSpeed;
            if(timeSinceLastAttack < timeBetweenAttacks) return;
            timeSinceLastAttack = 0f;
            GetComponent<Animator>().SetTrigger("attack");
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;
            Debug.Log("Attack!");
        }

        public void Cancel()
        {
            target = null;
        }
        
        //Animation Event
        void Hit()
        {
            if(target == null) return;
            if(target.GetComponent<Health>() == null) return;   
            target.GetComponent<Health>().TakeDamage(weaponDamage);
        }
    }
}