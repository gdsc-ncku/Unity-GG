using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������D�㱼�����W
/// �Ω����D��W�٦۰ʱ��઺DEMO�ĪG
/// </summary>
public class CanvaAutoRotate : MonoBehaviour
{
    public float rotationSpeed = 30f; // �C����઺���t�� (�׼�)

    private void FixedUpdate()
    {
        AutoRotate();
    }

    /// <summary>
    /// �۰ʱ���
    /// </summary>
    private void AutoRotate()
    {
        // �p�����q�A���ɶ��M�t��
        float rotationAngle = rotationSpeed * Time.deltaTime;

        // �֥[�����ثe�����ס]�Ȱw�� Y �b����H�O���b X-Z �����W�^
        transform.eulerAngles += new Vector3(0, rotationAngle, 0);
    }
}
