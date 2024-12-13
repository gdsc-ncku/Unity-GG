using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    bool isTimeControled = false;   //�p�G�ɶ��w�g�Q�Y�ӪF��ޱ� �h��Ӫ̵L�k�ޱ�

    /// <summary>
    /// �ޱ��ɶ�
    /// </summary>
    /// <param name="slowdownFactor"></param>
    private void TimeControl(float slowdownFactor)
    {
        if(isTimeControled == false)
        {
            Time.timeScale = slowdownFactor; // �N�����ɶ��Y��]�m�� slowdownFactor
            Time.fixedDeltaTime = 0.02f * Time.timeScale; // �T�O���z�����P�B
        }
        else
        {
            Debug.Log("TimeManager: Time is already controled by someone");
        }
    }

    /// <summary>
    /// �^�_�ɶ��]�w
    /// </summary>
    private void TimeResume()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // �T�O���z�����P�B
    }

    private void OnEnable()
    {
        // ���U��  �ƥ󪺭q�\
        EventManager.StartListening<float>(NameOfEvent.TimeControl, TimeControl);
        EventManager.StartListening(NameOfEvent.TimeResume, TimeResume);
    }
    
    private void OnDisable()
    {
        // �������U��  �ƥ󪺭q�\
        EventManager.StopListening<float>(NameOfEvent.TimeControl, TimeControl);
        EventManager.StopListening(NameOfEvent.TimeResume, TimeResume);
    }
}
