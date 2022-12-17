using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

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
    // Start is called before the first frame update
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
            tempEffect.transform.Translate(-(SetVector - MoveVector).normalized * -1f);
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
            // /Sequence seq = DOTween.Sequence();
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
    }

    // private void ShakeObject(float duration, int vibrato, GameObject shakeObject)
    // {

    // }
}
