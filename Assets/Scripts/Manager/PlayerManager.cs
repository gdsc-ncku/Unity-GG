using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public enum PlayerStatus
{
    move,
    sprint,
    jump
}

[DefaultExecutionOrder(-100)] // 負數越小越早執行
public class PlayerManager : MonoBehaviour
{
    [SerializeField] int sprintFrame;

    public float mouseSensitivity = 100f;

    public float maxSpeed = 15f;
    public float jumpforce = 200f;
    public float xRotation = 0f;
    public Transform playerTransform { get; private set; }
    #region 建立單例模式
    //instance mode
    private static PlayerManager _instance;
    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Can't Find Player Manager Instance");
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
    [SerializeField] PlayerControl _playerControl;
    public PlayerControl playerControl
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

    #region 狀態機
    public PlayerStatus playerStatus;
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

        _playerControl = new();
        _playerControl.Enable();
        _rb = Instance.GetComponent<Rigidbody>();
        playerStatus = PlayerStatus.move;
        playerTransform = this.transform;

        _playerControl.player.rebinding.performed += ctx => Rebinding();


        _playerControl.player.jump.performed += ctx => Jump();

        _playerControl.player.Item.performed += ctx => Item();

        _playerControl.player.CloseUI.performed += ctx => CloseUI();

        _playerControl.player.Setting.performed += ctx => Setting();

        /*
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
*/


    }
    #endregion

    public void Sprint(UnityEngine.Vector3 forward, float sprintDistance)
    {
        StartCoroutine(GetComponent<PlayerMove>().Sprint(forward, sprintDistance, sprintFrame));
    }

    public void Jump()
    {
        
        //跳多大力
        GetComponent<PlayerMove>().Jump(jumpforce);
    }

    public void Item()
    {
        //GetComponent<PlayerMove>().Item();
        EventManager.TriggerEvent(NameOfEvent.OpenItemPage);
    }

    public void Setting()
    {
        GetComponent<PlayerMove>().Setting();
    }

    public void CloseUI()
    {
        //GetComponent<PlayerMove>().CloseUI();
        EventManager.TriggerEvent(NameOfEvent.CloseUI);
    }


    public void Rebinding()//目前只能rebinding jump這個動作的按鍵，但可以很輕鬆的改到需要的動作上
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
