using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class Revolver : RangedWeapon
{
    [SerializeField]protected Transform aimTransform;
    [SerializeField]protected Transform holdTransform;
    [SerializeField]protected Transform revolver;
    [SerializeField]protected Transform fulcrum;
    [SerializeField]protected Transform cylinder;
    const int CYLINDER_PER_ANGLE = 60;
    Vector3 currentVelocity;
    float normalFOV = 60f;
    float aimFOV = 25f;
    float speed = 240;
    float cameraZoomVelocity;
    float smoothTime = 0.1f;
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
        TryShoot(playerCamera.transform.forward);
    }
    protected override void RClickStarted(InputAction.CallbackContext obj)
    {
        TryReload(1);
    }
    protected override void Aim()
    {
        base.Aim();
        Observable.EveryUpdate()
        .TakeWhile(_ => isAming)
        .Subscribe(_ =>
        {
            revolver.position = Vector3.SmoothDamp(revolver.position, aimTransform.position, ref currentVelocity, smoothTime);
            playerCamera.fieldOfView = Mathf.SmoothDamp(playerCamera.fieldOfView, aimFOV, ref cameraZoomVelocity, smoothTime);
        })
        .AddTo(gameObject);
    }
    protected override void AimCancel()
    {
        base.AimCancel();
        Observable.EveryUpdate()
        .TakeWhile(_ => !isAming)
        .Subscribe(_ =>
        {
            revolver.position = Vector3.SmoothDamp(revolver.position, holdTransform.position, ref currentVelocity, smoothTime);
            playerCamera.fieldOfView = Mathf.SmoothDamp(playerCamera.fieldOfView, normalFOV, ref cameraZoomVelocity, smoothTime);
        })
        .AddTo(gameObject);
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
    protected override void Reload(int ammoNum)
    {
        base.Reload(ammoNum);
        StartCoroutine(RotateAroundZAnim(cylinder, CYLINDER_PER_ANGLE, speed));
    }
    protected override void EndReload()
    {
        base.EndReload();
        StartCoroutine(RotateAroundZAnim(fulcrum, 0, speed));
    }
    IEnumerator RotateAroundZAnim(Transform transform, float targetAngle, float speed)
    {
        var currentAngle = 0f;
        while (currentAngle < targetAngle)
        {
            float step = speed * Time.deltaTime;

            if (currentAngle + step > targetAngle)
            {
                step = targetAngle - currentAngle;
            }

            transform.Rotate(0, 0, step, Space.Self);

            currentAngle += step;

            yield return null;
        }
        transform.localRotation = Quaternion.Euler(0, 0, targetAngle);
    }
}
