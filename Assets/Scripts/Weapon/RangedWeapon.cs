using System;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] protected float ammoSpeed = 50f;
    protected int currentAmmo;
    protected float cooldownTimestamp;
    [SerializeField] protected bool isAming = false;
    protected bool isReloading = false;

    [SerializeField] float normalFOV = 45f;
    [SerializeField] float aimFOV = 25f;
    [SerializeField] float smoothTime = 0.1f;

    [SerializeField] float reloadTime;
    [SerializeField] int OneTimeLoadingAmount;

    Vector3 currentVelocity;
    float cameraZoomVelocity;

    protected override void VariableInit()
    {
        base.VariableInit();
        currentAmmo = ammoCapacity;
    }

    protected override void RightClickStarted(InputAction.CallbackContext obj)
    {
        Aim();
    }
    protected override void RightClickCanceled(InputAction.CallbackContext obj)
    {
        AimCancel();
    }
    protected override void LeftClickStarted(InputAction.CallbackContext obj)
    {
        TryShoot();
    }
    protected override void RClickStarted(InputAction.CallbackContext obj)
    {
        TryReload();
    }

    protected virtual void Aim()
    {
        isAming = true;
        Observable.EveryUpdate()
            .TakeWhile(_ => isAming)
            .Subscribe(_ =>
            {
                modelObject.transform.position = Vector3.SmoothDamp(modelObject.transform.position, aimPoint.position, ref currentVelocity, smoothTime);
                playerCamera.fieldOfView = Mathf.SmoothDamp(playerCamera.fieldOfView, aimFOV, ref cameraZoomVelocity, smoothTime);
            })
            .AddTo(gameObject);
    }

    protected virtual void AimCancel()
    {
        isAming = false;
        Observable.EveryUpdate()
            .TakeWhile(_ => !isAming)
            .Subscribe(_ =>
            {
                modelObject.transform.position = Vector3.SmoothDamp(modelObject.transform.position, holdPoint.position, ref currentVelocity, smoothTime);
                playerCamera.fieldOfView = Mathf.SmoothDamp(playerCamera.fieldOfView, normalFOV, ref cameraZoomVelocity, smoothTime);
            })
            .AddTo(gameObject);
    }

    protected virtual bool TryShoot()
    {
        if (CanShoot())
        {
            Fire(playerCamera.transform.forward);
            if (!HasAmmo())
            {
                TryReload();
            }
            return true;
        }
        return false;
    }

    protected virtual bool CanShoot()
    {
        return !isReloading && currentAmmo > 0 && Time.time >= cooldownTimestamp;
    }

    protected virtual void Fire(Vector3 direction)
    {
        currentAmmo--;
        cooldownTimestamp = Time.time + 1f / fireRate;
        Debug.Log(currentAmmo);
    }

    protected virtual void WaitForReload()
    {
        isReloading = true;
    }

    protected virtual void TryReload()
    {
        if(currentAmmo != ammoCapacity)
        {
            WaitForReload();
            Observable.Interval(TimeSpan.FromSeconds(reloadTime / ammoCapacity * OneTimeLoadingAmount))
                .TakeWhile(_ => isReloading)
                .Subscribe(_ =>
                {
                    Reload();
                });
        }
    }
    protected virtual void Reload()
    {
        currentAmmo += OneTimeLoadingAmount;
        Debug.Log(currentAmmo);
        if(currentAmmo >= ammoCapacity)
        {
            currentAmmo = ammoCapacity;
            EndReload();
        }
    }
    protected virtual void EndReload()
    {
        isReloading = false;
    }
    bool HasAmmo()
    {
        return currentAmmo > 0;
    }
}
