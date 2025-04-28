using UnityEngine;
using UniRx;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    public GameObject BulletUIPrefab;

    public GameObject parentCanvas;

    // 生成後的實例
    private GameObject ItemUI;
    private GameObject WeaponUI;
    private GameObject CollectionUI;
    private GameObject BackMask;
    private GameObject GameplayUI;
    private GameObject ConfigurationUI;
    private GameObject GraphicUI;
    private GameObject AudioUI;
    private GameObject BulletUI;


    //用於動畫
    //private GraphicRaycaster raycaster;
    //private GameObject canvas;

    public void Start()
    {
        // 產生 prefab 的實例並保存在變數中
        BackMask = Instantiate(BackMaskPrefab,parentCanvas.transform);

        ItemUI = Instantiate(ItemUIPrefab,parentCanvas.transform);
        WeaponUI = Instantiate(WeaponUIPrefab,parentCanvas.transform);
        CollectionUI = Instantiate(CollectionUIPrefab,parentCanvas.transform);
        
        GameplayUI = Instantiate(GameplayUIPrefab,parentCanvas.transform);
        ConfigurationUI = Instantiate(ConfigurationUIPrefab,parentCanvas.transform);
        GraphicUI = Instantiate(GraphicUIPrefab,parentCanvas.transform);
        AudioUI = Instantiate(AudioUIPrefab,parentCanvas.transform);
        BulletUI = Instantiate(BulletUIPrefab,parentCanvas.transform);

        BulletUI_Selecting script = BulletUI.GetComponent<BulletUI_Selecting>();
        script.BulletUI = BulletUI;
        
        /*
        ItemUIPrefab.SetActive(false);
        WeaponUIPrefab.SetActive(false);
        CollectionUIPrefab.SetActive(false);
        BackMaskPrefab.SetActive(false);
        GameplayUIPrefab.SetActive(false);
        ConfigurationUIPrefab.SetActive(false);
        GraphicUIPrefab.SetActive(false);
        AudioUIPrefab.SetActive(false);
        */
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
        BulletUI.SetActive(true);
        Debug.Log("test1");
    }

    public void Pack_spin_animate()
    {

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

        //更新item資訊
        EventManager.TriggerEvent(NameOfEvent.UpdateItem);
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

        //更新item資訊
        EventManager.TriggerEvent(NameOfEvent.UpdateItem);
    }
    public void Selecting()
    {
        Debug.Log("Selecting");
        Cursor.lockState = CursorLockMode.None;
        
        BulletUI_Selecting script = BulletUI.GetComponent<BulletUI_Selecting>();

        script.startselect = true;
          
    }

    public void SelectItem()
    {
        Debug.Log("Select fin");
        Cursor.lockState = CursorLockMode.Locked;
        
        BulletUI_Selecting script = BulletUI.GetComponent<BulletUI_Selecting>();
        //script.isselecting = false;
        script.startselect = false;


    }
    public void Selecting()
    {
        Debug.Log("Selecting");
        Cursor.lockState = CursorLockMode.None;
        
        BulletUI_Selecting script = BulletUI.GetComponent<BulletUI_Selecting>();

        script.startselect = true;
          
    }

    public void SelectItem()
    {
        Debug.Log("Select fin");
        Cursor.lockState = CursorLockMode.Locked;
        
        BulletUI_Selecting script = BulletUI.GetComponent<BulletUI_Selecting>();
        //script.isselecting = false;
        script.startselect = false;


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
        Cursor.lockState = CursorLockMode.Locked;
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

    

    
    #region event
    private CompositeDisposable disposables = new CompositeDisposable();

    private void OnEnable()
    {
        /*
        disposables.Add(EventManager.StartListening<int>(
            NameOfEvent.ItemChoosed,
            (index) => ItemChoosed(index)
        ));
        */

        disposables.Add(EventManager.StartListening(
            NameOfEvent.ToGameplay,
            () => ToGameplay()
        ));
        disposables.Add(EventManager.StartListening(
            NameOfEvent.ToConfiguration,
            () => ToConfiguration()
        ));
        disposables.Add(EventManager.StartListening(
            NameOfEvent.ToGraphic,
            () => ToGraphic()
        ));
        disposables.Add(EventManager.StartListening(
            NameOfEvent.ToAudio,
            () => ToAudio()
        ));
        disposables.Add(EventManager.StartListening(
            NameOfEvent.Setting_Back,
            () => Setting_Back()
        ));
        disposables.Add(EventManager.StartListening(
            NameOfEvent.ToItem,
            () => ToItem()
        ));
        disposables.Add(EventManager.StartListening(
            NameOfEvent.ToWeapon,
            () => ToWeapon()
        ));
        disposables.Add(EventManager.StartListening(
            NameOfEvent.ToCollection,
            () => ToCollection()
        ));
    }

    private void OnDisable()
    {
        disposables.Clear(); 
    }
    #endregion
}
