using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [System.Serializable]
    private enum BattleState
    {
        ATTACK_READY,
        BUFF_DEBUFF_READY,
        HEAL_READY
    }

    [SerializeField]
    private List<SO_Skill> useSkillList = new List<SO_Skill>();

    private Collider collider;

    [SerializeField] private BattleState enemyBattleState = BattleState.ATTACK_READY;
    private bool isChanging = false;

    protected override void Start()
    {
        base.Start();
        collider = GetComponent<Collider>();
    }

    protected override void Update()
    {
        base.Update();

        if (isBattleMode)
        {
            collider.isTrigger = true;
        }
        else
        {
            collider.isTrigger = false;
        }
        if (!isStunned && (isBattleMode && BattleManager.instance.nowTurnID == 2 && !isChanging))
        {
            StartCoroutine(EnemySkillCast());
        }
        BattleStateCheck();
    }

    private void CheckTimeLimitOver()
    {
        if (GameManager.instance.isTimeLimitOver)
        {
            // 적 강화 함수 생성
        }
    }

    private void BattleStateCheck()
    {
        if (nowHP / characterStats.MAX_HP < 0.5f && CheckHavingAndUseSkill(SO_Skill.SkillDivision.HEAL))
        {
            enemyBattleState = BattleState.HEAL_READY;
        }
        else if ((!nowBuffing || !BattleManager.instance.player.nowDebuffing) && CheckHavingAndUseSkill(SO_Skill.SkillDivision.BUFF_DEBUFF))
        {
            enemyBattleState = BattleState.BUFF_DEBUFF_READY;
        }
        else
        {
            enemyBattleState = BattleState.ATTACK_READY;
        }
    }

    public IEnumerator EnemySkillCast()
    {
        isChanging = true;
        yield return new WaitForSeconds(1);

        if (enemyBattleState == BattleState.ATTACK_READY)
        {
            Debug.Log("Set Attack Skill");
            AddUseSkillList(SO_Skill.SkillDivision.ATTACK);
        }
        else if (enemyBattleState == BattleState.BUFF_DEBUFF_READY)
        {
            AddUseSkillList(SO_Skill.SkillDivision.BUFF_DEBUFF);
        }
        else if (enemyBattleState == BattleState.HEAL_READY)
        {
            AddUseSkillList(SO_Skill.SkillDivision.HEAL);
        }

        yield return new WaitForSeconds(0.5f);

        int cnt = useSkillList.Count;
        //Debug.Log("1. Now Skill Count " + useSkillList.Count);
        int selectSkillIndex = Random.Range(0, cnt);

        //Debug.Log(selectSkillIndex);
        //Debug.Log(useSkillList[selectSkillIndex].skillName);

        int idx = selectSkillIndex;

        yield return new WaitForSeconds(0.5f);

        BattleManager.instance.CastSkill(this, BattleManager.instance.player, useSkillList[idx]);

        Debug.Log("Enemy Casted!");
        Debug.Log("Turn Changed");
        isChanging = false;
    }


    private void AddUseSkillList(SO_Skill.SkillDivision checkDivision)
    {
        Debug.Log("Now Setting!");
        useSkillList = new List<SO_Skill>();
        for (int i = 0; i < skillList.Count; i++)
        {
            Debug.Log("Setting " + i);
            if (skillList[i].skillDivision == checkDivision && skillList[i].needMp <= nowMP)
            {
                useSkillList.Add(skillList[i]);
            }
        }
        //Debug.Log("2. Now Skill Count " + useSkillList.Count);
    }

    public bool CheckHavingAndUseSkill(SO_Skill.SkillDivision checkDivision)
    {
        bool tempBool = false;
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].skillDivision == checkDivision && skillList[i].needMp <= nowMP)
            {
                tempBool = true;
                break;
            }
        }
        return tempBool;
    }

}