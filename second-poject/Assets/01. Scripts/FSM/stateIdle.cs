using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class stateIdle : State<MonsterFSM>
{
    public bool flagRoaming = true;
    private float roamingStateMinIdleTime = 0.0f;
    private float roamingStateMaxIdleTime = 3.0f;
    private float roamingStateIdleTime = 0.0f;

     
    private CharacterController characterController;
     

    public override void OnAwake()
    { 
        characterController = stateMachineClass.GetComponent<CharacterController>();
    }
     
    public override void OnStart()
    {  
        characterController?.Move(Vector3.zero);

        if(flagRoaming)
        {
            roamingStateIdleTime = Random.Range(roamingStateMinIdleTime, roamingStateMaxIdleTime);
        }
    }
      
    public override void OnUpdate(float deltaTime)
    { 
        if (stateMachineClass.target)
        {
            Debug.Log(stateMachineClass.getFlagAtk); 
            if (stateMachineClass.getFlagAtk)
            {  
                stateMachine.ChangeState<stateAtk>();
            }
            else
            {  
                stateMachine.ChangeState<stateMove>();
            }
        }
        else if(flagRoaming && stateMachine.getStateDurationTime > roamingStateIdleTime)
        {
            stateMachine.ChangeState<stateRoaming>();
        }
    }
     
    public override void OnEnd()
    {
    }
} 