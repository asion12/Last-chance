using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class stateRoaming : State<MonsterFSM>
{
    private CharacterController characterController;
    private NavMeshAgent agent;


    public override void OnAwake()
    {
        characterController = stateMachineClass.GetComponent<CharacterController>();
        agent = stateMachineClass.GetComponent<NavMeshAgent>();
    }

    public override void OnStart()
    {
        if (stateMachineClass?.posRoaming == null)
        {
            stateMachineClass?.getPositionNextRoaming();
        }
        if (stateMachineClass?.posRoaming != null)
        {
            Vector3 destination = stateMachineClass.posRoaming.position;
            //if (agent.enabled == true)
            //{
            agent?.SetDestination(destination);
            //}
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        Transform target = stateMachineClass.SearchMonster();
        if (target)
        {
            if (stateMachineClass.getFlagAtk)
            {
                stateMachine.ChangeState<stateAtk>();
            }
            else
            {
                stateMachine.ChangeState<stateMove>();
            }
        }
        else
        {
            if (agent.enabled == true)
            {
                if (!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance))
                {
                    //Debug.Log(agent.remainingDistance + "<=" + agent.stoppingDistance);
                    Transform nextRoamingPosition = stateMachineClass.getPositionNextRoaming();
                    if (nextRoamingPosition)
                    {
                        agent.SetDestination(nextRoamingPosition.position);
                    }

                    stateMachineClass.ChangeState<stateIdle>();
                }
                else
                {
                    characterController.Move(agent.velocity * Time.deltaTime);
                }
            }
        }
    }

    public override void OnEnd()
    {
        agent.stoppingDistance = stateMachineClass.atkRange;
        agent.ResetPath();
    }
}