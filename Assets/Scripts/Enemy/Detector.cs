using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public Subject<GameObject> OnViewTarget = new();
    public Subject<Unit> OnTargetGone = new();
    public string targetTag { private get; set; }
    [SerializeField]float distance = 10; //偵測距離
    [SerializeField]float angle = 30;    //偵測角度
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
                if (taget != obj && obj.CompareTag(targetTag))
                {
                    taget = obj;
                    OnViewTarget.OnNext(taget);
                }
            }
        }
        if(taget != null && !detectedObjects.Contains(taget))
        {
            taget = null;
            OnTargetGone.OnNext(Unit.Default);
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
        if(deltaAngle > angle)
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
        if(Physics.Linecast(transform.position, target.transform.position, occlusionLayers))
        {
            return true;
        }
        return false;
    }
}
