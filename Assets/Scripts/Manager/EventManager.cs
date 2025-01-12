using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

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
    ChangeCursorState,
}

/// <summary>
/// 事件管理器
/// 用於註冊、訂閱事件
/// 
/// 需要訂閱的人
/// 選擇自己的參數 呼叫StartListening的多型進行訂閱
/// 要記得保留該訂閱在呼叫者本地的dispose中
/// 並在呼叫者釋放的時候 把訂閱給釋放掉
/// 
/// 訂閱的方法就是一般UniRx的方法
/// 詳情可以參考TimeManager
/// 
/// 目前支援0, 1, 2個參數
/// 有需要請在issue zhwa 我會寫更多支援
/// </summary>
public class EventManager
{
    // 使用 Subject 來管理事件流
    private static Dictionary<NameOfEvent, object> eventSubjects = new Dictionary<NameOfEvent, object>();

    /// <summary>
    /// 獲取或創建 Subject
    /// </summary>
    /// <typeparam name="T">事件所帶參數</typeparam>
    /// <param name="eventName">事件名稱</param>
    /// <returns></returns>
    private static Subject<T> GetOrCreateSubject<T>(NameOfEvent eventName)
    {
        if (!eventSubjects.ContainsKey(eventName))
        {
            eventSubjects[eventName] = new Subject<T>();
        }
        return (Subject<T>)eventSubjects[eventName];
    }

    #region 不帶參數

    // 訂閱不帶參數的事件
    
    /// <summary>
    /// 訂閱事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    /// <returns></returns>
    public static IDisposable StartListening(NameOfEvent eventName, Action listener)
    {
        return GetOrCreateSubject<Unit>(eventName).Subscribe(_ => listener());
    }

    /// <summary>
    /// 觸發不帶參數的事件
    /// </summary>
    /// <param name="eventName"></param>
    public static void TriggerEvent(NameOfEvent eventName)
    {
        GetOrCreateSubject<Unit>(eventName).OnNext(Unit.Default);
    }
    #endregion

    #region 帶一個參數

    /// <summary>
    /// 訂閱事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    /// <returns></returns>
    public static IDisposable StartListening<T>(NameOfEvent eventName, Action<T> listener)
    {
        return GetOrCreateSubject<T>(eventName).Subscribe(listener);
    }

    /// <summary>
    /// 觸發事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="parameter"></param>
    public static void TriggerEvent<T>(NameOfEvent eventName, T parameter)
    {
        GetOrCreateSubject<T>(eventName).OnNext(parameter);
    }

    #endregion

    #region 帶兩個參數
    /// <summary>
    /// 重載，支持多個參數的情況
    /// 註冊事件，這裡支持泛型
    /// 訂閱帶兩個參數的事件
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    /// <returns></returns>
    public static IDisposable StartListening<T1, T2>(NameOfEvent eventName, Action<T1, T2> listener)
    {
        return GetOrCreateSubject<(T1, T2)>(eventName).Subscribe(tuple => listener(tuple.Item1, tuple.Item2));
    }

    /// <summary>
    /// 觸發帶兩個參數的事件
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    public static void TriggerEvent<T1, T2>(NameOfEvent eventName, T1 param1, T2 param2)
    {
        GetOrCreateSubject<(T1, T2)>(eventName).OnNext((param1, param2));
    }
    #endregion

}
