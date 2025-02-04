using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [System.NonSerialized] public bool isGameStarted = false;
    public GameObject playerStartPoint;
    public GameObject playerOutDungeonPoint;
    private Player player;
    // public Canvas ui;
    private bool canvas = false;
    [System.NonSerialized] public float nowTimeLimit = 0;
    [System.NonSerialized] public float maxTimeLimit = 180;
    public bool isTimeLimitOver = false;
    public static GameManager instance = null;

    private OutDungeonUIManager outDungeonUIManager;
    private UIManager uIManager;
    private StoreManager_New storeManager_New;

    public int Gold = 0;
    public int HP_0 = 0;
    public int HP_1 = 0;
    public int HP_2 = 0;
    public int MP_0 = 0;
    public int MP_1 = 0;
    public int MP_2 = 0;

    public int potionCount = 0;

    [SerializeField] private GameObject GameClearUI;
    [SerializeField] private TextMeshProUGUI ClearInfoText;
    public int DungeonTryCount = 0;
    public int KilledEnemyCount = 0;
    public int SkillCastCount = 0;
    public int UsedPotionCount = 0;
    private void Awake()
    {
        storeManager_New = FindObjectOfType<StoreManager_New>();
        uIManager = FindObjectOfType<UIManager>();
        outDungeonUIManager = FindObjectOfType<OutDungeonUIManager>();
        ActiveDungeon();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (isGameStarted)
        {
            if (BattleManager.instance.nowTurnID == 0)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        // CursorLook();
        TimeLimitCheck();
        CheckPotionUse();
        //CheckPotionCount();
    }

    private void CheckPotionUse()
    {
        if (BattleManager.instance.nowTurnID == 0 && isGameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (HP_0 > 0 && player.nowHP < player.characterStats.MAX_HP)
                {
                    HP_0--;
                    player.nowHP += 500;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (HP_1 > 0 && player.nowHP < player.characterStats.MAX_HP)
                {
                    HP_1--;
                    player.nowHP += 1500;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (HP_2 > 0 && player.nowHP < player.characterStats.MAX_HP)
                {
                    HP_2--;
                    player.nowHP += 3000;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (MP_0 > 0 && player.nowMP < player.characterStats.MAX_MP)
                {
                    HP_1--;
                    player.nowMP += 150;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                if (MP_1 > 0 && player.nowMP < player.characterStats.MAX_MP)
                {
                    MP_1--;
                    player.nowMP += 500;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                if (MP_2 > 0 && player.nowMP < player.characterStats.MAX_MP)
                {
                    MP_2--;
                    player.nowMP += 1000;
                }
            }
        }
    }

    private void TimeLimitCheck()
    {
        if (isGameStarted)
        {
            nowTimeLimit += Time.deltaTime;
        }
        if (maxTimeLimit - nowTimeLimit < 0)
        {
            isTimeLimitOver = true;
        }
    }


    // public void CursorLook()
    // {
    //     if (Input.GetKeyDown(KeyCode.P))
    //     {
    //         if (ui.gameObject.activeSelf)
    //         {
    //             ui.gameObject.SetActive(false);
    //         }
    //         else
    //         {
    //             ui.gameObject.SetActive(true);
    //         }
    //     }
    // }

    // public void testclickGoldup()
    // {
    //     GameManager.instance.Gold += 1000;
    //     Debug.Log(Gold);
    // }

    public void ActiveDungeon()
    {
        SceneManager.LoadSceneAsync("JJB-Dungeon", LoadSceneMode.Additive);
    }

    public void CheckPotionCount()
    {
        potionCount = HP_0 + HP_1 + HP_2 + MP_0 + MP_1 + MP_2;
        Debug.Log("Potion Count " + potionCount.ToString());
    }

    public void ResetDungeon()
    {
        // /isGameStarted = false;
        isTimeLimitOver = false;
        nowTimeLimit = 0;
        player.transform.position = playerOutDungeonPoint.transform.position;
        player.nowHP = player.characterStats.MAX_HP;
        player.nowMP = player.characterStats.MAX_MP;
        player.nowCP = player.maxCP;
        player.GetComponent<Player>().isCanExit = false;
        SceneManager.UnloadSceneAsync("JJB-Dungeon");
        SceneManager.LoadSceneAsync("JJB-Dungeon", LoadSceneMode.Additive);
    }

    public void EnterDungeon()
    {
        outDungeonUIManager.InactiveOutDungeonUI();
        uIManager.ResetButtonPlayerSkillList();
        DungeonTryCount++;
        //ResetDungeon();
        player.transform.position = playerStartPoint.transform.position;
        isGameStarted = true;
        storeManager_New.BuyAllOrderedSkills();
    }

    public void ExitDungeon()
    {
        Debug.Log("Exit!");
        ResetSkills();
        player.skillList = new List<SO_Skill>();
        //player.SetTotalElements();
        ReSetPlayerTotalElements();
        outDungeonUIManager.ResetSkillSettedValue();
        ResetDungeon();
        isGameStarted = false;
        storeManager_New.ResetSkillStore();
        outDungeonUIManager.ActiveOutDungeonUI();
    }

    public void DieOutDungeon()
    {
        Debug.Log("Dead!");
        Debug.Log("LosshavingSkill");
        LossSkills();
        LossPotions();
        player.skillList = new List<SO_Skill>();
        //player.SetTotalElements();
        ReSetPlayerTotalElements();
        outDungeonUIManager.ResetSkillSettedValue();
        ResetDungeon();
        isGameStarted = false;
        storeManager_New.ResetSkillStore();
        outDungeonUIManager.ActiveOutDungeonUI();
    }

    public void ClearDungeon()
    {
        ExitDungeon();

        ClearInfoText.text = "";
        ClearInfoText.text += "\n던전에 총 " + DungeonTryCount.ToString() + "번 도전";
        ClearInfoText.text += $"\n총 {KilledEnemyCount.ToString()}마리의 몬스터 처치";
        ClearInfoText.text += $"\n총 {UsedPotionCount.ToString()}의 포션을 사용";
        ClearInfoText.text += $"\n총 {SkillCastCount.ToString()}번 스킬을 시전";

        GameClearUI.SetActive(true);
    }

    private void ReSetPlayerTotalElements()
    {
        player.totalResistElements.SOLAR = false;
        player.totalResistElements.LUMINOUS = false;
        player.totalResistElements.IGNITION = false;
        player.totalResistElements.HYDRO = false;
        player.totalResistElements.BIOLOGY = false;
        player.totalResistElements.METAL = false;
        player.totalResistElements.SOIL = false;

        player.totalWeakElements.SOLAR = false;
        player.totalWeakElements.LUMINOUS = false;
        player.totalWeakElements.IGNITION = false;
        player.totalWeakElements.HYDRO = false;
        player.totalWeakElements.BIOLOGY = false;
        player.totalWeakElements.METAL = false;
        player.totalWeakElements.SOIL = false;
    }
    // private void ResetPlayerSkillSettedValue()
    // {
    //     for (int i = 0; i < outDungeonUIManager.PlayerInventorySkillListData.Count; i++)
    //     {
    //         outDungeonUIManager.PlayerInventorySkillListData[i].playerSkillSetted = false;
    //     }
    // }
    private void CheckPlayerSkillSet()
    {

    }

    private void LossPotions()
    {
        HP_0 = 0;
        HP_1 = 0;
        HP_2 = 0;

        MP_0 = 0;
        MP_1 = 0;
        MP_2 = 0;
    }

    private void LossSkills()
    {
        for (int i = 0; i < player.skillList.Count; i++)
        {
            player.skillList[i].playerHavingCount--;
        }
        player.skillList = new List<SO_Skill>();
        outDungeonUIManager.ResetPlayerSkillInventory();
    }

    private void ResetSkills()
    {
        player.skillList = new List<SO_Skill>();
        outDungeonUIManager.ResetPlayerSkillInventory();
    }
}
