using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Revolver : RangedWeapon
{
    [SerializeField]protected Transform aimTransform;
    [SerializeField]protected Transform holdTransform;
    [SerializeField]protected Transform fulcrum;
    [SerializeField]protected Transform cylinder;
    const int CYLINDER_PER_ANGLE = 60;
    Vector3 currentVelocity;
    float normalFOV = 60f;
    float aimFOV = 25f;
    float speed = 240;
    float cameraZoomVelocity;

    void Update()
    {
        if (Input.GetMouseButton(1) && !isReloading) // 右鍵按下
        {
            Aim();
        }
        else
        {
            AimCancel();
        }
        if (Input.GetMouseButtonDown(0) && !isReloading)
        {
            TryShoot(playerCamera.transform.forward);
        }
        if (Input.GetKeyDown(KeyCode.R) && isReloading)
        {
            TryReload(1);
        }
    }
    protected override void Aim()
    {
        base.Aim();
		transform.position = Vector3.SmoothDamp(transform.position, aimTransform.position, ref currentVelocity, 0.1f);
		playerCamera.fieldOfView = Mathf.SmoothDamp(playerCamera.fieldOfView, aimFOV, ref cameraZoomVelocity, 0.1f);
    }
    protected override void AimCancel()
    {
        base.AimCancel();
		transform.position = Vector3.SmoothDamp(transform.position, holdTransform.position, ref currentVelocity, 0.1f);
		playerCamera.fieldOfView = Mathf.SmoothDamp(playerCamera.fieldOfView, normalFOV, ref cameraZoomVelocity, 0.1f);
    }
    protected override void Fire(Vector3 direction)
    {
        base.Fire(direction);
        StartCoroutine(RotateAroundZAnim(cylinder, CYLINDER_PER_ANGLE, speed));
    }
    protected override void WaitForReload()
    {
        base.WaitForReload();
        StartCoroutine(RotateAroundZAnim(fulcrum, CYLINDER_PER_ANGLE, speed));
    }
    protected override void TryReload(int ammoNum)
    {
        base.TryReload(ammoNum);
        if (currentAmmo < ammoCapacity)
        {
            StartCoroutine(RotateAroundZAnim(cylinder, CYLINDER_PER_ANGLE, speed));
        }
    }
    protected override void EndReload()
    {
        base.EndReload();
        StartCoroutine(RotateAroundZAnim(fulcrum, 0, speed));
    }
    IEnumerator RotateAroundZAnim(Transform transform, float finalAngle, float speed)
    {
        var currentAngle = 0f;
        while (currentAngle < finalAngle)
        {
            float step = speed * Time.deltaTime;

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
}
