using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.AI;

public class stateMove : State<MonsterFSM>
{
    //이동에 대한 애니메이터 
    private Animator animator;
    //이동 로직을 처리할 캐릭터 컨트롤 
    private CharacterController characterController;
    //길찾기를 위한 
    private NavMeshAgent agent;

    //애니메이터를 받아올 변수 이동과 이동 스피드를 추가 
    private int hashMove = Animator.StringToHash("Move");
    private int hashMoveSpeed = Animator.StringToHash("MoveSpd");

    //상태 초기화 언제 호출된다고 ? 등록할 때 딱 한번 
    public override void OnAwake()
    {
        //캐싱 해주고 
        animator = stateMachineClass.GetComponent<Animator>();
        characterController = stateMachineClass.GetComponent<CharacterController>();

        agent = stateMachineClass.GetComponent<NavMeshAgent>();
    }

    //상태전환 시  이동 상태가 될 때마다 
    public override void OnStart()
    {
        //초기 타겟 위치를 이동할 위치로 지정 
        agent?.SetDestination(stateMachineClass.target.position);
        animator?.SetBool(hashMove, true);
    }

    //이동이 계속 진행 될 때 일어날 일
    //이동 상태가 전환되는 건 타겟이 있고 타겟에 다가가야 할 때  이루어 지는데
    //이동시 계속 진행 될 것이 따로 있나 하겠지만
    //타겟이 이동을 하거나 타겟이 변경이 될 때
    //타겟 위치를 수정하며 이동을 해야 한다
    //그러면 결국 대기에서 사용했더 타겟 검사를 통해서
    //타겟에 이동이 가능하다면
    //새로운 적위치를 타겟 위치로 변경하고 이동한다
    //타겟이 이동이 불가능하거나 공격을 해야 할 때 대기 상태로 전환 된다 
    public override void OnUpdate(float deltaTime)
    {
        //타겟 검색 지속적 진
        Transform target= stateMachineClass.SearchMonster();
        //타겟이 존재하면
        Debug.Log("stateMachineClass.getFlagAtk : " +stateMachineClass.getFlagAtk);
        if (target && !stateMachineClass.getFlagAtk)
        {
            //타겟 위치로 목표지점을 설정하고 
            agent.SetDestination(stateMachineClass.target.position);

            //현재 navmesh에서 목표지점의 거리가 남았다면 
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                //캐릭터 컨트롤러로 이동을 시킨다 
                characterController.Move(agent.velocity * Time.deltaTime);
                //애니메이터의 값을 설정하는데 걷기 애니메이과 현재 가속도를 동기화
                //damp 보간 시간 0.1f
                animator.SetFloat(hashMoveSpeed, agent.velocity.magnitude / agent.speed, 0.1f, Time.deltaTime);
                return;
            }
        }
        //타겟이 존재하지 않거나 에이전트가 목표지점에 도달 했다면 대기상태로 전환 

        agent.SetDestination(stateMachineClass.SetPositon.position);
        stateMachine.ChangeState<stateIdle>();
    
    }

    //이동 상태에서 벗어날 때 
    public override void OnEnd()
    {
        //이동 애니메이션을 끈다 
        animator?.SetBool(hashMove, false);
        animator?.SetFloat(hashMoveSpeed, 0);
        //에이전트의 현재 길찾기를 전히 초기화 하여 더 이상 길찾기를 하지 않게 한다 
        agent.ResetPath();
    }
}