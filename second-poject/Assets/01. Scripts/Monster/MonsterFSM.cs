    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterFSM : MonoBehaviour
{
   
    public float atkRange = 0.15f;
    protected NavMeshAgent agent;
    private FOV fov;
    public Transform target => fov.FirstTarget;
    public LayerMask targetLayerMask => fov.targetLayerMask;
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = true;

        fov = GetComponent<FOV>();
    }
    protected virtual void Update()
    {

    }
    private void OnAnimatorMove()
    {

        // Follow CharacterController
        Vector3 pos = transform.position;
        pos.y = agent.nextPosition.y;

        agent.nextPosition = pos;

    }
    void lookAtTarget()
    {
        if (target)
        {
            Vector3 lookAt = (target.position - target.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(lookAt.x, 0, lookAt.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}
