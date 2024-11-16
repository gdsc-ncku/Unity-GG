using UnityEngine;

/*
 * �w�q��Weapon function
 * ��WeaponManager�I�s�ɥi�H�Τ@function�W�١A�����B�~��if�h�P�_�ӥέ��ӪZ������function
*/
public abstract class Weapon : MonoBehaviour
{
    #region �򥻫���
    public virtual void RightClick()
    {
        Debug.LogError($"{GetType().Name} havn't define right click logic");
    }
    public virtual void LeftClick()
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
