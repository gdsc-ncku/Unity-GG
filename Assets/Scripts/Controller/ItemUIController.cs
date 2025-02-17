using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UniRx;

/// <summary>
/// �t�d�D��I�]������
/// </summary>
public class ItemUIController : MonoBehaviour
{
    [Header("Overview")]
    public Image itemImage;

    [Header("Gadgets")]
    public GameObject gadgetParent;
    public List<GameObject> gadgets = new List<GameObject>();

    [Header("Resources")]
    public GameObject resourceParent;
    public List<GameObject> resources = new List<GameObject>();

    [Header("Craftables")]
    public GameObject craftablesParent;
    public List<GameObject> craftables= new List<GameObject>();

    private void Start()
    {
        GetEachObject(gadgetParent, gadgets);
        GetEachObject(resourceParent, resources);
        GetEachObject(craftablesParent, craftables);

    }

    /// <summary>
    /// UI��ܧ󴫬����w������
    /// </summary>
    public void ItemChoosed(int index)
    {
        if (0 <= index && index < ItemManager.Instance.itemEnumName_itemsData_illustratedBook.Count)
        {
            ItemName choosed = (ItemName)index;
            Debug.Log($"ItemUIController: ��ܹD�� {ItemManager.Instance.itemEnumName_itemsData_illustratedBook[choosed].itemName}");
        }
        else
        {
            Debug.LogWarning("ItemUIController: ��ܤ��s�b���D��");
        }
    }

    /// <summary>
    /// ��sitem����ܸ�T �Ϥ�
    /// </summary>
    public void UpdateItem() {
        Dictionary<ItemData, int> inventory = InventoryManager.Instance.inventory;

        int gadgetIndex, craftableIndex;
        gadgetIndex = craftableIndex = 0;

        foreach (var entry in inventory)
        {
            Debug.Log($"- {entry.Key.itemName}: {entry.Value}");
            ItemData data = entry.Key;
            int num = entry.Value;
            
            if(data.itemType == ItemType.Drop)
            {
                craftableIndex = InitEachItemUI(craftableIndex, data, num, craftables);
            }
            else if(data.itemType == ItemType.Item)
            {
                gadgetIndex = InitEachItemUI(gadgetIndex, data, num, gadgets);
            }
        }


    }

    /// <summary>
    /// �z�L�]�w�������� �۰ʧ�����U���X�ӹD�����
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="objs"></param>
    private void GetEachObject(GameObject parent, List<GameObject> objs)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                objs.Add(parent.transform.GetChild(i).gameObject);
            }
        }

    private int InitEachItemUI(int index, ItemData data, int num, List<GameObject> objs)
    {
        //�Ϥ�
        Image img = objs[index].transform.GetChild(0).GetComponent<Image>();
        img.sprite = data.itemSprite;

        //�ƶq
        TextMeshProUGUI text = objs[index].transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        text.text = num.ToString();

        //����
        ItemBtn btn = objs[index].transform.GetChild(2).GetComponentInChildren<ItemBtn>();
        btn.nameIndex = data.itemEnumName;
        btn.isSetting = true;

        index++;
        return index;
    }

    #region event
    private CompositeDisposable disposables = new CompositeDisposable();

    private void OnEnable()
    {
        // ���U��  �ƥ󪺭q�\
        disposables.Add(EventManager.StartListening<int>(
            NameOfEvent.ItemChoosed,
            (index) => ItemChoosed(index)
        ));
    }

    private void OnDisable()
    {
        disposables.Clear(); // �۰ʨ����Ҧ��ƥ�q�\
    }
    #endregion
}
