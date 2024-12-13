using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float speed = 20f;
    public float maxSpeed = 15;
    private Rigidbody playerRigibody;
    public float mouseSensitivity = 100f; // �ƹ��F�ӫ�
    private float xRotation = 0f; // ���⪺��������

    private void Awake()
    {
        playerRigibody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // ��w�ƹ�
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

        // �ھڿ�J�p�Ⲿ�ʤ�V
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
