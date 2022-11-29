using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class StoreUIManager : MonoBehaviour
{
    [SerializeField] private Text Prise;
    [SerializeField] private Text Gold;
    [SerializeField] private Text HPPortion;
    [SerializeField] private Text MPPortion;
    private int prise = 1000;
    public Image falseBuy;
    void Start()
    {
        
    }
    public void SetText<T>(Text text, T value)
    {
        text.text = value.ToString();
    }
    void Update()
    {
        SetText(Prise, prise);
        SetText(Gold, GameManager.instance.Gold);
        SetText(HPPortion, GameManager.instance.Hp_0);
       // SetText(MPPortion, GameManager.instance.MPPortion);
    }
    public void SellGoldItem()
    {
        if (GameManager.instance.Hp_0 >= 1)
        {
            GameManager.instance.Hp_0 -= 1;
            GameManager.instance.Gold += 1000;
            prise -= 100;
        }

    }
    public void DontBuy()
    {
        if (GameManager.instance.Gold >= 1000)
        {
            GameManager.instance.Gold -= 1000;
            GameManager.instance.Hp_0 += 1;
            prise += 100;
        }
        else if (GameManager.instance.Gold<1000)
        {
           
            falseBuy.gameObject.SetActive(true);
            Invoke("OnImage", 1);
        }

    }
    private void OnImage()
    {
        falseBuy.gameObject.SetActive(false);
    }
}
