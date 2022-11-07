using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

//객체의 hp가 다 되었으면 죽었다고 생각을 하고 3초후 해당 게임 오브젝트를 완전히 삭
[Serializable]
public class stateDie : State<MonsterFSM>
{
    private Animator animator;

    protected int flagLive = Animator.StringToHash("flagLive");
    public override void OnAwake()
    {
        animator = stateMachineClass.GetComponent<Animator>();
    }

    public override void OnStart()
    {
        animator?.SetBool(flagLive, false);
    }

    public override void OnUpdate(float deltaTime)
    {
        if (stateMachine.getStateDurationTime > 3.0f)
        {
            GameObject.Destroy(stateMachineClass.gameObject);
        }
    }

    public override void OnEnd()   { }
}