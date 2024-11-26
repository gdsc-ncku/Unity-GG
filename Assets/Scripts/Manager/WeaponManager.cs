using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    #region 測試用變數
    public Transform keepPosition;
    public GameObject playerCamera;
    public PlayerControl inputActions;
    #endregion

    #region 建立單例模式
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

    #region 初始化
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

    #region 飛鐮程式區段
    //非完整版，角色移動完成後需要修改inputAction相關邏輯
    public void UseFlyingSickle()
    {
        currWeapon = FlyingSickle.Instance;
        FlyingSickle.Instance.InitWeapon(keepPosition, playerCamera);
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
