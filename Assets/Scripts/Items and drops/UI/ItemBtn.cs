using UnityEngine;

public class ItemBtn : MonoBehaviour
{
    public bool isSetting = false;
    public ItemName nameIndex;

    /// <summary>
    /// 選擇該物件的按鍵
    /// </summary>
    public void ChooseItemButton()
    {
        if (isSetting == true)
        {
            Debug.Log("ItemBtn: trigger");

            //程式控制面向
            ItemManager.Instance.ItemChoosed(nameIndex);

            //UI控制面
            EventManager.TriggerEvent(NameOfEvent.ItemChoosed, nameIndex);
        }
    }
}
