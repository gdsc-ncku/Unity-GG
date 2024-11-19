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

    #region 飛鐮程式區段
    //非完整版，角色移動完成後需要修改inputAction相關邏輯
    public void UseFlyingSickle()
    {
        FlyingSickle.Instance.InitWeapon(keepPosition, playerCamera);
        inputActions = PlayerManager.instance.playerControl;

        inputActions.player.leftclick.performed += FlyingSickle.Instance.LeftClickPerformed;
        inputActions.player.leftclick.canceled += FlyingSickle.Instance.LeftClickCanceled;

        inputActions.player.rightclick.performed += FlyingSickle.Instance.RightClickPerformed;
    }
    #endregion
}
