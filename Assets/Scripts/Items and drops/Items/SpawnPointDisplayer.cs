using UnityEngine;

/// <summary>
/// 重生點展示 會丟出來的東西
/// </summary>
public class SpawnPointDisplayer : MonoBehaviour
{
    [HideInInspector]public float sensorRegion = 0;
    [HideInInspector]public float destroyTime = 0;
    [HideInInspector]public bool isShowing = false;

    /// <summary>
    /// 初始化
    /// </summary>
    public void InitDisplayer(float r, float t)
    {
        sensorRegion = r;
        destroyTime = t;

        Destroy(gameObject, destroyTime);
    }

    private void FixedUpdate()
    {
        ShowSpawnPoint();
    }

    /// <summary>
    /// 展示重生點光束效果
    /// </summary>
    private void ShowSpawnPoint()
    {
        if(isShowing == false && sensorRegion >= 0 && IsNearPlayer())
        {
            Debug.Log("SpawnPointDisplayer: show soawn point !!!");
            EventManager.TriggerEvent(NameOfEvent.ShowSpawnPoint, true);
            isShowing = true;
        }
        else if(isShowing == true && sensorRegion >= 0 && IsNearPlayer() == false)
        {
            EventManager.TriggerEvent(NameOfEvent.ShowSpawnPoint, false);
            isShowing = false;
        }
    }

    /// <summary>
    /// 檢查旁邊是否有玩家
    /// </summary>
    /// <returns></returns>
    private bool IsNearPlayer()
    {
        float d = Vector3.Distance(PlayerManager.Instance.gameObject.transform.position, this.transform.position);
        if( d < sensorRegion)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDestroy()
    {
        EventManager.TriggerEvent(NameOfEvent.ShowSpawnPoint, false);
    }
}
