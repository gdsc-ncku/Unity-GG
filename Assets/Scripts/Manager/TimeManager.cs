﻿using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// 掌控遊戲中跟時間有關的事物
/// </summary>
public class TimeManager : MonoBehaviour
{
    private CompositeDisposable disposables = new CompositeDisposable();

    bool isTimeControled = false;   //如果時間已經被某個東西操控 則後來者無法操控

    /// <summary>
    /// 操控時間
    /// </summary>
    /// <param name="slowdownFactor">指定的倍率</param>
    private void TimeControl(float slowdownFactor)
    {
        if(isTimeControled == false)
        {
            Debug.Log($"TimeManager: Time is modified to {slowdownFactor}");
            Time.timeScale = slowdownFactor; // 將全局時間縮放設置為 slowdownFactor
            Time.fixedDeltaTime = 0.02f * Time.timeScale; // 確保物理模擬同步

            isTimeControled = true;
        }
        else
        {
            Debug.Log("TimeManager: Time is already controled by someone");
        }
    }

    /// <summary>
    /// 回復時間設定
    /// </summary>
    private void TimeResume()
    {
        Debug.Log($"TimeManager: Time is resume");
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // 確保物理模擬同步
        isTimeControled = false;
    }

    private void OnEnable()
    {
        // 註冊對  事件的訂閱
        disposables.Add(EventManager.StartListening<float>(
            NameOfEvent.TimeControl,
            slowdownFactor => TimeControl(slowdownFactor)
        ));

        disposables.Add(EventManager.StartListening(
            NameOfEvent.TimeResume,
            TimeResume
        ));
    }
    
    private void OnDisable()
    {
        disposables.Clear(); // 自動取消所有事件訂閱
    }
}