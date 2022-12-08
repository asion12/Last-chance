using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class StoreUIManager : MonoBehaviour
{
    [SerializeField] private Text Prise;
    [SerializeField] private Text Gold;
    [SerializeField] private Text HP_0;
    [SerializeField] private Text HP_1;
    [SerializeField] private Text HP_2;
    [SerializeField] private Text MP_0;
    [SerializeField] private Text MP_1;
    [SerializeField] private Text MP_2;
    private int prise = 1000;
    public Image falseBuy;
    public InventoryObj equipObj;
    public InventoryObj inventoryObj;
    public ItemDBObj itemDBObj;
    public Canvas store;
    public bool canvas = false;
    void Start()
    {

    }
    public void SetText<T>(Text text, T value)
    {
        text.text = value.ToString();
    }
    void Update()
    {
        on();
        SetText(Prise, prise);
        SetText(Gold, GameManager.instance.Gold);
        SetText(HP_0, GameManager.instance.Hp_0);
        SetText(HP_1, GameManager.instance.HP_1);
        SetText(HP_2, GameManager.instance.HP_2);
        SetText(MP_0, GameManager.instance.MP_0);
        SetText(MP_1, GameManager.instance.MP_1);
        SetText(MP_2, GameManager.instance.MP_2);
        // SetText(MPPortion, GameManager.instance.MPPortion);
    }
    public void SellGoldItem()
    {
        if (GameManager.instance.Hp_0 >= 1)
        {
            GameManager.instance.Hp_0 -= 1;
            GameManager.instance.Gold += 1000;
        }

    }
    public void SellGoldItem1()
    {
        if (GameManager.instance.HP_1 >= 1)
        {
            GameManager.instance.HP_1 -= 1;
            GameManager.instance.Gold += 2000;
        }

    }
    public void SellGoldItem2()
    {
        if (GameManager.instance.HP_2 >= 1)
        {
            GameManager.instance.HP_2 -= 1;
            GameManager.instance.Gold += 3000;
        }

    }
    public void DontBuy()
    {
        if (GameManager.instance.Gold >= 1000)
        {
            GameManager.instance.Gold -= 1000;
            GameManager.instance.Hp_0 += 1;
        }
        else if (GameManager.instance.Gold < 1000)
        {

            falseBuy.gameObject.SetActive(true);
            Invoke("OnImage", 1);
        }
    }
    public void DontBuy1()
    {
        if (GameManager.instance.Gold >= 2000)
        {
            GameManager.instance.Gold -= 2000;
            GameManager.instance.HP_1 += 1;
        }
        else if (GameManager.instance.Gold < 2000)
        {

            falseBuy.gameObject.SetActive(true);
            Invoke("OnImage", 1);
        }

    }
    public void DontBuy2()
    {
        if (GameManager.instance.Gold >= 3000)
        {
            GameManager.instance.Gold -= 3000;
            GameManager.instance.HP_2 += 1;
        }
        else if (GameManager.instance.Gold < 3000)
        {

            falseBuy.gameObject.SetActive(true);
            Invoke("OnImage", 1);
        }

    }

    public void SellGoldItemMP()
    {
        if (GameManager.instance.MP_0 >= 1)
        {
            GameManager.instance.MP_0 -= 1;
            GameManager.instance.Gold += 1000;
        }

    }
    public void SellGoldItemMP1()
    {
        if (GameManager.instance.MP_1 >= 1)
        {
            GameManager.instance.MP_1 -= 1;
            GameManager.instance.Gold += 2000;
        }

    }
    public void SellGoldItemMP2()
    {
        if (GameManager.instance.MP_2 >= 1)
        {
            GameManager.instance.MP_2 -= 1;
            GameManager.instance.Gold += 3000;
        }

    }
    public void DontBuyMP()
    {
        if (GameManager.instance.Gold >= 1000)
        {
            GameManager.instance.Gold -= 1000;
            GameManager.instance.MP_0 += 1;
            AddnewItem0();
        }
        else if (GameManager.instance.Gold < 1000)
        {

            falseBuy.gameObject.SetActive(true);
            Invoke("OnImage", 1);
        }

    }
    public void DontBuyMP1()
    {
        if (GameManager.instance.Gold >= 2000)
        {
            GameManager.instance.Gold -= 2000;
            GameManager.instance.MP_1 += 1;
        }
        else if (GameManager.instance.Gold < 2000)
        {

            falseBuy.gameObject.SetActive(true);
            Invoke("OnImage", 1);
        }

    }
    public void DontBuyMP2()
    {
        if (GameManager.instance.Gold >= 3000)
        {
            GameManager.instance.Gold -= 3000;
            GameManager.instance.MP_2 += 1;
        }
        else if (GameManager.instance.Gold < 3000)
        {

            falseBuy.gameObject.SetActive(true);
            Invoke("OnImage", 1);
        }

    }
    private void OnImage()
    {
        falseBuy.gameObject.SetActive(false);
    }
    public void AddnewItem0()
    {
        if (itemDBObj.itemObjs.Length > 0)
        {
            ItemObj newItemObject = itemDBObj.itemObjs[0];
            Item newItem = new Item(newItemObject);
            inventoryObj.AddItem(newItem, 1);

        }
    }

    private void on()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (canvas == true)
            {
                canvas = true;
            }
            else
            {
                canvas = false;
            }
        }
    }
}
