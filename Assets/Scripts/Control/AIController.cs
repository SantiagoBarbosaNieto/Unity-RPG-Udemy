
using UnityEngine;

using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] float waypointTolerance = 0.5f;
        [SerializeField] float dwellTime = 3f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;


        [SerializeField] PatrolPath patrolPath;
        private Vector3 lastKnownTargetLocation;
        private Fighter fighter;
        private GameObject player;

        private float timeSinceLastSeenPlayer = Mathf.Infinity;
        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;


        Vector3 guardPosition;
        int currentWaypoint = 0;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            guardPosition = transform.position;
        }  

        private void Update()
        {  
            if(GetComponent<Health>() == null || GetComponent<Health>().IsDead) return;
            if(inAttackRange() && fighter.CanAttack(player))
            {
                // Debug.Log("Attacking player");
                AttackingBehaviour();
                timeSinceArrivedAtWaypoint = 0;
            }
            else if(timeSinceLastSeenPlayer <= suspicionTime && lastKnownTargetLocation != null)
            {
                // Debug.Log("SUSPICIOUS");
                SuspiciousBehaviour();
                timeSinceArrivedAtWaypoint = 0;
            }
            else
            {
                // Debug.Log("Returning to guard position");
                PatrolBehaviour();
            }
            timeSinceLastSeenPlayer += Time.deltaTime;
        }

        private void AttackingBehaviour()
        {
            fighter.SetAttackTarget(player);
            lastKnownTargetLocation = player.transform.position;
            timeSinceLastSeenPlayer = 0;
        }

        private void SuspiciousBehaviour()
        {
            fighter.SetAttackTarget(null);
            GetComponent<Mover>().StartMoveAction(lastKnownTargetLocation, 1f);
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;
            if(patrolPath != null)
            {
                if(AtWaypoint() )
                {
                    timeSinceArrivedAtWaypoint += Time.deltaTime;
                }
                if(timeSinceArrivedAtWaypoint > dwellTime)
                {
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            GetComponent<Mover>().StartMoveAction(nextPosition, patrolSpeedFraction);
        }

        private Vector3 GetCurrentWaypoint()
        {
            Vector3 newWaypoint = patrolPath.GetWaypoint(currentWaypoint);
            return newWaypoint;
        }

        private void CycleWaypoint()
        {
            currentWaypoint = patrolPath.GetNextIndex(currentWaypoint);
            timeSinceArrivedAtWaypoint = 0;
        }

        private bool AtWaypoint()
        {
            if(patrolPath == null) return true;
            float distanceToWaypoint = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), 
                                                        new Vector2(patrolPath.GetWaypoint(currentWaypoint).x, patrolPath.GetWaypoint(currentWaypoint).z));
            return distanceToWaypoint < waypointTolerance;
        }

        public float distanceToPlayer()
        {         
            return Vector3.Distance(player.transform.position, transform.position);;
        }

        public bool inAttackRange()
        {
            return distanceToPlayer() <= chaseDistance;
        }

        //Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
