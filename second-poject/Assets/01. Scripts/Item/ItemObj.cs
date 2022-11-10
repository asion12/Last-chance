using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType : int
{
    //���
    Head,
    Body,
    LeftHand,
    RightHand,
    //�Һ�
    use,
    //��ų
    useSkill,
}
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/new Item")]
public class ItemObj : ScriptableObject
{
    public ItemType itemType;//������ ������ �̳� ����ҷ��� ����
    public bool getFlagStackable;//��ĥ���ִ��� üũ
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
