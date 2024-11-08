using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    //instance mode
    private static PlayerMove _playerMove;
    public static PlayerMove Instance
    {
        get
        {
            if (!_playerMove)
            {
                _playerMove = FindObjectOfType(typeof(PlayerMove)) as PlayerMove;
                if (!_playerMove)
                {
                    Debug.LogError("No script");
                }
                else
                {
                    //init
                }
            }
            return _playerMove;
        }

    }

    public float speed = 20f;
    public float maxSpeed = 15;
    public GameObject player;

    private Rigidbody playerRigibody;
    public PlayerControl inputActions;

    public float mouseSensitivity = 100f; // 滑鼠靈敏度
    private Transform playerTransform; // 角色的Transform
    private float xRotation = 0f; // 角色的垂直旋轉

    private void Awake()
    {
        playerRigibody = player.GetComponent<Rigidbody>();
        playerTransform = player.transform;

        inputActions = new PlayerControl();
        inputActions.Enable();

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
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 限制視角在-90到90度之間

        // 更新攝影機的旋轉
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // 更新角色的旋轉
        playerTransform.Rotate(Vector3.up * mouseX);
    }

    private void Movement()
    {
        Vector2 inputVector = inputActions.player.move.ReadValue<Vector2>();

        // 獲取玩家的前方和右側方向
        Vector3 forward = playerRigibody.transform.forward;
        Vector3 right = playerRigibody.transform.right;

        // 根據輸入計算移動方向
        Vector3 moveDirection = forward * inputVector.y + right * inputVector.x;

        if (inputVector != Vector2.zero && Vector3.Distance(playerRigibody.velocity, Vector3.zero) < maxSpeed)
        {
            playerRigibody.AddForce(moveDirection.normalized * speed, ForceMode.Force);
        }
        else if(inputVector == Vector2.zero)
        {
            playerRigibody.velocity = Vector3.zero;
        }
    }
}
