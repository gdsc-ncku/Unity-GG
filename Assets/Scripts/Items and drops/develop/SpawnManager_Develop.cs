using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

/// <summary>
///  �����I�޲z�� �}�o��
/// </summary>
public class SpawnManager_Develop : MonoBehaviour
{
    public List<GameObject> spawnPoint = new List<GameObject>();

    /// <summary>
    /// �i�ܭ����I�S��
    /// </summary>
    private void ShowSpawnPoint(bool isShow)
    {
        foreach (GameObject go in spawnPoint) {
            go.GetComponent<LineRenderer>().enabled = isShow;
        }
    }

    #region event
    //�Ω�ƥ�q�\
    private CompositeDisposable disposables = new CompositeDisposable();

    private void OnEnable()
    {
        // ���U��  �ƥ󪺭q�\
        disposables.Add(EventManager.StartListening<bool>(
            NameOfEvent.ShowSpawnPoint,
            (isShow) => ShowSpawnPoint(isShow)
        ));
    }

    private void OnDisable()
    {
        // �������U��  �ƥ󪺭q�\
        disposables.Clear();
    }
    #endregion
}
