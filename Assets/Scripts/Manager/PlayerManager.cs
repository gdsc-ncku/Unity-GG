using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerStatus
{
    move,
    sprint
}

public class PlayerManager : MonoBehaviour
{
    [SerializeField] int sprintFrame;
    #region �إ߳�ҼҦ�
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

        _rb = _instance.GetComponent<Rigidbody>();
        playerStatus = PlayerStatus.move;
    }
    #endregion

    //�ȩ�A����n�h��Player movement��
    //StartCoroutine(debug());
    public IEnumerator Sprint(Vector3 forward, float sprintDistance)
    {
        PlayerManager.instance.playerStatus = PlayerStatus.sprint;
        rb.velocity = Vector3.zero;
        yield return new WaitForFixedUpdate();
        PlayerManager.instance.rb.AddForce(2 * rb.mass * sprintDistance / (Time.fixedDeltaTime * sprintFrame) * forward, ForceMode.Impulse);
        for (int i = 0; i < sprintFrame; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        rb.velocity = Vector3.zero;
        PlayerManager.instance.playerStatus = PlayerStatus.move;
        yield break;
    }
}