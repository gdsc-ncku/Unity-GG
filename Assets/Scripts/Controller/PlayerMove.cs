using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float speed = 20f;
    public float maxSpeed = 15;
    private Rigidbody playerRigibody;
    public float mouseSensitivity = 100f; // 滑鼠靈敏度
    private float xRotation = 0f; // 角色的垂直旋轉

    private void Awake()
    {
        playerRigibody = GetComponent<Rigidbody>();
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
            playerRigibody.velocity = moveDirection.normalized * maxSpeed;
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
}
