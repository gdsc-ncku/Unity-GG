using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// 負責道具背包的控制
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
    /// 更新item的顯示資訊 圖片
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
                Image img = craftables[craftableIndex].transform.GetChild(0).GetComponent<Image>();
                img.sprite = data.itemSprite;

                TextMeshProUGUI text = craftables[craftableIndex].transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
                text.text = num.ToString();

                craftableIndex++;
            }
            else if(data.itemType == ItemType.Item)
            {
                Image img = gadgets[gadgetIndex].transform.GetChild(0).GetComponent<Image>();
                img.sprite = data.itemSprite;

                TextMeshProUGUI text = craftables[craftableIndex].transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
                text.text = num.ToString();

                gadgetIndex++;
            }
        }


    }

    /// <summary>
    /// 透過設定的父物件 自動抓取底下有幾個道具欄位
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
}
