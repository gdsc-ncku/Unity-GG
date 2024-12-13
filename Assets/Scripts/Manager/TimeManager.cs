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
        // ���U�� "EnemyDeath" �ƥ󪺭q�\
        EventManager.StartListening<float>(NameOfEvent.TimeControl, TimeControl);
    }
    
    private void OnDisable()
    {
        // �������U�� "EnemyDeath" �ƥ󪺭q�\
        EventManager.StopListening<float>(NameOfEvent.TimeControl, TimeControl);
    }
}
