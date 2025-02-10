using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    //public float speed = 20f;
    private float mouseSensitivity;
    private float maxSpeed;
    private Rigidbody playerRigibody;

    public GameObject ItemUI;
    public GameObject WeaponUI;
    public GameObject CollectionUI;
    public GameObject BackMask;
    // �ƹ��F�ӫ�
    private float xRotation; // ���⪺��������

    private void Awake()
    {
        playerRigibody = PlayerManager.Instance.rb;
        Cursor.lockState = CursorLockMode.Locked; // ��w�ƹ�

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

        // �ھڿ�J�p�Ⲿ�ʤ�V
        Vector3 moveDirection = forward * inputVector.y + right * inputVector.x;

        if (PlayerManager.Instance.playerStatus == PlayerStatus.move && inputVector != Vector2.zero && Vector3.Distance(playerRigibody.linearVelocity, Vector3.zero) < maxSpeed)
        {
            playerRigibody.linearVelocity = moveDirection.normalized * maxSpeed;
        }
        else if(PlayerManager.Instance.playerStatus == PlayerStatus.move && inputVector == Vector2.zero)
        {
            playerRigibody.linearVelocity = new Vector3(0, playerRigibody.linearVelocity.y, 0);
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
}
