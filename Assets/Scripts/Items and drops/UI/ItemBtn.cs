using UnityEngine;

public class ItemBtn : MonoBehaviour
{
    public bool isSetting = false;
    public ItemName nameIndex;

    /// <summary>
    /// ��ܸӪ��󪺫���
    /// </summary>
    public void ChooseItemButton()
    {
        if (isSetting == true)
        {
            Debug.Log("ItemBtn: trigger");

            //�{������V
            ItemManager.Instance.ItemChoosed((int)nameIndex);

            //UI���
            EventManager.TriggerEvent(NameOfEvent.ItemChoosed, (int)nameIndex);
        }
    }
}
