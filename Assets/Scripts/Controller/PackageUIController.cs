using UniRx;
using UnityEngine;

/// <summary>
/// 負責整個背包的控制
/// </summary>
public class PackageUIController : MonoBehaviour
{
    public GameObject ItemUI;
    public GameObject WeaponUI;
    public GameObject CollectionUI;
    public GameObject BackMask;

    /// <summary>
    /// 打開道具介面
    /// </summary>
    public void Item()
    {
        //啟用指定UI
        if (ItemUI == null)
        {
            Debug.LogError("Backpack UI is not assigned!");
            return;
        }


        ItemUI.SetActive(true); // 切換背包顯示狀態
        
        //更新item資訊
        EventManager.TriggerEvent(NameOfEvent.UpdateItem);

        BackMask.SetActive(true);

        // 當背包開啟時，解除鎖定滑鼠
        Debug.Log(ItemUI.activeSelf);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //PlayerManager.Instance.playerStatus = PlayerStatus.ui; // 更新玩家狀態為 UI 模式，我只是猜你可能會這樣寫，要不要隨便你.jpg

        // 當背包關閉時，重新鎖定滑鼠
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        //PlayerManager.Instance.playerStatus = PlayerStatus.move; // 恢復到移動模式，我只是猜你可能會這樣寫，要不要隨便你.jpg

    }

    /// <summary>
    /// 關上道具介面
    /// </summary>
    public void CloseUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // 切換背包顯示狀態
        BackMask.SetActive(false);
        ItemUI.SetActive(false);
        //WeaponUI.SetActive(false);
        //CollectionUI.SetActive(false);
        //Debug.Log("test2");
    }

    #region event
    private CompositeDisposable disposables = new CompositeDisposable();

    private void OnEnable()
    {
        // 註冊對  事件的訂閱
        disposables.Add(EventManager.StartListening(
            NameOfEvent.OpenItemPage,
            () => Item()
        ));

        disposables.Add(EventManager.StartListening(
            NameOfEvent.CloseUI,
            () => CloseUI()
        ));
    }

    private void OnDisable()
    {
        disposables.Clear(); // 自動取消所有事件訂閱
    }
    #endregion
}
