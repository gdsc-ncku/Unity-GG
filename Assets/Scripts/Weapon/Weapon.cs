using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 武器的基底類別，直接掛在武器prefab上
/// 玩家要用的時候
/// Model object會在hold point跟aim point上來回移動
/// </summary>
public abstract class Weapon : MonoBehaviour
{
    protected PlayerControl inputActions;
    //根據玩家position給出的offset使裝備時可以簡單定位武器位置
    [SerializeField] protected GameObject modelObject;
    [SerializeField] protected Transform holdPoint;
    [SerializeField] protected Transform aimPoint;
    //把camera的position當原點給予offset讓他定位在視角某處
    [SerializeField] protected Vector3 holdPointOffset;
    [SerializeField] protected Vector3 aimPointOffset;
    protected Camera playerCamera;

    void Start() 
    {
        VariableInit();
        KeyboardBindingInit();
    }

    private void Update()
    {
        transform.forward = playerCamera.transform.forward;
    }

    /// <summary>
    /// 初始化
    /// 修改為virtual 用於讓繼承的武器可以自訂義額外的初始化 ---zhwa
    /// </summary>
    protected virtual void KeyboardBindingInit() 
    {
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

    protected virtual void VariableInit()
    {
        inputActions = PlayerManager.Instance.playerControl;
        playerCamera = Camera.main;
        transform.position = playerCamera.transform.position;
        transform.eulerAngles = playerCamera.transform.eulerAngles;

        holdPoint.position = playerCamera.transform.position + holdPointOffset;
        holdPoint.eulerAngles = playerCamera.transform.eulerAngles;
        modelObject.transform.position = holdPoint.position;
        modelObject.transform.eulerAngles = holdPoint.eulerAngles;

        aimPoint.position = playerCamera.transform.position + aimPointOffset;
    }

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
