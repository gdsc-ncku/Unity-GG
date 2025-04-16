using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 武器的基底類別，直接掛在武器prefab上
/// 玩家要用的時候
/// </summary>
public abstract class Weapon : MonoBehaviour
{
    PlayerControl inputActions;
    protected Transform keepPosition;
    protected Camera playerCamera;

    void Start() 
    {
        inputActions = PlayerManager.Instance.playerControl;
        keepPosition = transform;
        playerCamera = Camera.main;
        Init();

        inputActions.player.rightclick.started += RightClickStarted;
        inputActions.player.rightclick.performed += RightClickPerformed;
        inputActions.player.rightclick.canceled += RightClickCanceled;
        inputActions.player.leftclick.started += LeftClickStarted;
        inputActions.player.leftclick.performed += LeftClickPerformed;
        inputActions.player.leftclick.canceled += LeftClickCanceled;
        inputActions.player.rclick.started += RClickStarted;
        inputActions.player.rclick.performed += RClickPerformed;
        inputActions.player.rclick.canceled += RClickCanceled;
    }

    /// <summary>
    /// 初始化
    /// 修改為virtual 用於讓繼承的武器可以自訂義額外的初始化 ---zhwa
    /// </summary>
    protected virtual void Init() { }

    void OnDestroy() 
    {
        inputActions.player.rightclick.started -= RightClickStarted;
        inputActions.player.rightclick.performed -= RightClickPerformed;
        inputActions.player.rightclick.canceled -= RightClickCanceled;
        inputActions.player.leftclick.started -= LeftClickStarted;
        inputActions.player.leftclick.performed -= LeftClickPerformed;
        inputActions.player.leftclick.canceled -= LeftClickCanceled;
        inputActions.player.rclick.started -= RClickStarted;
        inputActions.player.rclick.performed -= RClickPerformed;
        inputActions.player.rclick.canceled -= RClickCanceled;
    }

    #region 右鍵設定
    protected virtual void RightClickStarted(InputAction.CallbackContext obj) { }
    protected virtual void RightClickPerformed(InputAction.CallbackContext obj) { }
    protected virtual void RightClickCanceled(InputAction.CallbackContext obj) { }
    #endregion

    #region 左鍵設定
    protected virtual void LeftClickStarted(InputAction.CallbackContext obj) { }
    protected virtual void LeftClickPerformed(InputAction.CallbackContext obj) { }
    protected virtual void LeftClickCanceled(InputAction.CallbackContext obj) { }
    #endregion

    #region 中鍵設定
    protected virtual void MiddleClickStarted(InputAction.CallbackContext obj) { }
    protected virtual void MiddleClickPerformed(InputAction.CallbackContext obj) { }
    protected virtual void MiddleClickCanceled(InputAction.CallbackContext obj) { }
    #endregion

    #region R鍵設定
    protected virtual void RClickStarted(InputAction.CallbackContext obj) { }
    protected virtual void RClickPerformed(InputAction.CallbackContext obj) { }
    protected virtual void RClickCanceled(InputAction.CallbackContext obj) { }
    #endregion
}
