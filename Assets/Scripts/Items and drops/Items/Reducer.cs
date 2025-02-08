using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 道具 減速器的實現代碼
/// </summary>
public class Reducer : Item
{
    public float slowdownFactor = 0.3f;
    public float duration = 3f;

    /// <summary>
    /// 使用減速器
    /// </summary>
    public override void ItemUsing()
    {
        base.ItemUsing();
        EventManager.TriggerEvent<float>(NameOfEvent.TimeControl, slowdownFactor);
        EventManager.TriggerEvent<bool>(NameOfEvent.ChangeMoveMode, false);

        // 使用 UniRx 的延遲來處理超時
        Observable.Timer(System.TimeSpan.FromSeconds(duration))
            .Subscribe(_ =>
            {
                ReducerTimeout();
            })
            .AddTo(this);
    }

    /// <summary>
    /// 減速器時間到 關閉效果
    /// </summary>
    private void ReducerTimeout()
    {
        EventManager.TriggerEvent(NameOfEvent.TimeResume);
        EventManager.TriggerEvent<bool>(NameOfEvent.ChangeMoveMode, true);
    }
}
