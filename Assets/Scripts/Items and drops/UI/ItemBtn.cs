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
            ItemManager.Instance.ItemChoosed(nameIndex);

            //UI���
            EventManager.TriggerEvent(NameOfEvent.ItemChoosed, nameIndex);
        }
    }
}
