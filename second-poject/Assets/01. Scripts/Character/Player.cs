using UnityEngine;
using cakeslice;
using DG.Tweening;

public class Player : Character
{
    public int EXP = 0;
    public int maxEXP = 100;
    public int statPoint = 0;
    // about Battle Start Check
    [SerializeField] private Camera playerCamera = null;
    [SerializeField] private float battleStartRange = 0f;
    [SerializeField] private GameObject battleStartRangePoint = null;

    private GameObject hitObject = null;
    private GameObject lastHitData = null;

    // about move
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotateSpeed;
    private bool freeze = false;

    private float xRotate, yRotate, xRotateMove, yRotateMove;

    private UIManager uIManager = null;
    public bool[] isSkillOverClockList = { false, };

    float cpIncreaseTimer = 0;
    float cpIncreaseTimer_MAX = 2f;

    float cpDecreaseTimer = 0;
    float cpDecreaseTimer_MAX = 0.5f;

    //public ItemDBObj itemDBObj;
    //public InventoryObj inventoryObj;
    //public GameObject box;
    //public GameObject items;

    public Transform poistion;

    public bool isCanExit = false;


    public int skillPoint = 0;
    private OutDungeonUIManager outDungeonUIManager;
    private void Awake()
    {
        outDungeonUIManager = FindObjectOfType<OutDungeonUIManager>();
    }
    protected virtual void Start()
    {
        base.Start();
        uIManager = FindObjectOfType<UIManager>();
    }

    protected override void Update()
    {
        base.Update();
        //Debug.DrawLine(playerCamera.transform.position, playerCamera.transform.forward, Color.red);
        if (GameManager.instance.isGameStarted)
        {
            if ((!isBattleMode || isStunned) && !uIManager.isInvenOn)
            {
                CheckIncreaseCP();
                CameraRotateToMousePointer();
                CharacterMove();
                CanBattleStartByRayCast();
                //ItemBoxCheck();
                //itemcheck();
            }
            else if (isBattleMode)
            {
                CheckDecreaseCP();
            }
            TargetCharacterRayCheck();
            CheckTargetEnemyLevelAndScaleStats();
            ExitCheck();
        }
        CheckExpOver();
        SetMaxEXP();
    }

    private void CheckExpOver()
    {
        if (EXP >= maxEXP)
        {
            EXP = EXP - maxEXP;
            Level++;
        }
    }

    private void SetMaxEXP()
    {
        maxEXP = (int)(20 * GetNewMaxEXP(Level + 1));
    }

    private void CheckIncreaseCP()
    {
        cpIncreaseTimer += Time.deltaTime;
        if (cpIncreaseTimer >= cpIncreaseTimer_MAX / ((100 + totalStats.CHA) / 100))
        {
            cpIncreaseTimer = 0;
            nowCP++;
            if (nowCP > maxCP)
            {
                nowCP = maxCP;
            }
        }
    }

    private void CheckDecreaseCP()
    {
        cpDecreaseTimer += Time.deltaTime;
        if (cpDecreaseTimer >= cpDecreaseTimer_MAX * ((100 + totalStats.FOC) / 100))
        {
            cpDecreaseTimer = 0;
            nowCP--;
            if (nowCP < 0)
            {
                nowCP = 0;
            }
        }
    }

    private void CheckTargetEnemyLevelAndScaleStats()
    {
        if (BattleManager.instance.targetEnemy != null)
        {
            float scaleSet = GetLevelScale_forBattle(Level - BattleManager.instance.targetEnemy.Level);
            buff_debuffStats.STR = characterStats.STR * scaleSet;
            buff_debuffStats.FIR = characterStats.FIR * scaleSet;
            buff_debuffStats.INT = characterStats.INT * scaleSet;
            buff_debuffStats.WIS = characterStats.WIS * scaleSet;
            buff_debuffStats.DEX = characterStats.DEX * scaleSet;
            buff_debuffStats.FOC = characterStats.FOC * scaleSet;
            buff_debuffStats.CHA = characterStats.CHA * scaleSet;
        }
        else
        {
            buff_debuffStats.STR = 0;
            buff_debuffStats.FIR = 0;
            buff_debuffStats.INT = 0;
            buff_debuffStats.WIS = 0;
            buff_debuffStats.DEX = 0;
            buff_debuffStats.FOC = 0;
            buff_debuffStats.CHA = 0;
        }
    }

    public float GetLevelScale_forBattle(float temp)
    {
        if (temp < 0)
        {
            temp *= -1;
            temp = -1 * ((-2 / (temp + 2f)) + 1f) / 2;
        }
        else if (temp > 0)
        {
            temp = ((-2 / (temp + 2f)) + 1f) / 2;
        }
        else
        {
            temp = 0;
        }
        return temp;
    }

    public float GetNewMaxEXP(float temp)
    {
        temp = (-1 * ((10) / (temp + 4)) + 2) * 5;

        return temp;
    }

    private void CheckOverClockAndAddWeakElement()
    {
        Elements_int tempEl_int = additionWeakElements;
        for (int i = 0; i < isSkillOverClockList.Length; i++)
        {
            if (isSkillOverClockList[i])
            {
                if (skillList[i].skillElements.SOLAR)
                {
                    tempEl_int.SOLAR++;
                }
                else if (skillList[i].skillElements.LUMINOUS)
                {
                    tempEl_int.LUMINOUS++;
                }
                else if (skillList[i].skillElements.IGNITION)
                {
                    tempEl_int.IGNITION++;
                }
                else if (skillList[i].skillElements.HYDRO)
                {
                    tempEl_int.HYDRO++;
                }
                else if (skillList[i].skillElements.BIOLOGY)
                {
                    tempEl_int.BIOLOGY++;
                }
                else if (skillList[i].skillElements.METAL)
                {
                    tempEl_int.METAL++;
                }
                else if (skillList[i].skillElements.SOIL)
                {
                    tempEl_int.SOIL++;
                }
            }
        }
        additionWeakElements = tempEl_int;
    }

    public void CameraRotateToTarget(GameObject target)
    {
        Vector3 rotateDistance = target.transform.position - playerCamera.transform.position;
        Vector3 toRotate = Quaternion.LookRotation(rotateDistance, Vector3.up).eulerAngles;
        //toRotate = Quaternion.Euler(toRotate.x, toRotate.y, 0);
        playerCamera.transform.DORotate(toRotate, 1).SetEase(Ease.OutExpo);
        //playerCamera.transform.rotation = Quaternion.Euler(playerCamera.transform.rotation.x, playerCamera.transform.rotation.y, 0);
    }

    private void CameraRotateToMousePointer()
    {
        xRotateMove = -Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeed;
        yRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed;

        yRotate = playerCamera.transform.eulerAngles.y + yRotateMove;
        xRotate = xRotate + xRotateMove;

        xRotate = Mathf.Clamp(xRotate, -90, 90);

        transform.eulerAngles = new Vector3(0, yRotate, 0);
        playerCamera.transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
    }

    public GameObject nowHitObject;
    private void CanBattleStartByRayCast()
    {
        //Debug.Log("Now Raying");

        RaycastHit hitData;
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hitData))
        {
            if (hitData.transform.tag == "Enemy")
            {
                if (uIManager == null)
                { Debug.Log("uIManager is null!"); }
                else
                {
                    GameObject temp = hitData.transform.gameObject;
                }
                //Debug.Log("RayIn!!");
                if (hitData.distance <= battleStartRange)
                {
                    lastHitData = hitData.transform.gameObject;

                    if (lastHitData.transform.GetComponent<Outline>() == null)
                    {
                        lastHitData.transform.GetChild(0).GetComponent<Outline>().eraseRenderer = false;
                    }
                    else
                    {
                        lastHitData.transform.GetComponent<Outline>().eraseRenderer = false;
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Battle Ready");
                        bool checkStun = lastHitData.transform.GetComponent<Enemy>().isStunned;
                        bool checkCareless = false;
                        if (checkStun || checkCareless)
                        {
                            Debug.Log("Enemy Careless Start!");
                            BattleManager.instance.BattleStart(true, true, lastHitData);
                        }
                        else
                        {
                            Debug.Log("Enemy Not Careless Start!");
                            BattleManager.instance.BattleStart(true, false, lastHitData);
                        }
                    }
                }
                else
                {
                    RayOutCheck();
                }
            }
            else
            {
                RayOutCheck();
            }
            // GameObject hitObj;
            // hitObj = Instantiate(hitObject, hitData.point, Quaternion.identity);
            // hitObj.transform.SetParent(null);
        }
        else
        {
            RayOutCheck();
        }
        // Debug.Log("last hit is " + lastHitData);
        // Debug.Log("now hit is" + hitData.transform.gameObject);
    }

    private void TargetCharacterRayCheck()
    {
        RaycastHit hitData;
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hitData))
        {
            if (hitData.transform.tag == "Enemy")
            {
                if (uIManager == null)
                { Debug.Log("uIManager is null!"); }
                else
                {
                    GameObject temp = hitData.transform.gameObject;
                    uIManager.UIUpdate_TargetEnemyBase(temp, true);
                }
            }
            else
            {
                uIManager.UIUpdate_OffTargetEnemyBase();
            }
        }
        else
        {
            uIManager.UIUpdate_OffTargetEnemyBase();
        }
    }
    // private void ItemBoxCheck()
    // {
    //     RaycastHit hit;
    //     Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    //     if (Physics.Raycast(ray, out hit))
    //     {
    //         if (hit.transform.tag == "Box")
    //         {
    //             Debug.Log("start");
    //             if (Input.GetKeyDown(KeyCode.F))
    //             {
    //                 items.gameObject.SetActive(true);
    //                 Destroy(box.gameObject);

    //             }
    //         }

    //     }
    // }
    // private void itemcheck()
    // {
    //     RaycastHit hit;
    //     Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    //     if (Physics.Raycast(ray, out hit))
    //     {
    //         if (hit.transform.tag == "items")
    //         {
    //             if (Input.GetKeyDown(KeyCode.V))
    //             {
    //                 AddnewItem();
    //                 Destroy(items);

    //             }
    //         }

    //     }

    // }

    // public void AddnewItem()
    // {
    //     if (itemDBObj.itemObjs.Length > 0)
    //     {
    //         ItemObj newItemObject = itemDBObj.itemObjs[Random.Range(0, itemDBObj.itemObjs.Length)];
    //         Item newItem = new Item(newItemObject);
    //         inventoryObj.AddItem(newItem, 1);
    //         Debug.Log("ȹ��");

    //     }
    // }

    private void RayOutCheck()
    {
        //Debug.Log("Ray Checking");
        if (lastHitData != null)
        {
            Debug.Log("RayOut!!");
            if (lastHitData.transform.GetComponent<Outline>() == null)
            {
                lastHitData.transform.GetChild(0).GetComponent<Outline>().eraseRenderer = true;
            }
            else
            {
                lastHitData.transform.GetComponent<Outline>().eraseRenderer = true;
            }
        }
        lastHitData = null;
    }

    private void CharacterMove()
    {
        float moveX = Input.GetAxisRaw("Vertical");
        float moveY = Input.GetAxisRaw("Horizontal");

        var pos = transform.position;

        pos += transform.forward * moveX * moveSpeed * Time.deltaTime;
        pos += transform.right * moveY * moveSpeed * Time.deltaTime;

        transform.position = pos;
    }

    private void ExitCheck()
    {
        if (isCanExit)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                GameManager.instance.ExitDungeon();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ExitPannel")
        {
            isCanExit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ExitPannel")
        {
            isCanExit = false;
        }
    }

    public void RemoveSkillFromList(SO_Skill removeSkill)
    {
        //SetTotalElements();
        removeSkill.playerSkillSetted = false;
        skillList.Remove(removeSkill);
        SetTotalElements();
        uIManager.ResetPlayerSkillList();
        //outDungeonUIManager.DungeonEnterCheck();
    }

    public void AddSkillToList(SO_Skill addSkill)
    {
        //SetTotalElements();
        addSkill.playerSkillSetted = true;
        skillList.Add(addSkill);
        uIManager.ResetPlayerSkillList();
        Debug.Log("Skill Set Complete_Add");
        SetTotalElements();
        //outDungeonUIManager.DungeonEnterCheck();
    }
}