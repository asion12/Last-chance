using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [System.NonSerialized] public bool isGameStarted = false;
    public GameObject playerStartPoint;
    private Player player;
    // public Canvas ui;
    private bool canvas = false;
    [System.NonSerialized] public float nowTimeLimit = 0;
    [System.NonSerialized] public float maxTimeLimit = 180;
    public bool isTimeLimitOver = false;
    public static GameManager instance = null;

    private OutDungeonUIManager outDungeonUIManager;
    private UIManager uIManager;

    public int Gold = 0;
    public int HP_0 = 0;
    public int HP_1 = 0;
    public int HP_2 = 0;
    public int MP_0 = 0;
    public int MP_1 = 0;
    public int MP_2 = 0;

    public int potionCount = 0;

    private void Awake()
    {
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
        //CheckPotionCount();
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
        SceneManager.LoadScene("JJB-Dungeon", LoadSceneMode.Additive);
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
        player.transform.position = playerStartPoint.transform.position;
        player.nowHP = player.characterStats.MAX_HP;
        player.nowMP = player.characterStats.MAX_MP;
        player.nowCP = player.maxCP;
        player.GetComponent<Player>().isCanExit = false;
        SceneManager.UnloadSceneAsync("JJB-Dungeon");
        SceneManager.LoadScene("JJB-Dungeon", LoadSceneMode.Additive);
    }

    public void EnterDungeon()
    {
        outDungeonUIManager.InactiveOutDungeonUI();
        uIManager.ResetButtonPlayerSkillList();
        ResetDungeon();
        isGameStarted = true;
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
        outDungeonUIManager.ActiveOutDungeonUI();
    }

    public void DieOutDungeon()
    {
        Debug.Log("Dead!");
        Debug.Log("LosshavingSkill");
        LossSkills();
        player.skillList = new List<SO_Skill>();
        //player.SetTotalElements();
        ReSetPlayerTotalElements();
        outDungeonUIManager.ResetSkillSettedValue();
        ResetDungeon();
        isGameStarted = false;
        outDungeonUIManager.ActiveOutDungeonUI();
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
