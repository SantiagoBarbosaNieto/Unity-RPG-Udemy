
using UnityEngine;

//Project namespaces Dependencies
using RPG.Movement;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(Fighter))]
    [RequireComponent(typeof(Health))]
    public class PlayerController : MonoBehaviour
    {

        //Add a precompiler directive to tell if we are in the editor or not
        #if UNITY_EDITOR
            bool isEditor = true;
        #else
            bool isEditor = false;
        #endif


        // Update is called once per frame
        void Update()
        {
            if(GetComponent<Health>() == null || GetComponent<Health>().IsDead) return;
            if(InteractCombat()) return;
            if(InteractMovement()) return;

            //Debug.Log("Nothing to do");
        }

        private bool InteractCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach(RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if(target == null) continue;
                if(Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().SetAttackTarget(target.gameObject);
                }
                return true;
            }
            return false;

        }

        private bool InteractMovement()
        {
            
            Ray ray = GetMouseRay();
            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach(RaycastHit hit in hits)
            { 
                if (hit.transform.gameObject == gameObject) continue;
                if(Input.GetMouseButton(0))
                    MovePlayerTarget(hit.point); 

                if(isEditor)
                {
                    Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 0.01f, false);
                }
                 
                return true;
            }
            return false;
        }

        private void MovePlayerTarget(Vector3 point)
        {
            GetComponent<Mover>().StartMoveAction(point, 1f);
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

}