using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 掛載於道具掉落物上
/// 用於讓道具名稱自動旋轉的DEMO效果
/// </summary>
public class CanvaAutoRotate : MonoBehaviour
{
    public float rotationSpeed = 30f; // 每秒旋轉的角速度 (度數)

    private void FixedUpdate()
    {
        AutoRotate();
    }

    /// <summary>
    /// 自動旋轉
    /// </summary>
    private void AutoRotate()
    {
        // 計算旋轉量，基於時間和速度
        float rotationAngle = rotationSpeed * Time.deltaTime;

        // 累加旋轉到目前的角度（僅針對 Y 軸旋轉以保持在 X-Z 平面上）
        transform.eulerAngles += new Vector3(0, rotationAngle, 0);
    }
}
