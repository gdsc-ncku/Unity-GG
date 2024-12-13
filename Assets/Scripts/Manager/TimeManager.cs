using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private void TimeControl(float rate)
    {
        Time.timeScale = rate;
    }

    private void OnEnable()
    {
        // 註冊對 "EnemyDeath" 事件的訂閱
        EventManager.StartListening<float>(NameOfEvent.TimeControl, TimeControl);
    }
    
    private void OnDisable()
    {
        // 取消註冊對 "EnemyDeath" 事件的訂閱
        EventManager.StopListening<float>(NameOfEvent.TimeControl, TimeControl);
    }
}
