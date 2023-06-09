using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Project namespaces Dependencies
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control
{
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

                if(Input.GetMouseButtonDown(0))
                {
                    GetComponent<Fighter>().SetAttackTarget(target);
                }
                return true;
            }
            return false;

        }

        private bool InteractMovement()
        {
            Ray ray = GetMouseRay();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);
            if(hasHit)
            {
                if(Input.GetMouseButton(0))
                    MovePlayerTarget(hit.point); 

                if(isEditor)
                {
                    Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 0.01f, true);
                }
                 
                return true;
            }
            return false;
        }

        private void MovePlayerTarget(Vector3 point)
        {
            GetComponent<Mover>().StartMoveAction(point);
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

}