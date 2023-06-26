using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        
        [SerializeField] float waypointGizmoRadius = 0.3f;
        void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            DrawWaypoints();
        }

        void OnDrawGizmosSelected()
        {
            //Change color to orange
            Gizmos.color = Color.yellow;
            DrawWaypoints();
        }

        void DrawWaypoints()
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetNextIndex(i)));
            }

        }

        public int GetNextIndex(int i)
        {

            return (i + 1) % transform.childCount;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
