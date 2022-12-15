using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;
using System.Reflection.Emit;

public class be : MonsterFSM, IAtkAble
{
    [SerializeField]
    private List<AtkBehaviour> attackBehaviours = new List<AtkBehaviour>();

    public AtkBehaviour nowAtkBehaviour
    {
        get;
        private set;
    }


    public LayerMask targetLayerMask;


    private GameObject atkEffectPrefab = null;

    public Transform launchWeaponTransform;
    public Transform weaponHitTransform;

    protected override void Start()
    {
        base.Start();
        fsmManager.AddStateList(new stateMove());
        fsmManager.AddStateList(new stateAtk());
        fsmManager.AddStateList(new stateDie());

        OnAwakeAtkBehaviour();

        atkRange = nowAtkBehaviour?.atkRange ?? 5.0f;
    }

    protected override void Update()
    {
        OnCheckAtkBehaviour();
        base.Update();
    }


    public override bool getFlagAtk
    {
        get
        {
            if (!target)
            {
                return false;
            }

            float distance = Vector3.Distance(transform.position, target.position);
            Debug.Log("distance : " + distance + ">>>>>> atkRange" + atkRange);
            return (distance <= atkRange);
        }
    }

    public J ChangeState<J>() where J : State<MonsterFSM>
    {
        return fsmManager.ChangeState<J>();
    }

    override public Transform SearchMonster()
    {
        return base.target;
    }

    public void OnExecuteAttack(int attackIndex)
    {
        if (nowAtkBehaviour != null && target != null)
        {
            nowAtkBehaviour.callAtkMotion(target.gameObject, launchWeaponTransform);
        }
    }

    private void OnAwakeAtkBehaviour()
    {
        foreach (AtkBehaviour behaviour in attackBehaviours)
        {
            if (nowAtkBehaviour == null)
            {
                nowAtkBehaviour = behaviour;
            }

            behaviour.targetLayerMask = targetLayerMask;
        }
    }

    private void OnCheckAtkBehaviour()
    {
        if (nowAtkBehaviour == null || !nowAtkBehaviour.IsAvailable)
        {
            nowAtkBehaviour = null;

            foreach (AtkBehaviour behaviour in attackBehaviours)
            {
                if (behaviour.IsAvailable)
                {
                    if ((nowAtkBehaviour == null) || (nowAtkBehaviour.importanceAtkNo < behaviour.importanceAtkNo))
                    {
                        nowAtkBehaviour = behaviour;
                    }
                }
            }
        }
    }



}
