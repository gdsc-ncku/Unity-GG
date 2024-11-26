using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    #region ���ե��ܼ�
    public Transform keepPosition;
    public Camera playerCamera;
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
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UseFlyingSickle();
    }
    #endregion

    Weapon currWeapon;

    #region ���I�{���Ϭq
    //�D���㪩�A���Ⲿ�ʧ�����ݭn�ק�inputAction�����޿�
    public void UseFlyingSickle()
    {
        currWeapon = FlyingSickle.Instance;
        FlyingSickle.Instance.InitWeapon(keepPosition, playerCamera.gameObject);
        inputActions = PlayerManager.instance.playerControl;

        inputActions.player.leftclick.performed += FlyingSickle.Instance.LeftClickPerformed;
        inputActions.player.leftclick.canceled += FlyingSickle.Instance.LeftClickCanceled;

        inputActions.player.rightclick.performed += FlyingSickle.Instance.RightClickPerformed;


        inputActions.player.rightclick.performed += currWeapon.RightClickPerformed;
    }

    public void UseWeapon(Weapon weapon)
    {
        //weapon.InitWeapon();
        inputActions.player.leftclick.performed += weapon.LeftClickPerformed;
        inputActions.player.leftclick.canceled += weapon.LeftClickCanceled;
    }

    #endregion
}
