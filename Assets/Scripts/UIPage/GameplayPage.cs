using UnityEngine;

public class GameplayPage : MonoBehaviour
{
    public void Configuration_OnClick()
    {
        EventManager.TriggerEvent(NameOfEvent.ToConfiguration);
    }
}
