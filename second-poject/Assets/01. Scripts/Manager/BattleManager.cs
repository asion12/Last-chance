using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            Debug.Log("Instance Set");
            instance = this;
        }
    }

    public int nowTurnID = 0;
    // 0: Battle Mode off
    // 1: PlayerTurn
    // 2: EnemyTurn
    // 3: 
    // Start is called before the first frame update

    public Character targetEnemy;
    public Character targetCharacter;
    public Player player;
    public bool isTurnUsed = false;
    public bool isCarelessTurnUsed = false;
    private UIManager uIManager = null;
    private EffectManager effectManager = null;
    public NavMeshAgent monster;
    private int BonusExpScale = 0;
    private int disBonusExpScale = 0;
    private bool startBt = true;
    void Start()
    {
        uIManager = FindObjectOfType<UIManager>();
        effectManager = FindObjectOfType<EffectManager>();
    }

    private void Update()
    {
        //Debug.Log("Now Turn is " + nowTurnID.ToString());
        if (targetEnemy != null)
        {
            //Debug.Log(targetEnemy.nowHP);
            //Debug.Log(targetEnemy.nowMP);
        }
        //CheckCharactersHp();
    }

    private bool CheckBattleEnd()
    {
        if (player.nowHP <= 0)
        {
            BattleEnd(false);
            return true;
        }
        else if (targetEnemy.nowHP <= 0)
        {
            BattleEnd(true);
            return true;
        }
        return false;
    }

    public IEnumerator TurnChange()
    {
        if (nowTurnID == 1)
        {
            uIManager.SetBattleUIInactive();
            yield return new WaitForSeconds(1.5f);
            Debug.Log("Turn Change To Enemy");
            nowTurnID = 2;
        }
        else if (nowTurnID == 2)
        {
            yield return new WaitForSeconds(1.5f);
            uIManager.SetBattleUIActive();
            uIManager.SetGameLog_PlayerTurnCommand();
            nowTurnID = 1;
            targetEnemy.GetComponent<Enemy>().isChanging = false;
            Debug.Log("Turn Change To Player");
        }
        else
        {
            Debug.Log("NoOnesTurn!");
        }
        //Debug.Log("Turn Change!");
    }

    public void BattleStart(bool isPlayerStart, bool isVictimCareless, GameObject detactedEnemy)
    {
        SoundManager._instance.backgroundSoundpl(true);
        //SoundManager._instance.cheng();
        SetAllEnemysStop();

        if (nowTurnID != 0)
        {
            Debug.LogWarning("Now is Battle Mode But Trying Battle Start!");
        }
        else
        {
            uIManager.InactiveCanBatteUI();
            SetEnemy(detactedEnemy.GetComponent<Character>());
            targetEnemy.isBattleMode = true;
            player.isBattleMode = true;
            Debug.Log("BattleStartFX!");
            uIManager.FX_BattleStart();
            Debug.Log("SkillUIOnFX!");
            uIManager.SetBattleUIActive();
            player.CameraRotateToTarget(targetEnemy.transform.gameObject);
            if (isPlayerStart)
            {
                //monster.speed = 0;
                uIManager.SetGameLog_PlayerTurnCommand();
                nowTurnID = 1;
                if (isVictimCareless)
                    targetEnemy.carelessCounter = targetEnemy.max_carelessCounter;
            }
            else
            {
                nowTurnID = 2;
            }
        }
    }

    public void BattleEnd(bool isPlayerWin)
    {
        SoundManager._instance.backgroundSoundpl(false);
        SetAllEnemysRestart();
        ResetBattleSetting();
        if (isPlayerWin)
        {
            float randomValue = UnityEngine.Random.Range(0.8f, 1.2f);
            player.EXP += (int)(20 * (1 + (-player.GetLevelScale_forBattle(player.Level - targetEnemy.Level) * 2)) * randomValue * (1 + Mathf.Log(BonusExpScale + 1, 2)) / (1 + Mathf.Log(disBonusExpScale + 1, 2)));
            BonusExpScale = 0;
            Debug.Log("Player Win");
            Debug.Log("Print UI What Player Get");
            uIManager.SetBattleUIInactive();
            Destroy(targetEnemy.gameObject);
            targetEnemy = null;
            //monster.speed = 3.5f;
        }
        else
        {
            BonusExpScale = 0;
            Debug.Log("Enemy Win!");
            Debug.Log("GameOverFX");
            Debug.Log("DropPlayerInventory");
            Debug.Log("GameRestart");
            uIManager.SetBattleUIInactive();
            GameManager.instance.DieOutDungeon();
            //monster.speed = 3.5f;
        }
        uIManager.InactiveCanBatteUI();
    }

    private void SetAllEnemysStop()
    {
        Enemy[] Enemys = FindObjectsOfType<Enemy>();
        for (int i = 0; i < Enemys.Length; i++)
        {
            Enemys[i].StopMoving();
        }
    }

    private void SetAllEnemysRestart()
    {
        Enemy[] Enemys = FindObjectsOfType<Enemy>();
        for (int i = 0; i < Enemys.Length; i++)
        {
            Enemys[i].RestartMoving();
            //Enemys[i].gameObject.GetComponent<MonsterFSM>().ChangeState<stateIdle>();
        }

    }

    public IEnumerator BattleRun(bool isPlayerRun)
    {
        SoundManager._instance.backgroundSoundpl(false);
        SetAllEnemysRestart();
        ResetBattleSetting();
        if (isPlayerRun)
        {
            uIManager.SetBattleUIInactive();
            Debug.Log("Enemy Stunned!");
            yield return new WaitForSeconds(5);
        }
        else
        {
            Debug.Log("Player Stunned!");
            yield return new WaitForSeconds(5);
        }
    }

    private void ResetBattleSetting()
    {
        nowTurnID = 0;
        targetEnemy.isBattleMode = false;
        player.isBattleMode = false;
        // Debug.Log(player.isBattleMode);
        player.ResetBattleStatus();
        targetEnemy.ResetBattleStatus();
    }

    public void SetEnemy(Character setEnemy)
    {
        targetEnemy = setEnemy;
    }

    public void SetCharacter(Character setEnemy)
    {
        targetCharacter = setEnemy;
    }

    public void CastSkill(Character skillCaster, Character skillVictim, SO_Skill castSkill)
    {
        if (skillCaster.GetComponent<Player>() != null)
        {
            StartCoroutine(uIManager.SendGameLog("당신은(는) " + castSkill.skillName + " 을(를) 사용했다!"));
        }
        else
        {
            StartCoroutine(uIManager.SendGameLog("상대은(는) " + castSkill.skillName + " 을(를) 사용했다!"));
        }


        if (skillCaster.nowMP < castSkill.needMp)
        {
        }
        else
        {
            skillCaster.nowMP -= castSkill.needMp;

            bool isCritical = false;
            bool isAdvantage = false;
            bool isReject = false;
            bool isGuard = false;
            bool isMiss = false;
            bool isDeception = false;
            int overDealing = 0;
            Debug.Log("Casted!");

            if (!skillVictim.isCareless)
            {
                isTurnUsed = true;
                bool PercentageCheck(float percnetage)
                {
                    float accuarityRoll = UnityEngine.Random.Range(0f, 100f);
                    if (accuarityRoll < percnetage)
                        return true;
                    else
                        return false;
                }

                // SkillCaster FOC Check
                castSkill.accuarityPer += Mathf.Log(1 + skillCaster.totalStats.FOC, 2) * 5;
                castSkill.casterCriticalPer += Mathf.Log(1 + skillCaster.totalStats.FOC, 2) * 5;
                if (skillCaster.nowCP > castSkill.needCP)
                {
                    Debug.Log("Cp Over!!");
                    castSkill.accuarityPer += (skillCaster.nowCP - castSkill.needCP) * Mathf.Log(1 + skillCaster.totalStats.FOC, 2);
                }
                else if (skillCaster.nowCP < castSkill.needCP)
                {
                    Debug.Log("Cp Min!!");
                    castSkill.accuarityPer -= (castSkill.needCP - skillCaster.nowCP) / Mathf.Log(1 + skillCaster.totalStats.FOC, 2);
                }
                else
                {
                    Debug.Log("Cp fits");
                }

                Debug.Log("Skill ACC is " + castSkill.accuarityPer.ToString());

                // SkillVicTim DEX Check
                castSkill.accuarityPer = castSkill.accuarityPer
                - ((castSkill.accuarityPer * (Mathf.Log(1 + skillVictim.totalStats.DEX, 2) - 1) / Mathf.Log(1 + skillVictim.totalStats.DEX, 2))
                * (Mathf.Log(1 + skillVictim.totalStats.CHA, 2) - 1 + (skillVictim.nowCP / skillVictim.maxCP))
                / Mathf.Log(1 + skillVictim.totalStats.CHA, 2));

                Debug.Log("DexVal = " + Mathf.Log(1 + skillVictim.totalStats.DEX, 2).ToString());
                Debug.Log("VicNowCP = " + skillVictim.nowCP + "VicMaxCP = " + skillVictim.maxCP + " CPVal = " + ((float)((float)skillVictim.nowCP / (float)skillVictim.maxCP)).ToString());
                Debug.Log("After Skill_1 ACC is " + castSkill.accuarityPer.ToString());

                castSkill.accuarityPer -= skillVictim.totalStats.DEX;

                Debug.Log("After Skill_2 ACC is " + castSkill.accuarityPer.ToString());

                castSkill.victimDeceptionPer += Mathf.Log(1 + skillVictim.totalStats.CHA, 2) * 10;


                if (castSkill.accuarityPer > 100)
                {
                    Debug.Log("Cp Over!!");
                    castSkill.casterCriticalPer += castSkill.accuarityPer - 100;
                }
                else if (castSkill.accuarityPer < 0)
                {
                    Debug.Log("Cp Min!!");
                    castSkill.victimDeceptionPer += -1 * castSkill.accuarityPer * Mathf.Log(1 + skillVictim.totalStats.CHA, 2);
                    castSkill.accuarityPer = (float)0;
                }
                // // Skill Deception Percentage Add to CHA
                // castSkill.victimDeceptionPer *= Mathf.Log( 1 +  1 + skillVictim.totalStats.CHA, 2);
                // castSkill.victimDeceptionPer += skillVictim.totalStats.CHA;

                // Skill Hit Check
                if (PercentageCheck(castSkill.accuarityPer))
                {
                    if (CheckElement(skillVictim.totalResistElements, castSkill.skillElements))
                    {
                        Debug.Log("Guard!");
                        skillCaster.carelessCounter++;
                        isReject = true;
                        isGuard = true;
                        if (PercentageCheck(castSkill.victimDeceptionPer))
                        {
                            Debug.Log("Deception!");
                            isDeception = true;
                            skillCaster.carelessCounter++;
                        }
                    }
                    else
                    {
                        if (PercentageCheck(castSkill.casterCriticalPer))
                        {
                            Debug.Log("Critical!");
                            isCritical = true;
                            skillVictim.carelessCounter++;
                        }

                        if (CheckElement(skillVictim.totalWeakElements, castSkill.skillElements))
                        {
                            Debug.Log("Advantage!");
                            isAdvantage = true;
                            skillVictim.carelessCounter++;
                        }

                        Debug.Log("Hit!");
                    }
                }
                else
                {
                    Debug.Log("Miss!");
                    isMiss = true;
                    isReject = true;
                    skillCaster.carelessCounter++;
                    if (PercentageCheck(castSkill.victimDeceptionPer))
                    {
                        Debug.Log("Deception!");
                        isDeception = true;
                        skillCaster.carelessCounter++;
                    }
                }
            }
            else
            {
                isCarelessTurnUsed = true;
                isCritical = true;
                isAdvantage = true;
                overDealing = skillVictim.carelessCounter - skillVictim.max_carelessCounter;
                Debug.Log("Careless!");

                skillVictim.carelessCounter = 0;
                skillVictim.isCareless = false;
            }

            if (isDeception && skillCaster.GetComponent<Player>() != null)
            {
                BonusExpScale++;
                Debug.Log("기만 성공! 경험치 배율 상승됨!");
            }
            else if (isDeception && skillCaster.GetComponent<Enemy>() != null)
            {
                disBonusExpScale++;
                Debug.Log("적 기만 성공! 경혐치 배율 하락됨...");
            }

            float increaseDamage = 0f;
            float decreaseDamage = 0f;

            if (castSkill.categoryPhysics)
            {
                increaseDamage = Mathf.Log(1 + skillCaster.totalStats.STR, 2f);
                decreaseDamage = Mathf.Log(1 + skillVictim.totalStats.FIR, 2f);
            }
            else if (castSkill.categoryChemistry)
            {
                Debug.Log("Chemistry");
                increaseDamage = Mathf.Log(1 + skillCaster.totalStats.INT, 2f);
                decreaseDamage = Mathf.Log(1 + skillVictim.totalStats.WIS, 2f);
            }

            Debug.Log("Cast Skill Damage  = " + castSkill.skillDamage.ToString());
            Debug.Log("increaseDamage = " + (float)increaseDamage);
            float checkDamage = (float)castSkill.skillDamage * increaseDamage;
            Debug.Log("Check Damage is " + checkDamage);

            float finalSkillDamage =
            (castSkill.skillDamage * increaseDamage * (1f + (float)overDealing + (float)Convert.ToDouble(isAdvantage))
            / (isCritical ? 1f : (float)decreaseDamage)) * (float)Convert.ToDouble(!isReject);

            Debug.Log("Final Skill Damage is " + finalSkillDamage.ToString());
            skillVictim.nowHP -= (int)finalSkillDamage;
            Debug.Log("Now Victim Hp : " + skillVictim.nowHP.ToString());

            if (!isReject && skillVictim.nowHP > 0)
            {
                StartCoroutine(effectManager.HitStop(skillVictim.gameObject,
                // (Convert.ToInt32(skillVictim.isCareless) + Convert.ToInt32(isAdvantage) + Convert.ToInt32(isCritical))
                1));
            }
            else
            {
                Debug.Log("Rejected!!!!!!");
            }

            ResetSkillNurmical(castSkill);
            CheckTurnChange(skillVictim);
            SkillDamageEffect((int)finalSkillDamage, isCarelessTurnUsed, isCritical, isAdvantage, isMiss, isGuard, isDeception,
            skillVictim.GetComponent<Player>() != null ? true : false);
        }
    }

    private void SkillDamageEffect(int DamageVal, bool isSuprised, bool isCritical, bool isAdditional, bool isMissed, bool isGuarded, bool isDeception, bool isPlayerEffect)
    {
        StartCoroutine(effectManager.MakeDamageInfoEffect(DamageVal, isSuprised, isCritical, isAdditional, isMissed, isGuarded, isDeception, isPlayerEffect));
    }

    public void SetOverClock()
    {

    }

    public void CastOverClockSkills(Player overClockSkillCaster, Character overClockSkillVictim)
    {
        List<SO_Skill> casterSkillList = overClockSkillCaster.skillList;
        for (int i = 0; i < casterSkillList.Count; i++)
        {
            if (overClockSkillCaster.isSkillOverClockList[i])
            {
                CastSkill(overClockSkillCaster, overClockSkillVictim, casterSkillList[i]);
                if (overClockSkillCaster.isCareless)
                {
                    OverClockEnd(overClockSkillCaster);
                    break;
                }
            }
        }
    }

    private void OverClockEnd(Player resetCharacter)
    {
        Debug.Log("OverClockEnded!");
        resetCharacter.isSkillOverClockList = new bool[] { false, };
    }

    private void OneMoreForEnemy()
    {
        targetEnemy.GetComponent<Enemy>().isChanging = false;
    }

    private void CheckTurnChange(Character checkCharacter)
    {
        if (CheckBattleEnd())
        {

        }
        else if (checkCharacter.carelessCounter >= checkCharacter.max_carelessCounter)
        {
            Debug.Log("OneMore");
            if (checkCharacter.GetComponent<Player>() != null)
            {
                //Debug.Log("One More Enemy Casted");
                Invoke("OneMoreForEnemy", 1.5f);
                //StartCoroutine(targetEnemy.GetComponent<Enemy>().EnemySkillCast());
            }
        }
        else
        {
            if (!isTurnUsed)
            {
                Debug.Log("One More");
                if (checkCharacter.GetComponent<Player>() != null)
                {
                    //Debug.Log("One More Enemy Casted");
                    Invoke("OneMoreForEnemy", 1.5f);
                    //StartCoroutine(targetEnemy.GetComponent<Enemy>().EnemySkillCast());
                }
            }
            else
            {
                Debug.Log("TurnChange");
                isTurnUsed = false;
                isCarelessTurnUsed = false;
                StartCoroutine(TurnChange());
            }
        }
    }

    private void ResetSkillNurmical(SO_Skill resetSkill)
    {
        resetSkill.accuarityPer = 100;
        resetSkill.casterCriticalPer = 0;
        resetSkill.victimDeceptionPer = 0;
    }

    private bool[] ElementArrReturn(Elements el)
    {
        bool[] elementArr = new bool[7];
        elementArr[0] = el.SOLAR;
        elementArr[1] = el.LUMINOUS;
        elementArr[2] = el.IGNITION;
        elementArr[3] = el.HYDRO;
        elementArr[4] = el.BIOLOGY;
        elementArr[5] = el.METAL;
        elementArr[6] = el.SOIL;

        return elementArr;
    }

    private int[] Element_intArrReturn(Elements_int el)
    {
        int[] elementArr = new int[7];
        elementArr[0] = el.SOLAR;
        elementArr[1] = el.LUMINOUS;
        elementArr[2] = el.IGNITION;
        elementArr[3] = el.HYDRO;
        elementArr[4] = el.BIOLOGY;
        elementArr[5] = el.METAL;
        elementArr[6] = el.SOIL;

        return elementArr;
    }

    private bool CheckElement(Elements characterElements, Elements skillElements)
    {

        int count = 0;

        bool[] characterEl = ElementArrReturn(characterElements);
        bool[] skillEl = ElementArrReturn(skillElements);

        for (int i = 0; i < 7; i++)
        {
            if (characterEl[i] && skillEl[i])
                count++;
        }

        // for (int i = 0; i < 7; i++)
        // {
        //     switch (i)
        //     {
        //         case 0:
        //             if (characterElements.SOLAR && skillElements.SOLAR)
        //                 count++;
        //             break;
        //         case 1:
        //             if (characterElements.LUMINOUS && skillElements.LUMINOUS)
        //                 count++;
        //             break;
        //         case 2:
        //             if (characterElements.IGNITION && skillElements.IGNITION)
        //                 count++;
        //             break;
        //         case 3:
        //             if (characterElements.HYDRO && skillElements.HYDRO)
        //                 count++;
        //             break;
        //         case 4:
        //             if (characterElements.BIOLOGY && skillElements.BIOLOGY)
        //                 count++;
        //             break;
        //         case 5:
        //             if (characterElements.METAL && skillElements.METAL)
        //                 count++;
        //             break;
        //         case 6:
        //             if (characterElements.SOIL && skillElements.SOIL)
        //                 count++;
        //             break;
        //         default:
        //             break;
        //     }
        // }

        if (count > 0)
            return true;
        else
            return false;
    }
}