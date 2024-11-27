using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EnemyView : MonoBehaviour
{
    public Mesh mesh;
    public float distance = 10; //�����Z��
    public float angle = 30;    //��������
    public float height = 1.0f; //��������
    public Color meshColor = Color.blue;    //�C��i��

    public LayerMask layers;    //�i�[�layer
    public LayerMask occlusionLayers;

    Collider[] colliders = new Collider[50];
    int count;
    public int scanFrequency = 30;  //���y�W�v
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
    /// ���y���
    /// </summary>
    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layers, QueryTriggerInteraction.Collide);

        //�z��g�u�˴��쪺�I���� �ݭ��ǯu���b������
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
    /// �ˬd�b�|�P�򪺸I����O�_�u���s�b������
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
    /// �Ыغ���mesh ���y ��@����
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
    /// �ھګ��w��layer �˯��b��layer���U�Q���y�쪺����
    /// buffer�O�x�s���󪺦C��
    /// �^���˴��쪺�ƶq
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
    /// Debug��T
    /// </summary>
    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
    }

    /// <summary>
    /// Debug��T
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
