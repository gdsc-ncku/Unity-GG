using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
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
    #region �إ߳�ҼҦ�
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

    #region Rigidbody�]�w
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

    #region InputAction�]�w
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

    #region ���A��
    public PlayerStatus playerStatus;
    #endregion

    #region ��l��
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

        _playerControl.player.jump.performed += ctx => Jump();
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
}
