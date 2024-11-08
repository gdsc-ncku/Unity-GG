using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * �w�q��Weapon function
 * ��WeaponManager�I�s�ɥi�H�Τ@function�W�١A�����B�~��if�h�P�_�ӥέ��ӪZ������function
*/
public abstract class Weapon : MonoBehaviour
{
    public abstract void RightClick();
    public abstract void LeftClick();
    public abstract void MiddleClick();
    public abstract void RClick();
}
