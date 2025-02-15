using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class Revolver : RangedWeapon
{
    [SerializeField]protected Transform fulcrum;
    [SerializeField]protected Transform cylinder;
    const int CYLINDER_PER_ANGLE = 60;
    float speed = 250;

    protected override void Fire(Vector3 direction)
    {
        base.Fire(direction);
        var ammo = Instantiate(ammoPrefab, firePoint.position, Quaternion.identity);
        ammo.GetComponent<Rigidbody>().linearVelocity = direction * ammoSpeed;
        StartCoroutine(RotateAroundZAnim(cylinder, CYLINDER_PER_ANGLE, speed));
        StartCoroutine(Recoil(modelObject.transform, -15, 400));
        StartCoroutine(ApplyCameraRecoil(2f, 0.1f, 0.02f)); // 後座力強度、回復時間、抖動強度
    }
    protected override void WaitForReload()
    {
        base.WaitForReload();
        StartCoroutine(RotateAroundZAnim(fulcrum, CYLINDER_PER_ANGLE, speed));
    }
    protected override void Reload()
    {
        base.Reload();
        StartCoroutine(RotateAroundZAnim(cylinder, -CYLINDER_PER_ANGLE, speed));
    }
    protected override void EndReload()
    {
        base.EndReload();
        StartCoroutine(RotateAroundZAnim(fulcrum, -CYLINDER_PER_ANGLE, speed));
    }
    IEnumerator Recoil(Transform transform, float targetAngle, float speed)
    {
        yield return StartCoroutine(RotateAroundXAnim(transform, targetAngle, speed));

        yield return StartCoroutine(RotateAroundXAnim(transform, -targetAngle, speed));
    }
    IEnumerator RotateAroundZAnim(Transform transform, float relativeAngle, float speed)
    {
        float startAngle = transform.localEulerAngles.z; // 獲取當前角度
        float elapsedTime = 0f;
        float targetAngle = Mathf.Round(startAngle + relativeAngle); // 計算目標角度
        float rotationDuration = Mathf.Abs(relativeAngle) / speed; // 計算需要的時間

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentAngle = Mathf.LerpAngle(startAngle, targetAngle, elapsedTime / rotationDuration);
            transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
            yield return null;
        }

        // 確保最終角度準確
        transform.localRotation = Quaternion.Euler(0, 0, targetAngle);
    }
    IEnumerator RotateAroundXAnim(Transform transform, float relativeAngle, float speed)
    {
        float startAngle = transform.localEulerAngles.x; // 獲取當前角度
        float elapsedTime = 0f;
        float targetAngle = Mathf.Round(startAngle + relativeAngle); // 計算目標角度
        float rotationDuration = Mathf.Abs(relativeAngle) / speed; // 計算需要的時間

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentAngle = Mathf.LerpAngle(startAngle, targetAngle, elapsedTime / rotationDuration);
            transform.localRotation = Quaternion.Euler(currentAngle, 0, 0);
            yield return null;
        }

        // 確保最終角度準確
        transform.localRotation = Quaternion.Euler(targetAngle, 0, 0);
    }

    IEnumerator ApplyCameraRecoil(float recoilAngle, float returnDuration, float shakeIntensity)
    {
        Transform cameraTransform = Camera.main.transform; // 主攝影機
        Quaternion originalRotation = cameraTransform.localRotation; // 原始旋轉

        // 向上旋轉的後座力
        float elapsedTime = 0f;
        while (elapsedTime < returnDuration)
        {
            elapsedTime += Time.deltaTime;

            // 計算後座旋轉角度
            float recoilStep = Mathf.Lerp(recoilAngle, 0, elapsedTime / returnDuration);
            float shakeOffset = Random.Range(-shakeIntensity, shakeIntensity); // 加入抖動效果

            cameraTransform.localRotation = originalRotation * Quaternion.Euler(-recoilStep + shakeOffset, shakeOffset, 0);

            yield return null;
        }

        // 回復到原始角度
        cameraTransform.localRotation = originalRotation;
    }

}
