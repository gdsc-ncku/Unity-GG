using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UniRx;
using System;
using System.Security.Cryptography;
using NUnit.Framework.Interfaces;

/// <summary>
/// �t�d�D��I�]��UI����
/// </summary>
public class ItemUIController : MonoBehaviour
{
    [Header("Overview")]
    public Image itemImage;
    public TextMeshProUGUI itemText;

    [Header("Gadgets")]
    public GameObject gadgetParent;
    public List<GameObject> gadgets = new List<GameObject>();

    [Header("Resources")]
    public GameObject resourceParent;
    public List<GameObject> resources = new List<GameObject>();

    [Header("Craftables")]
    public GameObject craftablesParent;
    public List<GameObject> craftables= new List<GameObject>();

    //��ƨӷ�
    [HideInInspector] public Dictionary<ItemName, ItemData> itemEnumName_itemsData_illustratedBook = new Dictionary<ItemName, ItemData>(); //�Ҧ���item��T

    private void Start()
    {
        //�����e���X�ӥi��ܦ�m
        GetEachObject(gadgetParent, gadgets);
        GetEachObject(resourceParent, resources);
        GetEachObject(craftablesParent, craftables);

        //�]�w�D���ƨӷ�
        itemEnumName_itemsData_illustratedBook = ItemManager.Instance.itemEnumName_itemsData_illustratedBook;
    }

    /// <summary>
    /// �Ȯɩʪ������\��j��o��
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)){
            ItemTrigger();
        }
    }

    /// <summary>
    /// Ĳ�o�D��
    /// </summary>
    public void ItemTrigger()
    {
        ItemManager.Instance.ItemTrigger();
        UpdateItem();
    }

    /// <summary>
    /// UI��ܧ󴫬����w������
    /// </summary>
    public void ItemChoosed(int index)
    {
        if (0 <= index && index < itemEnumName_itemsData_illustratedBook.Count)
        {
            ItemName choosed = (ItemName)index;
            ItemData itemData = itemEnumName_itemsData_illustratedBook[choosed];

            itemImage.sprite = itemData.itemSprite;
            itemText.text = $"�D��W��: {itemData.itemName}\n" +
                $"�D��y�z: {itemData.itemDescription}\n";

            Debug.Log($"ItemUIController: ��ܹD�� {itemData.itemName}");
        }
        else
        {
            Debug.LogWarning("ItemUIController: ��ܤ��s�b���D��");
        }
    }

    /// <summary>
    /// ��sitem�b�I�]����ܸ�T �Ϥ�
    /// </summary>
    public void UpdateItem() {
        //��ܤ��e ���M���¸��
        CleanAllUIData();

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

    /// <summary>
    /// �M�ũҦ����¸�T
    /// </summary>
    private void CleanAllUIData()
    {
        CleanEachUI(gadgets);
        CleanEachUI(craftables);
        CleanEachUI(resources);

        itemImage.sprite = null;
        itemText.text = $"�D��W��: \n" +
            $"�D��y�z: \n";
    }

    /// <summary>
    /// �M�ų�����O���¸�T
    /// </summary>
    /// <param name="objs"></param>
    private void CleanEachUI(List<GameObject> objs)
    {
        foreach (var obj in objs)
        {
            //�Ϥ�
            Image img = obj.transform.GetChild(0).GetComponent<Image>();
            img.sprite = null;

            //�ƶq
            TextMeshProUGUI text = obj.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
            text.text = "x";

            //����
            if(obj.transform.childCount >= 3)
            {
                ItemBtn btn = obj.transform.GetChild(2).GetComponentInChildren<ItemBtn>();
                btn.isSetting = false;
            }
        }
    }

    /// <summary>
    /// ��l�ƦU�ӹD�㪫��UI��T
    /// </summary>
    /// <param name="index"></param>
    /// <param name="data"></param>
    /// <param name="num"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    private int InitEachItemUI(int index, ItemData data, int num, List<GameObject> objs)
    {
        //�W�L��ܤW��
        if (index >= objs.Count) return index;

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
