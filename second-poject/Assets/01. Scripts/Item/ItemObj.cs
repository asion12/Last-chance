using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType : int
{
    //장비
    Head,
    Body,
    LeftHand,
    RightHand,
    //소비
    use,
    //스킬
    useSkill,
}
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/new Item")]
public class ItemObj : ScriptableObject
{
    public ItemType itemType;//위에쓴 아이템 이넘 사용할려는 변수
    public bool getFlagStackable;//합칠수있는지 체크
    public Sprite itemIcon;
    public GameObject objModelPrefab;

    public Item itemData = new Item();

    public List<string> boneNameLists = new List<string>();

    private void OnValidate()
    {
        boneNameLists.Clear();
    }
 public Item createItemObj()
    {
        Item new_Item = new Item(this);
        return new_Item;
    }
}
