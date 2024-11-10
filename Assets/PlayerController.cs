using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    public float speed = 200f; // 控制速度
    public float maxSpeed = 150f; // 最大速度
    public float mouseSensitivity = 10f; // 鼠標靈敏度

    private float xRotation = 0f; // 垂直旋轉量限制

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // 處理移動邏輯
    public void Move(Vector2 inputVector)
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 moveDirection = forward * inputVector.y + right * inputVector.x;

        if (inputVector != Vector2.zero && Vector3.Distance(playerRigidbody.velocity, Vector3.zero) < maxSpeed)
        {
            playerRigidbody.AddForce(moveDirection.normalized * speed, ForceMode.Force);
        }
        else if (inputVector == Vector2.zero)
        {
            playerRigidbody.velocity = Vector3.zero;
        }
    }

    // 處理旋轉邏輯
    public void RotateView(Vector2 mouseDelta)
    {
        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    //衝刺一次性Addforce，在move中的檢查是超過上限的話移動不加速，不是把速度鎖回上限所以不影響，一段時間後自己鎖回上限
    public void Sprint(float speed)
    {
        Vector3 forward = transform.forward;
        playerRigidbody.AddForce(forward.normalized * speed, ForceMode.Force);
    }

    public void ResetSpeed()
    {
        if (playerRigidbody.velocity.magnitude > maxSpeed)
        {
            playerRigidbody.velocity = playerRigidbody.velocity.normalized * maxSpeed;
        }
    }
}
