using Unity.VisualScripting;
using UnityEngine;

public enum PlayerStatus
{
    move,
    sprint
}

public class PlayerManager : MonoBehaviour
{
    [SerializeField] int sprintFrame;
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
    }
    #endregion

    public void Sprint(Vector3 forward, float sprintDistance)
    {
        StartCoroutine(GetComponent<PlayerMove>().Sprint(forward, sprintDistance, sprintFrame));
    }
}
