using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject ItemUI;
    public GameObject WeaponUI;
    public GameObject CollectionUI;
    public GameObject BackMask;
    public GameObject GameplayUI;
    public GameObject ConfigurationUI;
    public GameObject GraphicUI;
    public GameObject AudioUI;

    public void Reset()
    {
        ItemUI.SetActive(false);
        WeaponUI.SetActive(false);
        CollectionUI.SetActive(false);
        BackMask.SetActive(false);
        GameplayUI.SetActive(false);
        ConfigurationUI.SetActive(false);
        GraphicUI.SetActive(false);
        AudioUI.SetActive(false);
    }
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

    public void ToItem()
    {
        //啟用指定UI
        if (ItemUI == null)
        {
            Debug.LogError("Item UI is not assigned!");
            return;
        }

        ItemUI.SetActive(true);
        WeaponUI.SetActive(false);
        CollectionUI.SetActive(false);
        BackMask.SetActive(true);
        
        
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ToWeapon()
    {
        //啟用指定UI
        if (WeaponUI == null)
        {
            Debug.LogError("Weapon UI is not assigned!");
            return;
        }

        ItemUI.SetActive(false);
        WeaponUI.SetActive(true);
        CollectionUI.SetActive(false);
        BackMask.SetActive(true);
        
        
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ToCollection()
    {
        //啟用指定UI
        if (CollectionUI == null)
        {
            Debug.LogError("Collection UI is not assigned!");
            return;
        }

        ItemUI.SetActive(false);
        WeaponUI.SetActive(false);
        CollectionUI.SetActive(true);
        BackMask.SetActive(true);
        
        // 當背包開啟時，解除鎖定滑鼠
        Debug.Log(ItemUI.activeSelf);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void CloseUI()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        // 切換背包顯示狀態
        BackMask.SetActive(false);
        ItemUI.SetActive(false); 
        WeaponUI.SetActive(false);
        CollectionUI.SetActive(false);
        //Debug.Log("test2");
    }

    public void Setting()
    {
        if (GameplayUI == null)
        {
            Debug.LogError("Gameplay UI is not assigned!");
            return;
        }
        GameplayUI.SetActive(true); // 切換背包顯示狀態
        BackMask.SetActive(true);
        
        // 當背包開啟時，解除鎖定滑鼠
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ToGameplay()
    {
        if (GameplayUI == null)
        {
            Debug.LogError("Gameplay UI is not assigned!");
            return;
        }
        //BackMask.SetActive(true);
        GameplayUI.SetActive(true);
        ConfigurationUI.SetActive(false);
        GraphicUI.SetActive(false);
        AudioUI.SetActive(false);
        
        // 當背包開啟時，解除鎖定滑鼠
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ToConfiguration()
    {
        if (ConfigurationUI == null)
        {
            Debug.LogError("Configuration UI is not assigned!");
            return;
        }
        //BackMask.SetActive(true);
        GameplayUI.SetActive(false);
        ConfigurationUI.SetActive(true);
        GraphicUI.SetActive(false);
        AudioUI.SetActive(false);
        
        // 當背包開啟時，解除鎖定滑鼠
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ToGraphic()
    {
        if (GraphicUI == null)
        {
            Debug.LogError("Graphic UI is not assigned!");
            return;
        }
        //BackMask.SetActive(true);
        GameplayUI.SetActive(false);
        ConfigurationUI.SetActive(false);
        GraphicUI.SetActive(true);
        AudioUI.SetActive(false);
        
        // 當背包開啟時，解除鎖定滑鼠
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ToAudio()
    {
        if (AudioUI == null)
        {
            Debug.LogError("Audio UI is not assigned!");
            return;
        }
        //BackMask.SetActive(true);
        GameplayUI.SetActive(false);
        ConfigurationUI.SetActive(false);
        GraphicUI.SetActive(false);
        AudioUI.SetActive(true);
        
        // 當背包開啟時，解除鎖定滑鼠
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Setting_Back()
    {
        GameplayUI.SetActive(false);
        ConfigurationUI.SetActive(false);
        GraphicUI.SetActive(false);
        AudioUI.SetActive(false);
        BackMask.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
