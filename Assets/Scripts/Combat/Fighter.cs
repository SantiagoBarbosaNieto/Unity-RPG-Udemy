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

        float timeSinceLastAttack = Mathf.Infinity; 

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
                GetComponent<Mover>().StartMoveAction(target.transform.position);
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

        public void SetAttackTarget(GameObject combatTarget)
        {
            if(combatTarget == null)   
                target = null;
            else
                target = combatTarget.GetComponent<Health>();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead;
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