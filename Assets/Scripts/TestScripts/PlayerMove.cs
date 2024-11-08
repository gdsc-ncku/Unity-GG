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

    public float mouseSensitivity = 100f; // �ƹ��F�ӫ�
    private Transform playerTransform; // ���⪺Transform
    private float xRotation = 0f; // ���⪺��������

    private void Awake()
    {
        playerRigibody = player.GetComponent<Rigidbody>();
        playerTransform = player.transform;

        inputActions = new PlayerControl();
        inputActions.Enable();

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
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // ��������b-90��90�פ���

        // ��s��v��������
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // ��s���⪺����
        playerTransform.Rotate(Vector3.up * mouseX);
    }

    private void Movement()
    {
        Vector2 inputVector = inputActions.player.move.ReadValue<Vector2>();

        // ������a���e��M�k����V
        Vector3 forward = playerRigibody.transform.forward;
        Vector3 right = playerRigibody.transform.right;

        // �ھڿ�J�p�Ⲿ�ʤ�V
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
