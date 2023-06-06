using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
{

    [SerializeField] Transform target;

    NavMeshAgent navMeshAgent;

    Vector3 targetPoint;
    

//Add a precompiler directive to tell if we are in the editor or not
#if UNITY_EDITOR
    bool isEditor = true;
#else
    bool isEditor = false;
#endif


    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
            Debug.LogError("NavMeshAgent not found on " + gameObject.name);

        if(target != null){
            target.Find("Mesh").gameObject.SetActive(isEditor);
            target.position = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTarget();
        UpdateAnimator();
    }

    public void StartMoveAction(Vector3 point)
    {
        GetComponent<ActionScheduler>().StartAction(this);
        MoveTarget(point);
    }

    public void MoveTarget(Vector3 point)
    {
        targetPoint = point;
        navMeshAgent.isStopped = false;

    }

    public void Stop()
    {
        navMeshAgent.isStopped = true;
    }

    public void Cancel()
    {
        Stop();
    }

    private void UpdateTarget()
    {
        if(target != null)
            target.position = targetPoint;
        navMeshAgent.SetDestination(targetPoint);
    }

    private void UpdateAnimator()
    {
        Vector3 Velocity = GetComponent<NavMeshAgent>().velocity;
        Vector3 LocalVelocity = transform.InverseTransformDirection(Velocity);
        float speed = LocalVelocity.z;
        GetComponent<Animator>().SetFloat("forwardSpeed", speed);
    }
}

}
