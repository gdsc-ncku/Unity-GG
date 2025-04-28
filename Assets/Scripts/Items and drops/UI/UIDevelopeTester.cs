using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// �Ω���չD��UI�}�o �}�o�̪��}��
/// </summary>
public class UIDevelopeTester : MonoBehaviour
{
    [Header("For develop")]
    //�}�o�ϥΰ�
    [SerializeField] private ItemName wantAddItem;
    [SerializeField] private TMP_InputField wantedAddIndex;
    [SerializeField] private TMP_InputField inputChooseIndex;
    [SerializeField] private Transform itemInstantiatePoint;    //����ͦ���m
    [SerializeField] private int instantiateRange = 5;
    [SerializeField] private GameObject particalEffect;
    [SerializeField] private GameObject itemCanva;

    [HideInInspector] public Dictionary<ItemName, GameObject> itemEnumName_itemsPrefabs_illustratedBook = new Dictionary<ItemName, GameObject>(); //�Ҧ���item��T
    [HideInInspector] public Dictionary<ItemName, ItemData> itemEnumName_itemsData_illustratedBook = new Dictionary<ItemName, ItemData>(); //�Ҧ���item��T

    private void Start()
    {
        itemEnumName_itemsPrefabs_illustratedBook = ItemManager.Instance.itemEnumName_itemsPrefabs_illustratedBook;
        itemEnumName_itemsData_illustratedBook = ItemManager.Instance.itemEnumName_itemsData_illustratedBook;
    }

    /// <summary>
    /// Ĳ�o�D��
    /// </summary>
    public void ItemTrigger()
    {
        ItemManager.Instance.ItemTrigger();
    }

    /// <summary>
    /// �}�o�̥Ϊ��h�� ���J���w��J�ت�index
    /// </summary>
    /// <param name="index">��ܪ��s��</param>
    public void ItemChoosed()
    {
        //�p�Gindex �O-1 �N��O�Ѷ}�o���ǥѴ���Ĳ�o ���J���խ��O����T
        int index = int.Parse(inputChooseIndex.text);
        ItemName itemName = (ItemName)index;

        ItemManager.Instance.ItemChoosed(itemName);
    }

    /// <summary>
    /// �ͦ��Ҧ��D���I�]���U
    /// </summary>
    public void RandomInitItemInPackage()
    {
        foreach(var dataPair in ItemManager.Instance.itemEnumName_itemsData_illustratedBook)
        {
            ItemData data = dataPair.Value;
            if (data != null)
            {
                InventoryManager.Instance.AddItem(data.itemEnumName, 1);
            }
        }

        //��s�D����ܤ���
        EventManager.TriggerEvent(NameOfEvent.UpdateItem);
    }

    /// <summary>
    /// �ͦ���e�Ҧ����D�����W �Ω�}�o��DEBUG
    /// </summary>
    public void RandomInstantiateItem()
    {
        int range = instantiateRange;
        foreach (var dataPair in ItemManager.Instance.itemEnumName_itemsData_illustratedBook)
        {
            ItemName itemName = dataPair.Key;
            ItemManager.Instance.InstantiateItemObject(itemName);
        }
    }

    /// <summary>
    /// ���եΨ��
    /// �K�[�@�Ӫ���i�Jmanager
    /// </summary>
    public void AddItem()
    {
        //GameObject obj = itemEnumName_items_illustratedBook[wantAddItem];
        //GameObject item = Instantiate(obj, this.transform);

        //package.Add(item);

        int index = int.Parse(wantedAddIndex.text);
        ItemName itemName = (ItemName)index;
        ItemManager.Instance.AddItem(itemName);
    }
}
