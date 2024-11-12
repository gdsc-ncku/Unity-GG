using UnityEngine;
using UnityEngine.InputSystem;

/*
 * �w�q��Weapon function
 * ��WeaponManager�I�s�ɥi�H�Τ@function�W�١A�����B�~��if�h�P�_�ӥέ��ӪZ������function
*/
public abstract class Weapon : MonoBehaviour
{
    #region �򥻫���
    public virtual void RightClick(InputAction.CallbackContext callback)
    {
        Debug.LogError($"{GetType().Name} havn't define right click logic");
    }
    public virtual void LeftClick(InputAction.CallbackContext callback)
    {
        Debug.LogError($"{GetType().Name} havn't define left click logic");
    }
    public virtual void MiddleClick(InputAction.CallbackContext callback)
    {
        Debug.LogError($"{GetType().Name} havn't define middle click logic");
    }
    public virtual void RClick(InputAction.CallbackContext callback)
    {
        Debug.LogError($"{GetType().Name} havn't define r click logic");
    }
    #endregion
}
