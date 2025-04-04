using System;
using UnityEngine;

/// <summary>
/// 遠程武器父類別
/// </summary>
public abstract class RangedWeapon : Weapon
{
    /// <summary>
    /// 基本設定
    /// </summary>
    [SerializeField] protected float fireRate;
    [SerializeField] protected int ammoCapacity;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected GameObject ammoPrefab;
    float ammoSpeed = 50f;
    protected int currentAmmo;
    float cooldownTimestamp;
    protected bool isAming = false;
    protected bool isReloading = false;

    protected override void Init()
    {
        base.Init();
        currentAmmo = ammoCapacity;
    }
    /// <summary>
    /// 後座力
    /// </summary>
    protected virtual void Aim()
    {
        isAming = true;
    }

    protected virtual void AimCancel()
    {
        isAming = false;
    }

    protected virtual bool TryShoot(Vector3 direction)
    {
        if (CanShoot())
        {
            Fire(direction);
            if (!HasAmmo())
            {
                WaitForReload();
            }
            return true;
        }
        return false;
    }

    protected virtual bool CanShoot()
    {
        return currentAmmo != 0 && Time.time >= cooldownTimestamp;
    }

    protected virtual void Fire(Vector3 direction)
    {
        currentAmmo--;
        cooldownTimestamp = Time.time + 1f / fireRate;
        var ammo = Instantiate(ammoPrefab, firePoint.position, Quaternion.identity);
        ammo.GetComponent<Rigidbody>().linearVelocity = direction * ammoSpeed;
        Debug.Log("Fire");
    }

    protected virtual void WaitForReload()
    {
        isReloading = true;
    }

    protected virtual void TryReload(int ammoNum)
    {
        if (currentAmmo < ammoCapacity)
        {
            Reload(ammoNum);
        }
        if (currentAmmo >= ammoCapacity && isReloading)
        {
            EndReload();
        }
    }
    protected virtual void Reload(int ammoNum)
    {
        currentAmmo += ammoNum;
    }
    protected virtual void EndReload()
    {
        currentAmmo = ammoCapacity;
        isReloading = false;
    }
    bool HasAmmo()
    {
        return currentAmmo > 0;
    }
}
