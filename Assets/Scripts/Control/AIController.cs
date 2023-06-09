using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Combat;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;

        private Vector3 lastKnownTargetLocation;

        private Fighter fighter;
        private GameObject player;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
        }  

        private void Update()
        {  
            if(inAttackRange() && fighter.CanAttack(player))
            {
                fighter.SetAttackTarget(player);
                lastKnownTargetLocation = player.transform.position;
            }
            else if (lastKnownTargetLocation != null)
            {
                fighter.Cancel();
            }
        }

        public float distanceToPlayer()
        {         
            return Vector3.Distance(player.transform.position, transform.position);;
        }

        public bool inAttackRange()
        {
            return distanceToPlayer() <= chaseDistance;
        }
    }
}
