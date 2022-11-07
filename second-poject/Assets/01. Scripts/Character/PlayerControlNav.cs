using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerControlNav : MonoBehaviour
{
    CharacterController characterController;
    NavMeshAgent agent;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true;
        agent.updatePosition = false;
    }


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                agent.SetDestination(hit.point);
            }
        }
        if (agent.remainingDistance < agent.remainingDistance)
        {
            characterController.Move(agent.velocity * Time.deltaTime);
        }
        else
        {
            characterController.Move(Vector3.zero);
        }
    }
    private void LateUpdate()
    {
        transform.position = agent.nextPosition;
    }

}
