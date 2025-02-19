using System.Collections;
using UniRx;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float mouseSensitivity;
    private float maxSpeed;
    private float xRotation; // 角色的垂直旋轉

    public bool isLockCursor = true;

    [Header("與時間放慢有關的移動設置")]
    [SerializeField] private bool isScaledByTime = true;

    //用於事件訂閱
    private CompositeDisposable disposables = new CompositeDisposable();

    private CharacterController characterController;
    private Vector3 velocity; // 控制角色的重力影響

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked; // ��w�ƹ�

        maxSpeed = PlayerManager.Instance.maxSpeed;
        mouseSensitivity = PlayerManager.Instance.mouseSensitivity; 
        xRotation = PlayerManager.Instance.xRotation;

        if(isLockCursor)
            Cursor.lockState = CursorLockMode.Locked; // 鎖定滑鼠
    }

    private void Start()
    {
        characterController = PlayerManager.Instance.characterController;
    }

    private void FixedUpdate()
    {
        Movement();
        ViewportFocus();
    }

    private void ViewportFocus()
    {
        // 獲取滑鼠移動的輸入
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 更新垂直旋轉，限制上下角度
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -70f, 70f); // 限制視角在-70到70度之間

        // 更新攝影機的旋轉
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // 更新角色的旋轉
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Movement()
    {
        // 取得玩家輸入
        Vector2 inputVector = PlayerManager.Instance.playerControl.player.move.ReadValue<Vector2>();

        // 計算移動方向
        Vector3 moveDirection = (transform.forward * inputVector.y +
                                 transform.right * inputVector.x).normalized;
        Vector3 newVelocity = moveDirection * maxSpeed * (isScaledByTime ? Time.deltaTime : Time.unscaledDeltaTime / Time.timeScale);
        characterController.Move(newVelocity);

        if (!characterController.isGrounded)
        {
            velocity.y -= GameManager.Instance.playerGravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }
        else
        {
            velocity.y = -0.1f; // 避免角色浮空
        }
    }



    public IEnumerator Sprint(Vector3 forward, float sprintDistance, int sprintFrame)
    {
        /*PlayerManager.Instance.playerStatus = PlayerStatus.sprint;
        PlayerManager.Instance.rb.linearVelocity = Vector3.zero;
        yield return new WaitForFixedUpdate();
        PlayerManager.Instance.rb.AddForce(2 * PlayerManager.Instance.rb.mass * sprintDistance / (Time.fixedDeltaTime * sprintFrame) * forward, ForceMode.Impulse);
        for (int i = 0; i < sprintFrame; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        PlayerManager.Instance.rb.linearVelocity = Vector3.zero;
        PlayerManager.Instance.playerStatus = PlayerStatus.move;*/
        yield break;
    }

    private void OnEnable()
    {
        // 註冊對  事件的訂閱
        disposables.Add(EventManager.StartListening<bool>(
            NameOfEvent.ChangeMoveMode,
            _isScaledByTime => ChangeMoveMode(_isScaledByTime)
        ));
    }

    private void OnDisable()
    {
        // 取消註冊對  事件的訂閱
        disposables.Clear();
    }

    private void ChangeMoveMode(bool _isScaledByTime)
    {
        Debug.Log($"PlayMove: Move mode is changed (is scaled by time: {_isScaledByTime})");
        isScaledByTime = _isScaledByTime;
    }

    public void Jump(float jumpHeight)
    {
        if(characterController.isGrounded)
        {
            Debug.Log("Jump!");
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * GameManager.Instance.playerGravity);
            characterController.Move(velocity * Time.deltaTime);
        }
        else
        {
            Debug.Log("player is jumping now");
        }
    }
    /*
    public void Item()
    {
        //啟用指定UI
        if (ItemUI == null)
        {
            Debug.LogError("Backpack UI is not assigned!");
            return;
        }

        
        ItemUI.SetActive(true); // 切換背包顯示狀態
        BackMask.SetActive(true);
        
        // 當背包開啟時，解除鎖定滑鼠
        Debug.Log(ItemUI.activeSelf);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //PlayerManager.Instance.playerStatus = PlayerStatus.ui; // 更新玩家狀態為 UI 模式，我只是猜你可能會這樣寫，要不要隨便你.jpg
    
        // 當背包關閉時，重新鎖定滑鼠
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        //PlayerManager.Instance.playerStatus = PlayerStatus.move; // 恢復到移動模式，我只是猜你可能會這樣寫，要不要隨便你.jpg
        
    }

    public void CloseUI()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // 切換背包顯示狀態
        BackMask.SetActive(false);
        ItemUI.SetActive(false); 
        //WeaponUI.SetActive(false);
        //CollectionUI.SetActive(false);
        //Debug.Log("test2");
    }

    public void Setting()
    {
        
    }
    */
}
