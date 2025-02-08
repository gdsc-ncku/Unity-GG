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
    [SerializeField] LayerMask selfLayerMask;
    LayerMask occlusionLayers => LayerMask.GetMask(LayerTagPack.Environment);       //不偵測的層
    Collider[] colliders = new Collider[50]; //偵測到的物件(暫存器)

    private void Start()
    {
        selfLayerMask = LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer));
    }

    /// <summary>
    /// 偵測
    /// 在behavior tree的function下方可以直接丟LayerTagPack裡的layer進去或是GetMask完在丟進去
    /// 這裡這樣做是因為敵人、玩家偵測分開才可以做不同行為，也可以傳入參數時丟LayerMask.GetMask(LayerTagPack.layer1, LayerTagPack.layer2)來偵測多種layer
    /// </summary>
    public GameObject Detect(LayerMask layer)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, distance, layer, QueryTriggerInteraction.Collide);

        /// <summary>
        /// 按優先級和距離從(優先級高且近)到(優先級低且遠)排序
        /// 主要邏輯: 因為只有三個狀態，所以把優先級最高(runAway)跟優先級最低(stay)拆出來判斷，就可以用簡單的if在O(1)解決
        /// 先判斷優先級是否相同
        /// 相同: 用距離判斷排序
        /// 不相同: 是否有人優先級最低，有->無條件選另一個，無->其中必定有一人優先級最高(因為stay與優先級相等在上面被排除了)
        /// 稍微有點Tricky，但因為這是內部的排序所以如果再用查表的方式解決問題會導致這邊變成O(n^2)解，太慢了
        /// </summary>
        Array.Sort(colliders, (a, b) =>
        {
            int aFirst = -1, bFirst = 1;
            LayerMask LayerMaskA = LayerMask.GetMask(LayerMask.LayerToName(a.gameObject.layer))
                    , LayerMaskB = LayerMask.GetMask(LayerMask.LayerToName(b.gameObject.layer));
            int valueA = EnemyDiagrams.enemyDiagram[selfLayerMask][LayerMaskA]
                , valueB = EnemyDiagrams.enemyDiagram[selfLayerMask][LayerMaskB];
            //查表狀態相同距離近的先偵測
            if(valueA == valueB)
            {
                float distA = Vector3.Distance(transform.position, a.transform.position);
                float distB = Vector3.Distance(transform.position, b.transform.position);
                return distA.CompareTo(distB);
            }
            else
            {
                //查表狀態不同，stay優先級最低
                if (valueA == EnemyDiagrams.stay)
                {
                    return bFirst;
                }
                else if(valueB == EnemyDiagrams.stay)
                {
                    return aFirst;
                }

                //查表狀態不同且無人是stay，runAway優先級最高
                if(valueA == EnemyDiagrams.runAway)
                {
                    return aFirst;
                }
                else if(valueB == EnemyDiagrams.runAway)
                {
                    return bFirst;
                }
            }

            //此狀況不會發生
            return 0;
        });

        foreach (Collider collider in colliders)
        {
            GameObject obj = collider.gameObject;
            if (obj == transform.root.gameObject) continue;
            if (IsVisible(obj))
            {
                return obj;
            }
        }

        return null;
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
