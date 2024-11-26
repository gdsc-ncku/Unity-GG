using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 按鍵觸發的狀態
/// 按下、按住、鬆開
/// </summary>
public enum ClickType
{
    push,
    hold,
    release
}

/*
 * 定義基本Weapon function
 * 使WeaponManager呼叫時可以統一function名稱，不需額外用if去判斷該用哪個武器內的function
*/
public abstract class Weapon : MonoBehaviour
{
//     //instance mode
//     private static Weapon _weapon;
//     public static Weapon Instance
//     {
//         get
//         {
//             if (!_weapon)
//             {
//                 _weapon = FindObjectOfType(typeof(Weapon)) as Weapon;
//             }
//             return _weapon;
//         }
//     }
    
    protected Transform keepPosition;
    protected Camera playerCamera;

    public void Init(Transform transform, Camera camera, PlayerControl playerControl)
    {
        keepPosition = transform;
        playerCamera = camera;
    }

    #region 右鍵設定
    public virtual void RightClickStarted(InputAction.CallbackContext obj)
    {
        Debug.LogError($"{GetType().Name} havn't define right click logic");
    }
    public virtual void RightClickPerformed(InputAction.CallbackContext obj)
    {
        Debug.LogError($"{GetType().Name} havn't define right click logic");
    }

    public virtual void RightClickCanceled(InputAction.CallbackContext obj)
    {
        Debug.LogError($"{GetType().Name} havn't define right click logic");
    }
    #endregion

    #region 左鍵設定
    public virtual void LeftClickStarted(InputAction.CallbackContext obj)
    {
        Debug.LogError($"{GetType().Name} havn't define left click logic");
    }

    public virtual void LeftClickPerformed(InputAction.CallbackContext obj)
    {
        Debug.LogError($"{GetType().Name} havn't define left click logic");
    }

    public virtual void LeftClickCanceled(InputAction.CallbackContext obj)
    {
        Debug.LogError($"{GetType().Name} havn't define left click logic");
    }
    #endregion

    #region 中鍵設定
    public virtual void MiddleClickStarted(InputAction.CallbackContext obj)
    {
        Debug.LogError($"{GetType().Name} havn't define middle click logic");
    }

    public virtual void MiddleClickPerformed(InputAction.CallbackContext obj)
    {
        Debug.LogError($"{GetType().Name} havn't define middle click logic");
    }

    public virtual void MiddleClickCanceled(InputAction.CallbackContext obj)
    {
        Debug.LogError($"{GetType().Name} havn't define middle click logic");
    }
    #endregion

    #region R鍵設定
    public virtual void RClickStarted(InputAction.CallbackContext obj)
    {
        Debug.LogError($"{GetType().Name} havn't define r click logic");
    }

    public virtual void RClickPerformed(InputAction.CallbackContext obj)
    {
        Debug.LogError($"{GetType().Name} havn't define r click logic");
    }

    public virtual void RClickCanceled(InputAction.CallbackContext obj)
    {
        Debug.LogError($"{GetType().Name} havn't define r click logic");
    }
    #endregion
}
