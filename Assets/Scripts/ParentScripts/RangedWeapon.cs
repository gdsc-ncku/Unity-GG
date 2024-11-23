using System.Collections;
using UnityEngine;

/// <summary>
/// 遠程武器父類別
/// </summary>
public abstract class RangedWeapon : Weapon
{
    /// <summary>
    /// 基本設定
    /// </summary>
    [SerializeField]protected float fireRate;
    [SerializeField]protected int ammoCapacity;
    [SerializeField]protected Transform FirePoint;
    int currentAmmo = 6;
    /// <summary>
    /// 瞄準
    /// </summary>
    protected bool IsAiming = false;
    /// <summary>
    /// 後座力
    /// </summary>
    protected virtual void Aim()
    {
        IsAiming = true;
    }
    protected virtual void AimCancel()
    {
        IsAiming = false;
    }
    protected virtual void TryFire()
    {
        Fire();
        if (!HasAmmo())
        {
            WaitForReload();
        }
    }
    protected virtual void Fire()
    {
        currentAmmo--;
        // Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    protected virtual void WaitForReload()
    {
        AimCancel();
    }

    protected virtual void Reload(int ammoNum)
    {
        currentAmmo += ammoNum;
        if (currentAmmo == ammoCapacity)
        {
            EndReload();
        }
    }
    protected virtual void EndReload()
    {
        
    }
    bool HasAmmo()
    {
        return currentAmmo > 0;
    }
}
