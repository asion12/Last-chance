using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox_new : MonoBehaviour
{
    public List<SO_Skill> AddSkillPollList;
    private StoreManager_New storeManager_New;
    private EffectManager effectManager;

    private void Awake()
    {
        storeManager_New = FindObjectOfType<StoreManager_New>();
        effectManager = FindObjectOfType<EffectManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.LogWarning("스킬 획득함");
            AddStoreSkillPool();
            effectManager.FX_AddSKillStorePool(AddSkillPollList);
            Destroy(gameObject);
        }
    }

    private void AddStoreSkillPool()
    {
        for (int i = 0; i < AddSkillPollList.Count; i++)
        {
            storeManager_New.StoreSkillPool.Add(AddSkillPollList[i]);
        }
    }
}
