using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    #region ���ե��ܼ�
    public Transform keepPosition;
    public Camera playerCamera;
    public GameObject player;
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
         Weapon.Instance.Init(keepPosition, playerCamera, inputActions);
        UseFlyingSickle();
    }
    #endregion

    #region ���I�{���Ϭq
    //�D���㪩�A���Ⲿ�ʧ�����ݭn�ק�inputAction�����޿�
    public void UseFlyingSickle()
    {
        FlyingSickle.Instance.InitWeapon(keepPosition, player);
        inputActions = PlayerManager.instance.playerControl;

        inputActions.player.leftclick.performed += FlyingSickle.Instance.LeftClickPerformed;
        inputActions.player.leftclick.canceled += FlyingSickle.Instance.LeftClickCanceled;

        inputActions.player.rightclick.performed += FlyingSickle.Instance.RightClickPerformed;
    }
    #endregion
}
