using System.Collections;
using UnityEngine;

/// <summary>
/// 遠程武器父類別
/// </summary>
public abstract class RangedWeapon : Weapon
{
    [SerializeField]protected float fireRate;
    [SerializeField]protected int ammoCapacity;
    [SerializeField]protected int currentAmmo;
    [SerializeField]protected Transform FirePoint;
    private float normalFOV = 60f;
    private float aimFOV = 20f;
    private float zoomSpeed = 5f;
    protected bool IsAiming;
    protected virtual void Aim()
    {
        IsAiming = true;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, aimFOV, Time.deltaTime * zoomSpeed);
    }
    protected virtual IEnumerator CancelAim()
    {
        IsAiming = false;
        while (Mathf.Abs(playerCamera.fieldOfView - normalFOV) > 0.1f)
        {
            // Debug.Log(playerCamera.fieldOfView);
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, Time.deltaTime * zoomSpeed);
            yield return null;
        }
    }
    protected virtual void Fire()
    {
        if (HasAmmo())
        {
            currentAmmo--;
            // Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
        else
        {
            WaitForReload();
        }
    }

    protected virtual void WaitForReload()
    {
        // Animation?
    }

    protected virtual void Reload(int ammoNum)
    {
        currentAmmo++;
        int ammoNeeded = ammoCapacity - currentAmmo;
        if (ammoNeeded > 0)
        {
            currentAmmo = ammoCapacity;
        }
        else
        {
        }
    }
    protected bool HasAmmo()
    {
        return currentAmmo > 0;
    }
}
