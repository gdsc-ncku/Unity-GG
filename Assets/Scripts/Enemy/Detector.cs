using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// 偵測
/// </summary>
[ExecuteInEditMode]
public class Detector : MonoBehaviour
{
    [SerializeField] float distance = 10;    //偵測距離
    [SerializeField] float angle = 30;       //偵測角度
    [SerializeField] bool isDebug = true;    //偵測角度
    LayerMask layer => LayerMask.GetMask(LayerTagPack.Enemy, LayerTagPack.Player);
    LayerMask occlusionLayers => LayerMask.GetMask(LayerTagPack.Environment);
    Collider[] colliders = new Collider[50];
    List<GameObject> detectedObjects = new List<GameObject>();
    GameObject taget;

    
    void Update()
    {
        Dectect();
    }

    /// <summary>
    /// 偵測
    /// </summary>
    void Dectect()
    {
        detectedObjects.Clear();
        var count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layer, QueryTriggerInteraction.Collide);
        for(int i = 0; i < count; i++)
        {
            GameObject obj = colliders[i].gameObject;
            if(obj == transform.root.gameObject) continue;
            if(IsVisible(obj))
            {
                detectedObjects.Add(obj);
            }
        }
        if(taget != null && !detectedObjects.Contains(taget))
        {
            taget = null;
        }
    }

    /// <summary>
    /// 檢查是否看的到物件
    /// 假設以後有甚麼透明怪或是透明技能之類的
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    bool IsVisible(GameObject target)
    {
        return IsInAngle(target) && !IsBlocked(target);
    }

    /// <summary>
    /// 檢查物件是否在視野角度內(只判斷左右)
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    bool IsInAngle(GameObject target)
    {
        var direction = target.transform.position - transform.position;
        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if (deltaAngle > angle)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 檢查物件是否被遮擋
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    bool IsBlocked(GameObject target)
    {
        // 待修，occlusionLayers偵測不到
        // Debug.DrawLine(transform.position, target.transform.position, Color.red);
        if (Physics.Linecast(transform.position, target.transform.position, occlusionLayers))
        {
            return true;
        }
        return false;
    }

    #region Debug
    Mesh mesh;
    Color meshColor = Color.blue;
    void OnValidate()
    {
        mesh = CreateWedgeMesh();
    }

    void OnDrawGizmos()
    {
        if (mesh && isDebug)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
            Gizmos.DrawWireSphere(transform.position, distance);
        }
    }

    /// <summary>
    /// 創建網格mesh 掃描 當作視野
    /// </summary>
    /// <returns></returns>
    Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 50; // 扇形分段數
        int numVertices = segments + 2; // 中心點 + 每個分段的兩個頂點
        int numTriangles = segments * 3; // 每個分段對應的三角形

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numTriangles];

        // 中心點
        vertices[0] = Vector3.zero;

        // 計算扇形頂點
        float currentAngle = -angle;
        float deltaAngle = angle * 2 / segments;

        for (int i = 0; i <= segments; i++)
        {
            // 計算頂點位置
            Vector3 vertex = Quaternion.Euler(0f, currentAngle, 0f) * Vector3.forward * distance;
            vertices[i + 1] = vertex;

            currentAngle += deltaAngle;
        }

        // 設定三角形索引
        int vert = 1; // 三角形的第一個頂點索引（從 1 開始）
        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;          // 中心點
            triangles[i * 3 + 1] = vert;  // 當前頂點
            triangles[i * 3 + 2] = vert + 1; // 下一個頂點

            vert++;
        }

        // 設定網格
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
    #endregion
}
