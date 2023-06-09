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
        
        float timeBetweenAttacks = 1;

        Health target = null;

        float timeSinceLastAttack = 0f; 

        private void Awake()
        {
            timeBetweenAttacks = 1/attackSpeed;
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if(target == null || target.IsDead) return;
        
            bool isInRange = Vector3.Distance(transform.position, target.transform.position) < weaponRange;
            if(!isInRange)
            {
                GetComponent<Mover>().MoveTarget(target.transform.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                GetComponent<ActionScheduler>().StartAction(this);
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if(timeSinceLastAttack < timeBetweenAttacks) return;
            timeSinceLastAttack = 0f;
            GetComponent<Animator>().ResetTrigger("stopAttacking");
            GetComponent<Animator>().SetTrigger("attack");
        }

        public void SetAttackTarget(CombatTarget combatTarget)
        {
            target = combatTarget.GetComponent<Health>();
            Debug.Log("Attack!");
        }

        public void Cancel()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttacking");
            timeSinceLastAttack = timeBetweenAttacks;
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