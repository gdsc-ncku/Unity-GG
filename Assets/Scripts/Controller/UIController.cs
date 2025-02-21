using UnityEngine;

public class UIController : MonoBehaviour
{
    // 以下這些變數在 Inspector 中被設定為 prefab
    public GameObject ItemUIPrefab;
    public GameObject WeaponUIPrefab;
    public GameObject CollectionUIPrefab;
    public GameObject BackMaskPrefab;
    public GameObject GameplayUIPrefab;
    public GameObject ConfigurationUIPrefab;
    public GameObject GraphicUIPrefab;
    public GameObject AudioUIPrefab;

    // 生成後的實例
    private GameObject ItemUI;
    private GameObject WeaponUI;
    private GameObject CollectionUI;
    private GameObject BackMask;
    private GameObject GameplayUI;
    private GameObject ConfigurationUI;
    private GameObject GraphicUI;
    private GameObject AudioUI;

    public void Start()
    {
        // 產生 prefab 的實例並保存在變數中
        ItemUI = Instantiate(ItemUIPrefab);
        WeaponUI = Instantiate(WeaponUIPrefab);
        CollectionUI = Instantiate(CollectionUIPrefab);
        BackMask = Instantiate(BackMaskPrefab);
        GameplayUI = Instantiate(GameplayUIPrefab);
        ConfigurationUI = Instantiate(ConfigurationUIPrefab);
        GraphicUI = Instantiate(GraphicUIPrefab);
        AudioUI = Instantiate(AudioUIPrefab);

        ItemUIPrefab.SetActive(false);
        WeaponUIPrefab.SetActive(false);
        CollectionUIPrefab.SetActive(false);
        BackMaskPrefab.SetActive(false);
        GameplayUIPrefab.SetActive(false);
        ConfigurationUIPrefab.SetActive(false);
        GraphicUIPrefab.SetActive(false);
        AudioUIPrefab.SetActive(false);
        // 確保所有 UI 預設為隱藏
        Reset();
    }

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
        Debug.Log("test1");
    }

    public void Item()
    {
        if (ItemUI == null)
        {
            Debug.LogError("Item UI instance is not created!");
            return;
        }

        ItemUI.SetActive(true);
        BackMask.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ToItem()
    {
        if (ItemUI == null)
        {
            Debug.LogError("Item UI instance is not created!");
            return;
        }

        ItemUI.SetActive(true);
        WeaponUI.SetActive(false);
        CollectionUI.SetActive(false);
        BackMask.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // 其他函數（ToWeapon、ToCollection 等）同樣使用實例變數
    public void ToWeapon()
    {
        if (WeaponUI == null)
        {
            Debug.LogError("Weapon UI instance is not created!");
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
        if (CollectionUI == null)
        {
            Debug.LogError("Collection UI instance is not created!");
            return;
        }

        ItemUI.SetActive(false);
        WeaponUI.SetActive(false);
        CollectionUI.SetActive(true);
        BackMask.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseUI()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        BackMask.SetActive(false);
        ItemUI.SetActive(false);
        WeaponUI.SetActive(false);
        CollectionUI.SetActive(false);
    }

    public void Setting()
    {
        if (GameplayUI == null)
        {
            Debug.LogError("Gameplay UI instance is not created!");
            return;
        }

        GameplayUI.SetActive(true);
        ConfigurationUI.SetActive(false);
        GraphicUI.SetActive(false);
        AudioUI.SetActive(false);
        BackMask.SetActive(true);

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
        Debug.Log(GameplayUI.activeSelf);
        Debug.Log(ConfigurationUI.activeSelf);
        Debug.Log(GraphicUI.activeSelf);
        Debug.Log(AudioUI.activeSelf);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
}
