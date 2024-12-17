using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 當前已經註冊的事件名稱 方便代碼索引
/// </summary>
[SerializeField]
public enum NameOfEvent
{
    [Header("TimeManager")]
    TimeControl,
    TimeResume,

    [Header("PlayerMove")]
    ChangeMoveMode,
}

/// <summary>
/// 事件管理器
/// 用於註冊、訂閱事件
/// </summary>
public class EventManager
{
    // 這裡使用一個泛型字典來存儲帶有不同參數的事件
    private static Dictionary<NameOfEvent, Delegate> eventDictionary = new Dictionary<NameOfEvent, Delegate>();

    #region 不帶參數
    // 重載，支持多個參數的情況
    // 註冊事件，這裡支持泛型
    public static void StartListening(NameOfEvent eventName, Action listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = Delegate.Combine(eventDictionary[eventName], listener);
        }
        else
        {
            eventDictionary.Add(eventName, listener);
        }
    }
    // 取消註冊事件
    public static void StopListening(NameOfEvent eventName, Action listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = Delegate.Remove(eventDictionary[eventName], listener);
        }
    }

    // 觸發事件，這裡支持傳遞不同類型的參數
    public static void TriggerEvent(NameOfEvent eventName)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            Action action = eventDictionary[eventName] as Action;
            action?.Invoke();
        }
    }
    #endregion

    #region 帶一個參數
    // 註冊事件，這裡支持泛型
    public static void StartListening<T>(NameOfEvent eventName, Action<T> listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = Delegate.Combine(eventDictionary[eventName], listener);
        }
        else
        {
            eventDictionary.Add(eventName, listener);
        }
    }
    // 取消註冊事件
    public static void StopListening<T>(NameOfEvent eventName, Action<T> listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = Delegate.Remove(eventDictionary[eventName], listener);
        }
    }

    // 觸發事件，這裡支持傳遞不同類型的參數
    public static void TriggerEvent<T>(NameOfEvent eventName, T parameter)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            Action<T> action = eventDictionary[eventName] as Action<T>;
            action?.Invoke(parameter);
        }
    }
    #endregion

    #region 帶兩個參數
    // 重載，支持多個參數的情況
    // 註冊事件，這裡支持泛型
    public static void StartListening<T1, T2>(NameOfEvent eventName, Action<T1, T2> listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = Delegate.Combine(eventDictionary[eventName], listener);
        }
        else
        {
            eventDictionary.Add(eventName, listener);
        }
    }
    // 取消註冊事件
    public static void StopListening<T1, T2>(NameOfEvent eventName, Action<T1, T2> listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = Delegate.Remove(eventDictionary[eventName], listener);
        }
    }

    // 觸發事件，這裡支持傳遞不同類型的參數
    public static void TriggerEvent<T1, T2>(NameOfEvent eventName, T1 p1, T2 p2)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            Action<T1, T2> action = eventDictionary[eventName] as Action<T1, T2>;
            action?.Invoke(p1, p2);
        }
    }
    #endregion

}
