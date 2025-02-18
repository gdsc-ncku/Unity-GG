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

        ItemManager.Instance.ItemChoosed(index);
    }

    /// <summary>
    /// �ͦ��Ҧ��D���I�]���U
    /// </summary>
    public void RandomInitItemInPackage()
    {
        for (int i = 0; i < itemEnumName_itemsPrefabs_illustratedBook.Count; i++)
        {
            ItemData data = itemEnumName_itemsData_illustratedBook[(ItemName)i];
            InventoryManager.Instance.AddItem(data.itemEnumName, 1);
        }
    }

    /// <summary>
    /// �ͦ���e�Ҧ����D�����W �Ω�}�o��DEBUG
    /// </summary>
    public void RandomInstantiateItem()
    {
        int range = instantiateRange;
        for (int i = 0; i < itemEnumName_itemsPrefabs_illustratedBook.Count; i++)
        {
            ItemData data = itemEnumName_itemsData_illustratedBook[(ItemName)i];
            if (data.itemType == ItemType.Drop)
            {
                GameObject obj = Instantiate(itemEnumName_itemsPrefabs_illustratedBook[(ItemName)i], itemInstantiatePoint);
                obj.transform.localPosition = new Vector3(Random.Range(-range, range),
                                                        Random.Range(1, range),
                                                        Random.Range(-range, range));

                //�]�m�S��
                GameObject partical = Instantiate(particalEffect, obj.transform);

                //�]�m�W��
                GameObject canva = Instantiate(itemCanva, obj.transform);
                canva.transform.localPosition = new Vector3(0, 1f, 0);
                canva.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = obj.GetComponent<Item>().itemData.itemName;

            }
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

        ItemManager.Instance.AddItem(index);
    }
}
