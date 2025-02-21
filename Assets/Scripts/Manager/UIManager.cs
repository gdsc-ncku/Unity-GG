using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;


public class UIManager: MonoBehaviour 
{
    PlayerControl _playerControl;
    
    
    
    void Start()
    {
        _playerControl = PlayerManager.Instance.playerControl;

        _playerControl.player.Item.performed += ctx => Item();

        _playerControl.player.CloseUI.performed += ctx => CloseUI();

        _playerControl.player.Setting.performed += ctx => Setting();

        //GetComponent<UIController>().Reset();
    }
    
    //UIController
    public void Item()
    {
        GetComponent<UIController>().Item();
    }

    public void CloseUI()
    {
        GetComponent<UIController>().CloseUI();
    }

    public void Setting()
    {
        GetComponent<UIController>().Setting();
    }
    

    //Backpage
    public void On_pagebtn_Item_click()
    {
        Debug.Log("btnItem clicked");
    }
    public void On_pagebtn_Weapon_click()
    {
        Debug.Log("btnWeapon clicked");
    }
    public void On_pagebtn_Collection_click()
    {
        Debug.Log("btnCollection clicked");
    }

    //Setting
    public void On_Gameplay_click()
    {
        GetComponent<UIController>().ToGameplay();
    }

    public void On_Configuration_click()
    {
        GetComponent<UIController>().ToConfiguration();
    }

    public void On_Graphic_click()
    {
        GetComponent<UIController>().ToGraphic();
    }

    public void On_Audio_click()
    {
        GetComponent<UIController>().ToAudio();
    }
    public void On_Back_click()
    {
        GetComponent<UIController>().Setting_Back();
    }
    

    //Rebinding
    public void On_Rebinding_Move_Forward_click(Button btn)
    {
        _playerControl.player.move.Disable();
        int forwardBindingIndex = 1;//表示停用第幾個子綁定
        Debug.Log(_playerControl.player.move);
        
        _playerControl.player.move.PerformInteractiveRebinding(forwardBindingIndex)
            .OnComplete(callback => {
                Debug.Log("Rebinding forward complete.");
                callback.Dispose();

                // 列出所有綁定
                foreach (var binding in _playerControl.player.move.bindings)
                {
                    Debug.Log($"Binding: {binding.path}");
                }
                // 重新啟用 move 動作
                _playerControl.player.move.Enable();

                TextMeshProUGUI buttonText = btn.GetComponentInChildren<TextMeshProUGUI>(); 
                if (buttonText != null)
                {
                    buttonText.text = $"{_playerControl.player.move.bindings[forwardBindingIndex].path}";
                    Debug.Log("Button Text: " + buttonText.text);
                }
                else
                {
                    Debug.Log("No Text component found on the button.");
                }
            })
            .Start();

        
    }

    public void On_Rebinding_Move_Back_click()
    {
        _playerControl.player.move.Disable();
        int forwardBindingIndex = 2;//表示停用第幾個子綁定
        Debug.Log(_playerControl.player.move);
        
        _playerControl.player.move.PerformInteractiveRebinding(forwardBindingIndex)
            .OnComplete(callback => {
                Debug.Log("Rebinding forward complete.");
                callback.Dispose();

                // 列出所有綁定
                foreach (var binding in _playerControl.player.move.bindings)
                {
                    Debug.Log($"Binding: {binding.path}");
                }
                // 重新啟用 move 動作
                _playerControl.player.move.Enable();
            })
            .Start();
    }

    public void On_Rebinding_Move_Left_click()
    {
        _playerControl.player.move.Disable();
        int forwardBindingIndex = 3;//表示停用第幾個子綁定
        Debug.Log(_playerControl.player.move);
        
        _playerControl.player.move.PerformInteractiveRebinding(forwardBindingIndex)
            .OnComplete(callback => {
                Debug.Log("Rebinding forward complete.");
                callback.Dispose();

                // 列出所有綁定
                foreach (var binding in _playerControl.player.move.bindings)
                {
                    Debug.Log($"Binding: {binding.path}");
                }
                // 重新啟用 move 動作
                _playerControl.player.move.Enable();
            })
            .Start();
    }

    public void On_Rebinding_Move_Right_click()
    {
        _playerControl.player.move.Disable();
        int forwardBindingIndex = 4;//表示停用第幾個子綁定
        Debug.Log(_playerControl.player.move);
        
        _playerControl.player.move.PerformInteractiveRebinding(forwardBindingIndex)
            .OnComplete(callback => {
                Debug.Log("Rebinding forward complete.");
                callback.Dispose();

                // 列出所有綁定
                foreach (var binding in _playerControl.player.move.bindings)
                {
                    Debug.Log($"Binding: {binding.path}");
                }
                // 重新啟用 move 動作
                _playerControl.player.move.Enable();
            })
            .Start();
    }

    public void On_Rebinding_Jump_click()
    {
        _playerControl.player.jump.Disable();

        _playerControl.player.jump.PerformInteractiveRebinding()
            .OnComplete(callback => {
                Debug.Log("Rebinding complete.");
                callback.Dispose();
                // 在這裡查看綁定的按鍵
                foreach (var binding in _playerControl.player.jump.bindings)
                {
                    Debug.Log($"Binding: {binding.path}");
                }

                // 重綁定完成後啟用 action
                _playerControl.player.jump.Enable();
            })
            .Start();
    }

    
}
