using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    #region ���ե��ܼ�
    public Transform keepPosition;
    public GameObject playerCamera;
    public PlayerControl inputActions;
    #endregion

    #region �إ߳�ҼҦ�
    static private WeaponManager _instance = null;
    public static WeaponManager instance
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

    #region ��l��
    private void Awake()
    {
        if(_instance ==  null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogError("Duplicate creating WeaponManager Instance");
            Destroy(this);
        }
    }

    private void Start()
    {
        UseFlyingSickle();        
    }
    #endregion

    #region ���I�{���Ϭq
    //�D���㪩�A���Ⲿ�ʧ�����ݭn�ק�inputAction�����޿�
    public void UseFlyingSickle()
    {
        FlyingSickle.Instance.InitWeapon(keepPosition, playerCamera);
        inputActions = PlayerMove.Instance.inputActions;

        inputActions.player.leftclick.performed += LeftClick;
        inputActions.player.leftclick.canceled += Leftclick_canceled;

        inputActions.player.rightclick.performed += RightClick;
    }

    private void Leftclick_canceled(InputAction.CallbackContext obj)
    {
        FlyingSickle.Instance.LeftClick(2);
    }

    private void LeftClick(InputAction.CallbackContext context)
    {
        FlyingSickle.Instance.LeftClick(0);
    }

    private void RightClick(InputAction.CallbackContext context)
    {
        FlyingSickle.Instance.RightClick();
    }
    #endregion
}
