using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveManager : MonoBehaviour
{
    private static PlayerMoveManager _playerMoveManager;
    public static PlayerMoveManager Instance
    {
        get
        {
            if (!_playerMoveManager)
            {
                _playerMoveManager = FindObjectOfType<PlayerMoveManager>();
                if (!_playerMoveManager)
                {
                    Debug.LogError("No script");
                }
            }
            return _playerMoveManager;
        }
    }

    public GameObject player;
    private PlayerController playerController;

    public PlayerControl inputActions;
    
    public float sprintForce = 100f; // Sprint 的瞬時加速力量
    public float sprintDuration = 1f; // Sprint 的持續時間
    private bool isSprinting = false;
    private float sprintEndTime = 0f;

    private void Awake()
    {
        playerController = player.GetComponent<PlayerController>();

        inputActions = new PlayerControl();
        inputActions.Enable();

        Cursor.lockState = CursorLockMode.Locked; // 鎖定鼠標

        // 設定空白鍵的按鍵事件
        //inputActions.player.Sprint.performed += ctx => StartSprint();
    }

    private void FixedUpdate()
    {
        Movement();
        ViewportFocus();
        
        // 如果已經在 Sprint 且時間超過，則結束 Sprint 狀態
        if (isSprinting && Time.time >= sprintEndTime)
        {
            StopSprint();
        }
    }

    private void Movement()
    {
        Vector2 inputVector = inputActions.player.move.ReadValue<Vector2>();
        playerController.Move(inputVector); // 呼叫 PlayerController 的移動方法
    }

    private void ViewportFocus()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        playerController.RotateView(mouseDelta); // 呼叫 PlayerController 的旋轉方法
    }

    // 開始 Sprint
    private void StartSprint()
    {
        if (!isSprinting)
        {
            isSprinting = true;
            sprintEndTime = Time.time + sprintDuration; // 設定 Sprint 結束時間
            playerController.Sprint(sprintForce); // 添加一次性加速力
        }
    }

    // 停止 Sprint
    private void StopSprint()
    {
        isSprinting = false;
        playerController.ResetSpeed(); 
    }
}
