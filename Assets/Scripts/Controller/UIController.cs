using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject ItemUI;
    public GameObject WeaponUI;
    public GameObject CollectionUI;
    public GameObject BackMask;
    public void Item()
    {
        //啟用指定UI
        if (ItemUI == null)
        {
            Debug.LogError("Backpack UI is not assigned!");
            return;
        }

        
        ItemUI.SetActive(true); // 切換背包顯示狀態
        BackMask.SetActive(true);
        
        // 當背包開啟時，解除鎖定滑鼠
        Debug.Log(ItemUI.activeSelf);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
    }
    public void CloseUI()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // 切換背包顯示狀態
        BackMask.SetActive(false);
        ItemUI.SetActive(false); 
        //WeaponUI.SetActive(false);
        //CollectionUI.SetActive(false);
        //Debug.Log("test2");
    }

    public void Setting()
    {
        
    }
}
