using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    #region 建立單例模式
    static private PlayerManager _instance = null;
    public static PlayerManager instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Can't Find WeaponManager Instance");
            }
            return _instance;
        }
    }
    #endregion

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

        set
        {
            Debug.LogError("You can't directly set player rigidbody, you should using function to set");
        }
    }
    #endregion

    #region InputAction設定
    [SerializeField] InputAction _playerControl;
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

        set
        {
            Debug.LogError("You can't directly set player input action, you should using function to set");
        }
    }
    #endregion

    #region 初始化
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogError("Duplicate creating PlayerManager Instance");
            Destroy(gameObject);
        }

        _rb = _instance.GetComponent<Rigidbody>();
    }
    #endregion
}
