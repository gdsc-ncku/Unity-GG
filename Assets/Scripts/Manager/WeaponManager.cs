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
    public static WeaponManager Instance
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
        //UseFlyingSickle();
        inputActions = PlayerManager.Instance.playerControl;

        UseWeapon(Shotgun.Instance);
    }
    #endregion

    Weapon currWeapon;

    #region ���I�{���Ϭq

    public void UseWeapon(Weapon weapon)
    {
        //初始化武器
        weapon.Init(keepPosition, playerCamera);

        inputActions.player.leftclick.performed += weapon.LeftClickPerformed;
        inputActions.player.leftclick.canceled += weapon.LeftClickCanceled;

        inputActions.player.rightclick.performed += weapon.RightClickPerformed;

        inputActions.player.rclick.performed += weapon.RClickPerformed;
    }

    #endregion
}
