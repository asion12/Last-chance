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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MakeSkillEffect(SO_Skill useSkill, bool isCasterPlayer)
    {
        Debug.LogWarning("Effect!");
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
            Debug.LogWarning("No Element");
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
            tempEffect.transform.Translate(-(SetVector - MoveVector).normalized * 0.5f);
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
                Debug.LogWarning("Hit Stopped!");
                Time.timeScale = 0f;
                yield return new WaitForSecondsRealtime(0.15f * HitValue);
                Debug.LogWarning("HitStopEnd!");
                Time.timeScale = 1f;

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
            // /Sequence seq = DOTween.Sequence();
        }
    }

    private int fxCount = 1;

    public IEnumerator MakeDamageInfoEffect(int DamageVal, bool isSuprised, bool isCritical, bool isAdditional, bool isMissed, bool isGuarded, bool isPlayerEffect)
    {
        fxCount = 1;

        bool[] effectActiveList = { false, };

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
        if (isCritical)
        {
            effectActiveList[3] = true;
            fxCount++;
        }
        if (isGuarded)
        {
            effectActiveList[4] = true;
            fxCount++;
        }

        if (isPlayerEffect)
        {
            fxCount++;
            FX_DamageEffect_PlayerBG();
            yield return new WaitForSeconds(1 / fxCount);
            FX_DamageEffect_Base(PlayerEffectBase);
        }
        else
            FX_DamageEffect_Base(EnemyEffectBase);

        yield return new WaitForSeconds(1 / fxCount);

        for (int i = 0; i < fxCount; i++)
        {
            if (effectActiveList[i])
            {
                if (isPlayerEffect)
                {
                    FX_DamageEffect(PlayerDamageEffectGroup[i]);
                    yield return new WaitForSeconds(1 / fxCount);
                }
                else
                {
                    FX_DamageEffect(EnemyDamageEffectGroup[i]);
                    yield return new WaitForSeconds(1 / fxCount);
                }
            }
        }
    }

    private void FX_DamageEffect_PlayerBG()
    {
        PlaeyrEffectBG.DOFade(0.75f, 1 / fxCount);
    }

    private void FX_DamageEffect_Base(GameObject effectBaseObject)
    {
        effectBaseObject.transform.DOKill();

        //effectBaseSet
        effectBaseObject.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);

        //effectBaseImageSet
        effectBaseObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(effectBaseObject.GetComponent<Image>().color.r, effectBaseObject.GetComponent<Image>().color.g, effectBaseObject.GetComponent<Image>().color.b, 0);

        //effectBaseDamageIntSet
        Color tempColor = effectBaseObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color;
        effectBaseObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = new Color(tempColor.r, tempColor.g, tempColor.b, 0);

        //EffectBaseInfoSet
        tempColor = effectBaseObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().color;
        effectBaseObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().color = new Color(tempColor.r, tempColor.g, tempColor.b, 0);


        effectBaseObject.SetActive(true);

        effectBaseObject.transform.DOScale(1, 1 / fxCount);
        effectBaseObject.transform.GetChild(0).gameObject.GetComponent<Image>().DOFade(1, 1 / fxCount);

    }

    private void FX_DamageEffect(GameObject effectObject)
    {
        effectObject.transform.DOKill();
        effectObject.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        effectObject.GetComponent<Image>().color = new Color(effectObject.GetComponent<Image>().color.r, effectObject.GetComponent<Image>().color.g, effectObject.GetComponent<Image>().color.b, 0);
        effectObject.SetActive(true);
        effectObject.transform.DOScale(1, 1 / fxCount);
        effectObject.GetComponent<Image>().DOFade(1, 1 / fxCount);
    }
    // private void ShakeObject(float duration, int vibrato, GameObject shakeObject)
    // {

    // }
}
