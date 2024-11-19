using UnityEngine;

/// <summary>
/// ����Ĳ�o�����A
/// ���U�B����B�P�}
/// </summary>
public enum ClickType
{
    push,
    hold,
    release
}

/*
 * �w�q��Weapon function
 * ��WeaponManager�I�s�ɥi�H�Τ@function�W�١A�����B�~��if�h�P�_�ӥέ��ӪZ������function
*/
public abstract class Weapon : MonoBehaviour
{
    #region �򥻫���
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
