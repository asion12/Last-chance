using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterFSM : MonoBehaviour
{
  
    private Character character;
    protected CharacterController characterController;
    protected StateMachine<MonsterFSM> fsmManager;
    public StateMachine<MonsterFSM> FsmManager => fsmManager;

    public float atkRange = 0.15f;

    protected NavMeshAgent agent; 

    public Transform[] posRoamingLists;
    public Transform posRoaming = null;
    private int posRoamingListIdx = 0;


    public Transform SetPositon;
    private FOV fov;
    public Transform target => fov.FirstTarget;
    public LayerMask targetLayerMask => fov.targetLayerMask;
 
    protected virtual void Start()
    {
        character = GetComponent<Character>();
        characterController = GetComponent<CharacterController>();
        fsmManager = new StateMachine<MonsterFSM>(this, new stateRoaming());
        stateIdle stateIdle = new stateIdle();
        stateIdle.flagRoaming = true;
        fsmManager.AddStateList(stateIdle);
        fsmManager.AddStateList(new stateMove());
        fsmManager.AddStateList(new stateAtk());

        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = true;
         
        fov = GetComponent<FOV>();
    }

    protected virtual void Update()
    {
        fsmManager.Update(Time.deltaTime);
        if(!(fsmManager.getNowState is stateMove) && !(fsmManager.getNowState is stateDie))
        {
            lookAtTarget();
            
        }
    }

    private void LateUpdate()
    {
        transform.position = agent.nextPosition;
    }

    void lookAtTarget()
    {
        if(target)
        {
            Vector3 lookAt = (target.position - target.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(lookAt.x, 0, lookAt.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private void OnAnimatorMove()
    { 

        // Follow CharacterController
        Vector3 pos = transform.position;
        pos.y = agent.nextPosition.y;
         
        agent.nextPosition = pos;

    }
     
    public virtual Transform SearchMonster()
    {
        return target;
    }

    public Transform getPositionNextRoaming()
    {
        posRoaming = null;

        if (posRoamingLists.Length > 0)
        {
            posRoaming = posRoamingLists[posRoamingListIdx];
            posRoamingListIdx = (posRoamingListIdx + 1) % posRoamingLists.Length;
        }

        return posRoaming;
    }

    public virtual bool getFlagAtk => false;

    public J ChangeState<J>() where J : State<MonsterFSM>
    {
        return fsmManager.ChangeState<J>();
    }
}





