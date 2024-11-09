using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerStaticData", menuName = "ScriptableObjects/Data/PlayerStaticData", order = 0)]
public class PlayerStaticData : ScriptableObject
{
    #region Rigidbody設定
    private Rigidbody _rb = null;
    public Rigidbody rb 
    { 
        get 
        { 
            if (_rb == null) 
            { 
                Debug.LogError("Havn't setting player rigidbody"); 
            } 
            return _rb; 
        } 
    }
    public void SettingRigidbody(Rigidbody rigidbody)
    {
        _rb = rigidbody;
    }
    #endregion

    #region InputAction設定
    private InputAction _playerControl = null;
    public InputAction playerControl
    {
        get
        {
            if (_playerControl == null)
            {
                Debug.LogError("Havn't setting player input action");
            }
            return _playerControl;
        }
    }
    public void SettingPlayerControl(InputAction inputAction)
    {
        _playerControl = inputAction;
    }
    #endregion
}
