using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

public class PlayerMove : MonoBehaviour
{
    //public float speed = 20f;
    public float maxSpeed = 15;
    private Rigidbody playerRigibody;
    public float mouseSensitivity = 100f; // 滑鼠靈敏度
    private float xRotation = 0f; // 角色的垂直旋轉

    public bool isLockCursor = true;
    [SerializeField] private bool isScaledByTime = true;

    private void Awake()
    {
        playerRigibody = GetComponent<Rigidbody>();

        if(isLockCursor)
            Cursor.lockState = CursorLockMode.Locked; // 鎖定滑鼠
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
        //此處耦合了(下層需要上層資料)，後續可思考是否將playerControl移到ScriptableObject來解耦合
        Vector2 inputVector = PlayerManager.Instance.playerControl.player.move.ReadValue<Vector2>();

        // 獲取玩家的前方和右側方向
        Vector3 forward = playerRigibody.transform.forward;
        Vector3 right = playerRigibody.transform.right;

        // 根據輸入計算移動方向
        Vector3 moveDirection = forward * inputVector.y + right * inputVector.x;

        if (PlayerManager.Instance.playerStatus == PlayerStatus.move && inputVector != Vector2.zero && Vector3.Distance(playerRigibody.velocity, Vector3.zero) < maxSpeed)
        {
            //playerRigibody.velocity = moveDirection.normalized * maxSpeed;

            // 計算移動方向
            Vector3 moveDir = (playerRigibody.transform.forward * inputVector.y + playerRigibody.transform.right * inputVector.x).normalized;
            Vector3 moveDistance = moveDir * maxSpeed;

            // 根據 Time.timeScale 是否縮放，選擇合適的時間間隔
            float deltaTime = isScaledByTime ? Time.deltaTime : Time.unscaledDeltaTime;

            // 這樣可以手動更新位置，避免受 Time.timeScale 影響
            playerRigibody.MovePosition(playerRigibody.position + moveDistance * deltaTime);
        }
        else if(PlayerManager.Instance.playerStatus == PlayerStatus.move && inputVector == Vector2.zero)
        {
            playerRigibody.velocity = new Vector3(0, playerRigibody.velocity.y, 0);
        }
    }

    public IEnumerator Sprint(Vector3 forward, float sprintDistance, int sprintFrame)
    {
        PlayerManager.Instance.playerStatus = PlayerStatus.sprint;
        PlayerManager.Instance.rb.velocity = Vector3.zero;
        yield return new WaitForFixedUpdate();
        PlayerManager.Instance.rb.AddForce(2 * PlayerManager.Instance.rb.mass * sprintDistance / (Time.fixedDeltaTime * sprintFrame) * forward, ForceMode.Impulse);
        for (int i = 0; i < sprintFrame; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        PlayerManager.Instance.rb.velocity = Vector3.zero;
        PlayerManager.Instance.playerStatus = PlayerStatus.move;
        yield break;
    }

    private void OnEnable()
    {
        // 註冊對  事件的訂閱
        EventManager.StartListening<bool>(NameOfEvent.ChangeMoveMode, ChangeMoveMode);
    }

    private void OnDisable()
    {
        // 取消註冊對  事件的訂閱
        EventManager.StopListening<bool>(NameOfEvent.ChangeMoveMode, ChangeMoveMode);
    }

    private void ChangeMoveMode(bool _isScaledByTime)
    {
        Debug.Log($"PlayMove: Move mode is changed (is scaled by time: {_isScaledByTime})");
        isScaledByTime = _isScaledByTime;
    }
}
