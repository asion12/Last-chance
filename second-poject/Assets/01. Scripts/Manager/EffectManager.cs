using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private GameObject SOLAR_ChemicalEffect;
    [SerializeField] private GameObject LUMINOUS_ChemicalEffect;
    [SerializeField] private GameObject IGNITION_ChemicalEffect;
    [SerializeField] private GameObject HYDRO_ChemicalEffect;
    [SerializeField] private GameObject BIOLOGY_ChemicalEffect;
    [SerializeField] private GameObject METAL_ChemicalEffect;
    [SerializeField] private GameObject SOIL_ChemicalEffect;

    [SerializeField] private GameObject SOLAR_PhysicsEffect;
    [SerializeField] private GameObject LUMINOUS_PhysicsEffect;
    [SerializeField] private GameObject IGNITION_PhysicsEffect;
    [SerializeField] private GameObject HYDRO_PhysicsEffect;
    [SerializeField] private GameObject BIOLOGY_PhysicsEffect;
    [SerializeField] private GameObject METAL_PhysicsEffect;
    [SerializeField] private GameObject SOIL_PhysicsEffect;
    // Start is called before the first frame update4

    [SerializeField] private Image PlaeyrEffectBG;
    [SerializeField] private GameObject PlayerEffectBase;
    [SerializeField] private GameObject EnemyEffectBase;
    [SerializeField] private List<GameObject> PlayerDamageEffectGroup;
    [SerializeField] private List<GameObject> EnemyDamageEffectGroup;

    [SerializeField] private GameObject BattleResultEffectParent;
    [SerializeField] private List<GameObject> BattleResultEffectGroup;

    [SerializeField] private GameObject AddSkillPool;
    void Start()
    {
        //StartCoroutine(MakeDamageInfoEffect(1000, true, true, true, false, false, false, true));
        //StartCoroutine(MakeDamageInfoEffect(1000, true, true, true, false, false, false, false));
        //FX_DamageEffect(PlayerEffectBase);
        //FX_DamageEffect_Base(EnemyEffectBase);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MakeSkillEffect(SO_Skill useSkill, bool isCasterPlayer)
    {
        //Debug.LogWarning("Effect!");
        GameObject setEffect = null;
        GameObject tempEffect = null;

        if (useSkill.categoryPhysics)
        {
            if (useSkill.skillElements.SOLAR)
                setEffect = SOLAR_PhysicsEffect;

            else if (useSkill.skillElements.LUMINOUS)
                setEffect = LUMINOUS_PhysicsEffect;

            else if (useSkill.skillElements.IGNITION)
                setEffect = IGNITION_PhysicsEffect;

            else if (useSkill.skillElements.HYDRO)
                setEffect = HYDRO_PhysicsEffect;

            else if (useSkill.skillElements.BIOLOGY)
                setEffect = BIOLOGY_PhysicsEffect;

            else if (useSkill.skillElements.METAL)
                setEffect = METAL_PhysicsEffect;

            else if (useSkill.skillElements.SOIL)
                setEffect = SOIL_PhysicsEffect;
        }
        else if (useSkill.categoryChemistry)
        {
            if (useSkill.skillElements.SOLAR)
                setEffect = SOLAR_ChemicalEffect;

            else if (useSkill.skillElements.LUMINOUS)
                setEffect = LUMINOUS_ChemicalEffect;

            else if (useSkill.skillElements.IGNITION)
                setEffect = IGNITION_ChemicalEffect;

            else if (useSkill.skillElements.HYDRO)
                setEffect = HYDRO_ChemicalEffect;

            else if (useSkill.skillElements.BIOLOGY)
                setEffect = BIOLOGY_ChemicalEffect;

            else if (useSkill.skillElements.METAL)
                setEffect = METAL_ChemicalEffect;

            else if (useSkill.skillElements.SOIL)
                setEffect = SOIL_ChemicalEffect;
        }
        else
        {
            //Debug.LogWarning("No Element");
            setEffect = METAL_PhysicsEffect;
        }

        Vector3 SetVector;
        Vector3 MoveVector;
        if (isCasterPlayer)
        {
            SetVector = BattleManager.instance.targetEnemy.transform.position;
            MoveVector = new Vector3(BattleManager.instance.player.transform.position.x,
            BattleManager.instance.player.transform.localPosition.y + 0.75f,
            BattleManager.instance.player.transform.localPosition.z);

            tempEffect = Instantiate(setEffect, SetVector, Quaternion.identity) as GameObject;
            tempEffect.transform.Translate(-(SetVector - MoveVector).normalized * 0.75f);
        }
        else
        {
            SetVector = new Vector3(BattleManager.instance.player.transform.localPosition.x,
            BattleManager.instance.player.transform.localPosition.y + 0.5f,
            BattleManager.instance.player.transform.localPosition.z);
            MoveVector = BattleManager.instance.targetEnemy.transform.localPosition;

            tempEffect = Instantiate(setEffect, SetVector, Quaternion.identity) as GameObject;
            tempEffect.transform.Translate(-(SetVector - MoveVector).normalized * 1);
        }

        ParticleSystem particleSystem = tempEffect.GetComponent<ParticleSystem>();
        particleSystem.Play();
        //float particleDuration = particleSystem.duration + particleSystem.startLifetime;
        Destroy(tempEffect, 1.25f);
    }

    public IEnumerator HitStop(GameObject HitObject, float HitValue)
    {
        if (HitObject == null)
        {

        }
        else
        {
            // bool isHitObjHasNav = false;
            // if (HitObject.GetComponent<NavMeshAgent>())
            // {
            //     isHitObjHasNav = true;
            //     //HitObject.GetComponent<NavMeshAgent>().enabled = true;
            // }

            Vector3 orginPos = HitObject.transform.localPosition;
            if (HitObject != null)
            {
                //Debug.LogWarning("Hit Stopped!");
                Time.timeScale = 0f;
                yield return new WaitForSecondsRealtime(0.15f * HitValue);
                //Debug.LogWarning("HitStopEnd!");
                Time.timeScale = 1f;
                if (HitObject != null)
                {
                    HitObject.transform.DOKill();

                    //HitObject.transform.localPosition = orginPos;
                    HitObject.transform.DOShakePosition(0.25f, 1f * HitValue, 25 * (int)HitValue, 45).OnComplete(() =>
                    {
                        //HitObject.transform.position = orginPos;
                        // if (isHitObjHasNav)
                        //     HitObject.GetComponent<NavMeshAgent>().enabled = false;
                    }).OnKill(() =>
                    {

                        //HitObject.transform.position = orginPos;
                        // if (isHitObjHasNav)
                        //     HitObject.GetComponent<NavMeshAgent>().enabled = false;
                    });
                }
            }
            // /Sequence seq = DOTween.Sequence();
        }
    }

    private int fxCount = 1;
    private float effectTime = 0.125f;
    public IEnumerator MakeDamageInfoEffect(int DamageVal, bool isSuprised, bool isCritical, bool isAdditional, bool isMissed, bool isGuarded, bool isDeception, bool isPlayerEffect)
    {
        fxCount = 1;

        bool[] effectActiveList = { false, false, false, false, false, false };

        if (isSuprised)
        {
            effectActiveList[0] = true;
            fxCount++;
        }
        if (isCritical)
        {
            effectActiveList[1] = true;
            fxCount++;
        }
        if (isAdditional)
        {
            effectActiveList[2] = true;
            fxCount++;
        }
        if (isMissed)
        {
            effectActiveList[3] = true;
            fxCount++;
        }
        if (isGuarded)
        {
            effectActiveList[4] = true;
            fxCount++;
        }
        if (isDeception)
        {
            effectActiveList[5] = true;
            fxCount++;
        }

        if (isPlayerEffect)
        {
            fxCount++;
            FX_DamageEffect_PlayerBG();
            yield return new WaitForSeconds(effectTime / fxCount);
            PlayerEffectBase.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = DamageVal.ToString();
            FX_DamageEffect(PlayerEffectBase);
        }
        else
        {
            EnemyEffectBase.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = DamageVal.ToString();
            FX_DamageEffect(EnemyEffectBase);
        }

        yield return new WaitForSeconds(effectTime / fxCount);

        for (int i = 0; i < 6; i++)
        {
            if (effectActiveList[i])
            {
                if (isPlayerEffect)
                {
                    FX_DamageEffect(PlayerDamageEffectGroup[i]);
                    yield return new WaitForSeconds(effectTime / fxCount);
                }
                else
                {
                    FX_DamageEffect(EnemyDamageEffectGroup[i]);
                    yield return new WaitForSeconds(effectTime / fxCount);
                }
            }
        }
        yield return new WaitForSeconds(1f);
        if (isPlayerEffect)
            FX_DamageEffect_Off(PlayerEffectBase);
        else
            FX_DamageEffect_Off(EnemyEffectBase);
        FX_AllDamageEffectOff(effectActiveList, isPlayerEffect);
        yield return new WaitForSeconds(effectTime / fxCount);
        FX_DamageEffect_PlayerBG_Off();
    }

    private void FX_AllDamageEffectOff(bool[] tempActiveList, bool isPlayerEffect)
    {
        for (int i = 0; i < 6; i++)
        {
            if (tempActiveList[i])
            {
                if (isPlayerEffect)
                {
                    FX_DamageEffect_Off(PlayerDamageEffectGroup[i]);
                }
                else
                {
                    FX_DamageEffect_Off(EnemyDamageEffectGroup[i]);
                }
            }
        }
    }

    private void FX_DamageEffect_PlayerBG()
    {
        PlaeyrEffectBG.DOKill();
        PlaeyrEffectBG.color = new Color(PlaeyrEffectBG.color.r, PlaeyrEffectBG.color.g, PlaeyrEffectBG.color.b, 0);
        PlaeyrEffectBG.gameObject.SetActive(true);

        PlaeyrEffectBG.DOFade(0.75f, 0.125f);
    }

    private void FX_DamageEffect_PlayerBG_Off()
    {
        PlaeyrEffectBG.DOKill();
        //PlaeyrEffectBG.color = new Color(PlaeyrEffectBG.color.r, PlaeyrEffectBG.color.g, PlaeyrEffectBG.color.b, 0);


        PlaeyrEffectBG.DOFade(0, 0.5f / fxCount)
        .OnComplete(() =>
        {
            PlaeyrEffectBG.gameObject.SetActive(false);
        })
        ;
    }

    // private void FX_DamageEffect_Base(GameObject effectBaseObject)
    // {
    //     effectBaseObject.transform.localScale = new Vector3(0, 0, 0);
    //     effectBaseObject.SetActive(true);

    //     Sequence sequence = DOTween.Sequence();
    //     sequence
    //     .Append(effectBaseObject.transform.DOScale(1.25f, 1f))
    //     .Append(effectBaseObject.transform.DOScale(1f, 1f));
    //     // effectBaseObject.transform.DOKill();

    //     // //effectBaseSet
    //     // effectBaseObject.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);

    //     // //effectBaseImageSet
    //     // effectBaseObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(effectBaseObject.GetComponent<Image>().color.r, effectBaseObject.GetComponent<Image>().color.g, effectBaseObject.GetComponent<Image>().color.b, 0);

    //     // //effectBaseDamageIntSet
    //     // Color tempColor = effectBaseObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color;
    //     // effectBaseObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = new Color(tempColor.r, tempColor.g, tempColor.b, 0);

    //     // //EffectBaseInfoSet
    //     // tempColor = effectBaseObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().color;
    //     // effectBaseObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().color = new Color(tempColor.r, tempColor.g, tempColor.b, 0);


    //     // effectBaseObject.SetActive(true);

    //     // //effectBaseTwee 
    //     // effectBaseObject.transform.DOScale(1, 1 / fxCount);

    //     // //effectBaseImageTween
    //     // effectBaseObject.transform.GetChild(0).gameObject.GetComponent<Image>().DOFade(1, 1 / fxCount);

    //     // //effectBaseImageTween
    //     // effectBaseObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().DOFade(1, 1 / fxCount);

    //     // //effectBase
    //     // effectBaseObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().DOFade(1, 1 / fxCount);
    // }

    private void FX_DamageEffect(GameObject effectObject /*float BounceValue*/)
    {
        effectObject.transform.localScale = new Vector3(0, 0, 0);
        effectObject.SetActive(true);

        Sequence sequence = DOTween.Sequence();
        sequence
        .Append(effectObject.transform.DOScale(1.25f, effectTime))
        .Append(effectObject.transform.DOScale(1f, effectTime / 2));
    }

    private void FX_DamageEffect_Off(GameObject effectObject)
    {
        effectObject.transform.DOScale(0, effectTime).OnComplete(() =>
        {
            effectObject.SetActive(false);
        });
    }
    // private void ShakeObject(float duration, int vibrato, GameObject shakeObject)
    // {

    // }

    public void FX_BattleResultEffect(int GotExpValue, bool isLevelUp, int GotGoldValue)
    {
        BattleResultEffectGroup[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Exp +" + GotExpValue.ToString();
        BattleResultEffectGroup[0].SetActive(true);
        if (isLevelUp)
        {
            for (int i = 1; i < 6; i++)
            {
                BattleResultEffectGroup[i].SetActive(true);
            }
        }
        BattleResultEffectGroup[6].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Gold +" + GotGoldValue.ToString();
        BattleResultEffectGroup[6].SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence
        .Append(BattleResultEffectParent.transform.DOLocalMoveX(946, 0.125f))
        .AppendInterval(3f)
        .Append(BattleResultEffectParent.transform.DOLocalMoveX(1521, 0.125f))
        .OnComplete(() =>
        {
            for (int i = 0; i < BattleResultEffectGroup.Count; i++)
            {
                BattleResultEffectGroup[i].SetActive(false);
            }
        })
        .OnKill(() =>
        {
            for (int i = 0; i < BattleResultEffectGroup.Count; i++)
            {
                BattleResultEffectGroup[i].SetActive(false);
            }
        });
    }

    public void FX_AddSKillStorePool(List<SO_Skill> addskillList)
    {
        AddSkillPool.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "지금부터 아래의 스킬들이 상점에 등장";
        for (int i = 0; i < addskillList.Count; i++)
        {
            AddSkillPool.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text += "\n·";
            AddSkillPool.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text += addskillList[i].skillName;
        }

        Sequence sequence = DOTween.Sequence();
        sequence
        .Append(AddSkillPool.transform.DOLocalMoveX(964, 0.125f))
        .AppendInterval(3f)
        .Append(AddSkillPool.transform.DOLocalMoveX(1600, 0.125f));
    }
}
