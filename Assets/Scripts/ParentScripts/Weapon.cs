using UnityEngine;

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
    #region 基本按鍵
    public virtual void RightClick(ClickType type)
    {
        Debug.LogError($"{GetType().Name} havn't define right click logic");
    }
    public virtual void LeftClick(ClickType type)
    {
        Debug.LogError($"{GetType().Name} havn't define left click logic");
    }
    public virtual void MiddleClick()
    {
        Debug.LogError($"{GetType().Name} havn't define middle click logic");
    }
    public virtual void RClick()
    {
        Debug.LogError($"{GetType().Name} havn't define r click logic");
    }
    #endregion
}
