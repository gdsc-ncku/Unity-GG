using UnityEngine;

public class UIPage_BtnEvent : MonoBehaviour
{
    public void Gameplay_OnClick()
    {
        EventManager.TriggerEvent(NameOfEvent.ToGameplay);
    }
    public void Configuration_OnClick()
    {
        EventManager.TriggerEvent(NameOfEvent.ToConfiguration);
    }
    public void Graphic_OnClick()
    {
        EventManager.TriggerEvent(NameOfEvent.ToGraphic);
    }
    public void Audio_OnClick()
    {
        EventManager.TriggerEvent(NameOfEvent.ToAudio);
    }
    public void Back_OnClick()
    {
        EventManager.TriggerEvent(NameOfEvent.Setting_Back);
    }
    public void Item_OnClick()
    {
        EventManager.TriggerEvent(NameOfEvent.ToItem);
    }
    public void Weapon_OnClick()
    {
        EventManager.TriggerEvent(NameOfEvent.ToWeapon);
    }
    public void Collection_OnClick()
    {
        EventManager.TriggerEvent(NameOfEvent.ToCollection);
    }
}
