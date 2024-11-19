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

    public float mouseSensitivity = 100f; // �ƹ��F�ӫ�
    private Transform playerTransform; // ���⪺Transform
    private float xRotation = 0f; // ���⪺��������

    private void Awake()
    {
        playerRigibody = player.GetComponent<Rigidbody>();
        playerTransform = player.transform;
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
        playerTransform.Rotate(Vector3.up * mouseX);
    }

    private void Movement()
    {
        Vector2 inputVector = PlayerManager.instance.playerControl.player.move.ReadValue<Vector2>();

        // ������a���e��M�k����V
        Vector3 forward = playerRigibody.transform.forward;
        Vector3 right = playerRigibody.transform.right;

        // �ھڿ�J�p�Ⲿ�ʤ�V
        Vector3 moveDirection = forward * inputVector.y + right * inputVector.x;

        if (PlayerManager.instance.playerStatus == PlayerStatus.move && inputVector != Vector2.zero && Vector3.Distance(playerRigibody.velocity, Vector3.zero) < maxSpeed)
        {
            playerRigibody.velocity = moveDirection.normalized * maxSpeed;
        }
        else if(PlayerManager.instance.playerStatus == PlayerStatus.move && inputVector == Vector2.zero)
        {
            playerRigibody.velocity = new Vector3(0, playerRigibody.velocity.y, 0);
        }
    }

    public IEnumerator Sprint(Vector3 forward, float sprintDistance, int sprintFrame)
    {
        PlayerManager.instance.playerStatus = PlayerStatus.sprint;
        PlayerManager.instance.rb.velocity = Vector3.zero;
        yield return new WaitForFixedUpdate();
        PlayerManager.instance.rb.AddForce(2 * PlayerManager.instance.rb.mass * sprintDistance / (Time.fixedDeltaTime * sprintFrame) * forward, ForceMode.Impulse);
        for (int i = 0; i < sprintFrame; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        PlayerManager.instance.rb.velocity = Vector3.zero;
        PlayerManager.instance.playerStatus = PlayerStatus.move;
        yield break;
    }
}
