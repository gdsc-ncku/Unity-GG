using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 定義基本Weapon function
 * 使WeaponManager呼叫時可以統一function名稱，不需額外用if去判斷該用哪個武器內的function
*/
public abstract class Weapon : MonoBehaviour
{
    public abstract void RightClick();
    public abstract void LeftClick();
    public abstract void MiddleClick();
    public abstract void RClick();
}
