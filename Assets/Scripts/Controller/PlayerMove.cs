using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

public class PlayerMove : MonoBehaviour
{
    //public float speed = 20f;
    private float mouseSensitivity;
    private float maxSpeed;
    private Rigidbody playerRigibody;
    //public GameObject ItemUI;
    //public GameObject WeaponUI;
    //public GameObject CollectionUI;
    //public GameObject BackMask;
    // �ƹ��F�ӫ�
    private float xRotation; // 角色的垂直旋轉

    [Header("與時間放慢有關的移動設置")]
    [SerializeField] private bool isScaledByTime = true;
    [SerializeField] private Vector3 currentVelocity = Vector3.zero;
    public float friction = 0.9f; // 摩擦力係數（值越小減速越慢）

    //用於事件訂閱
    private CompositeDisposable disposables = new CompositeDisposable();

    //private void Awake()
    //{
    //    playerRigibody = PlayerManager.Instance.rb;
    //    Cursor.lockState = CursorLockMode.Locked; // ��w�ƹ�

    //    maxSpeed = PlayerManager.Instance.maxSpeed;
    //    mouseSensitivity = PlayerManager.Instance.mouseSensitivity; 
    //    xRotation = PlayerManager.Instance.xRotation;

    //    if(isLockCursor)
    //        Cursor.lockState = CursorLockMode.Locked; // 鎖定滑鼠
    //}

    private void Start()
    {
        playerRigibody = PlayerManager.Instance.rb;
        Cursor.lockState = CursorLockMode.Locked; // 鎖定滑鼠到視窗中

        maxSpeed = PlayerManager.Instance.maxSpeed;
        mouseSensitivity = PlayerManager.Instance.mouseSensitivity;
        xRotation = PlayerManager.Instance.xRotation;
    }

    private void FixedUpdate()
    {
        Movement();
        ViewportFocus();
    }

    private void ViewportFocus()
    {
        // ����ƹ����ʪ���J
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ��s��������A����W�U����
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -70f, 70f); // ��������b-70��70�פ���

        // ��s��v��������
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // ��s���⪺����
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Movement()
    {
        //���B���X�F(�U�h�ݭn�W�h���)�A����i��ҬO�_�NplayerControl����ScriptableObject�Ӹѽ��X
        Vector2 inputVector = PlayerManager.Instance.playerControl.player.move.ReadValue<Vector2>();

        // ������a���e��M�k����V
        Vector3 forward = playerRigibody.transform.forward;
        Vector3 right = playerRigibody.transform.right;

        if (PlayerManager.Instance.playerStatus == PlayerStatus.move && inputVector != Vector2.zero && Vector3.Distance(playerRigibody.linearVelocity, Vector3.zero) < maxSpeed)
        {
            //playerRigibody.velocity = moveDirection.normalized * maxSpeed;

            // 計算輸入的移動方向
            Vector3 moveDir = (playerRigibody.transform.forward * inputVector.y + playerRigibody.transform.right * inputVector.x).normalized;

            // 根據輸入更新目標速度
            Vector3 targetVelocity = moveDir * maxSpeed;

            // 使用 Lerp 模擬摩擦力，逐漸將速度拉近目標速度
            currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, friction * (isScaledByTime ? Time.deltaTime : Time.unscaledDeltaTime));

            // 計算實際移動距離
            Vector3 moveDistance = currentVelocity * (isScaledByTime ? Time.deltaTime : Time.unscaledDeltaTime);

            // 更新剛體的位置
            playerRigibody.MovePosition(playerRigibody.position + moveDistance);
        }
        else if(PlayerManager.Instance.playerStatus == PlayerStatus.move && inputVector == Vector2.zero)
        {
            //playerRigibody.velocity = new Vector3(0, playerRigibody.velocity.y, 0);

            //手動模擬摩擦力減速效果
            if(Vector3.Distance(currentVelocity, Vector3.zero) > 1f)
            {
                // 使用 Lerp 模擬摩擦力，逐漸將速度拉近目標速度
                currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, friction);

                // 計算實際移動距離
                Vector3 moveDistance = currentVelocity * (isScaledByTime ? Time.deltaTime : Time.unscaledDeltaTime);

                // 更新剛體的位置
                playerRigibody.MovePosition(playerRigibody.position + moveDistance);
            }
            else
            {
                currentVelocity = Vector3.zero;
            }
        }
    }

    public IEnumerator Sprint(Vector3 forward, float sprintDistance, int sprintFrame)
    {
        PlayerManager.Instance.playerStatus = PlayerStatus.sprint;
        PlayerManager.Instance.rb.linearVelocity = Vector3.zero;
        yield return new WaitForFixedUpdate();
        PlayerManager.Instance.rb.AddForce(2 * PlayerManager.Instance.rb.mass * sprintDistance / (Time.fixedDeltaTime * sprintFrame) * forward, ForceMode.Impulse);
        for (int i = 0; i < sprintFrame; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        PlayerManager.Instance.rb.linearVelocity = Vector3.zero;
        PlayerManager.Instance.playerStatus = PlayerStatus.move;
        yield break;
    }

    private void OnEnable()
    {
        // 註冊對  事件的訂閱
        disposables.Add(EventManager.StartListening<bool>(
            NameOfEvent.ChangeMoveMode,
            _isScaledByTime => ChangeMoveMode(_isScaledByTime)
        ));

        disposables.Add(EventManager.StartListening<bool>(
            NameOfEvent.ChangeCursorState,
            isLocked => ChangeCursorState(isLocked)
        ));
    }

    private void OnDisable()
    {
        // 取消註冊對  事件的訂閱
        disposables.Clear();
    }

    /// <summary>
    /// 更改當前鼠標狀態
    /// 是否鎖定到視窗中
    /// </summary>
    /// <param name="isLocked">是否上鎖</param>
    private void ChangeCursorState(bool isLocked)
    {
        if (isLocked == true)
            Cursor.lockState = CursorLockMode.Locked; // 鎖定滑鼠
        else if(isLocked == false)
            Cursor.lockState = CursorLockMode.None;
    }

    private void ChangeMoveMode(bool _isScaledByTime)
    {
        Debug.Log($"PlayMove: Move mode is changed (is scaled by time: {_isScaledByTime})");
        isScaledByTime = _isScaledByTime;
    }

    public void Jump(float jumpforce)
    {
        if(PlayerManager.Instance.playerStatus == PlayerStatus.move)
        {
            Debug.Log("Jump!");
            PlayerManager.Instance.playerStatus = PlayerStatus.jump;  
            PlayerManager.Instance.rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
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
