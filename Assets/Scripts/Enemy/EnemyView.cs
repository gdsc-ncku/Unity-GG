using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EnemyView : MonoBehaviour
{
    public Mesh mesh;
    public float distance = 10; //視野距離
    public float angle = 30;    //視野角度
    public float height = 1.0f; //視野高度
    public Color meshColor = Color.blue;    //顏色展示

    public LayerMask layers;    //可觀察的layer
    public LayerMask occlusionLayers;

    Collider[] colliders = new Collider[50];
    int count;
    public int scanFrequency = 30;  //掃描頻率
    float scanInterval;
    float scanTimer;

    private List<GameObject> scanObjects = new List<GameObject>();

    private void Start()
    {
        scanInterval = 1.0f / scanFrequency;
    }

    private void Update()
    {
        scanTimer -= Time.deltaTime;
        if(scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }

    /// <summary>
    /// 掃描函數
    /// </summary>
    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layers, QueryTriggerInteraction.Collide);

        //篩選射線檢測到的碰撞體 看哪些真的在視野中
        scanObjects.Clear();
        for(int i = 0; i < count; i++)
        {
            GameObject obj = colliders[i].gameObject;
            if(IsInsight(obj))
            {
                scanObjects.Add(obj);
            }
        }
    }

    /// <summary>
    /// 檢查在四周圍的碰撞體是否真的存在視野中
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private bool IsInsight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - origin;
        if(direction.y < 0 || direction.y > height)
        {
            return false;
        }

        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if(deltaAngle > angle)
        {
            return false;
        }

        origin.y += height / 2;
        dest.y = origin.y;
        if(Physics.Linecast(origin, dest, occlusionLayers))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 創建網格mesh 掃描 當作視野
    /// </summary>
    /// <returns></returns>
    private Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 50;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0f, -angle, 0f) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0f, angle, 0f) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;

        int vert = 0;

        //left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        //right center
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;

        for(int i = 0; i < segments; i++)
        {
            bottomLeft = Quaternion.Euler(0f, currentAngle, 0f) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0f, currentAngle + deltaAngle, 0f) * Vector3.forward * distance;

            topLeft = bottomLeft + Vector3.up * height;
            topRight = bottomRight + Vector3.up * height;


            //far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            //top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            //bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;
        }

        for (int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    /// <summary>
    /// 根據指定的layer 檢索在該layer底下被掃描到的物件
    /// buffer是儲存物件的列表
    /// 回傳檢測到的數量
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="layerName"></param>
    /// <returns></returns>
    public int Filter(GameObject[] buffer, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        int count = 0;
        foreach(var obj in scanObjects)
        {
            if(obj.layer == layer)
            {
                buffer[count++] = obj;
            }

            if(buffer.Length == count)
            {
                break;
            }
        }

        return count;
    }

    public bool IsViewPlayer()
    {
        GameObject[] gm = new GameObject[1];
        return Filter(gm, "Player") > 0 ? true : false;
    }

    /// <summary>
    /// Debug資訊
    /// </summary>
    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
    }

    /// <summary>
    /// Debug資訊
    /// </summary>
    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        Gizmos.color = Color.green;
        foreach(var obj in scanObjects)
        {
            Gizmos.DrawSphere(obj.transform.position, 0.2f);
        }
    }
}
