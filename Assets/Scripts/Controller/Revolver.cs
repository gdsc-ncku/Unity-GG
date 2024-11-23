using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Revolver : RangedWeapon
{
    [SerializeField]Transform fulcrum;
    [SerializeField]GameObject cylinder;
    const float CYLINDER_PER_ANGLE = 60f;
    const float CYLINDER_OUT_ANGLE = 60f;
    const float OUT_SPEED = 180f;

    float normalFOV = 60f;
    float aimFOV = 20f;
    float zoomSpeed = 5f;

    void Update()
    {
        if (Input.GetMouseButton(1)) // 右鍵按下
        {
            Aim();
            Debug.Log("aim");
        }
        if (Input.GetMouseButtonUp(1))
        {
            AimCancel();
        }
        if (Input.GetMouseButtonDown(0))
        {
            TryFire();
            Debug.Log("Fire");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload(1);
        }
    }
    protected override void Aim()
    {
        base.Aim();
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, aimFOV, Time.deltaTime * zoomSpeed);
    }
    protected override void AimCancel()
    {
        base.AimCancel();
        StartCoroutine(AimCancleAnim());
    }
    IEnumerator AimCancleAnim()
    {
        while (Mathf.Abs(playerCamera.fieldOfView - normalFOV) > 0.1f)
        {
            // Debug.Log(playerCamera.fieldOfView);
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, Time.deltaTime * zoomSpeed);
            yield return null;
        }
    }
    protected override void Fire()
    {
        base.Fire();
        StartCoroutine(RotateAroundZAnim(cylinder.transform, CYLINDER_PER_ANGLE));
    }
    protected override void WaitForReload()
    {
        base.WaitForReload();
        StartCoroutine(RotateAroundZAnim(fulcrum, CYLINDER_OUT_ANGLE));
    }
    protected override void EndReload()
    {
        base.EndReload();
        StartCoroutine(RotateAroundZAnim(fulcrum, 0));
    }
    IEnumerator RotateAroundZAnim(Transform transform, float finalAngle)
    {
        var currentAngle = 0f;
        while (currentAngle < finalAngle)
        {
            float step = OUT_SPEED * Time.deltaTime;

            if (currentAngle + step > finalAngle)
            {
                step = finalAngle - currentAngle;
            }

            transform.Rotate(0, 0, step, Space.Self);

            currentAngle += step;

            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, 0, finalAngle);
    }

    public override void LeftClickStarted(InputAction.CallbackContext obj)
    {
        TryFire();
    }
    public override void RightClickPerformed(InputAction.CallbackContext obj)
    {
        Aim();
    }
    public override void RightClickCanceled(InputAction.CallbackContext obj)
    {
        AimCancel();
    }
}
