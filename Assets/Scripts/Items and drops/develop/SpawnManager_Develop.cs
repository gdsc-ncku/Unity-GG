using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

/// <summary>
///  重生點管理者 開發用
/// </summary>
public class SpawnManager_Develop : MonoBehaviour
{
    public List<GameObject> spawnPoint = new List<GameObject>();

    /// <summary>
    /// 展示重生點特效
    /// </summary>
    private void ShowSpawnPoint(bool isShow)
    {
        foreach (GameObject go in spawnPoint) {
            go.GetComponent<LineRenderer>().enabled = isShow;
        }
    }

    #region event
    //用於事件訂閱
    private CompositeDisposable disposables = new CompositeDisposable();

    private void OnEnable()
    {
        // 註冊對  事件的訂閱
        disposables.Add(EventManager.StartListening<bool>(
            NameOfEvent.ShowSpawnPoint,
            (isShow) => ShowSpawnPoint(isShow)
        ));
    }

    private void OnDisable()
    {
        // 取消註冊對  事件的訂閱
        disposables.Clear();
    }
    #endregion
}
