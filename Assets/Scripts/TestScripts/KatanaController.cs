using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class KatanaController : Weapon
{
    [SerializeField] float specialAttackDistance, specialAttackDetectSphere, detectDistance, sprintDistance, knockUpHeight;
    [SerializeField] LayerMask enemyLayer;
    private HashSet<GameObject> hitEnemy = new HashSet<GameObject>();
    void Start()
    {
        StartCoroutine(debug());
        //Debug
        PlayerMove.Instance.inputActions.player.rightclick.performed += RightClick;
    }
    private void Update()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo, specialAttackDistance, enemyLayer))
        {
            Debug.Log(hitInfo.collider.name);
        }

    }
    public override void RightClick(InputAction.CallbackContext callback)
    {
        Vector3 origin = Camera.main.transform.position, forward = Camera.main.transform.forward;
        if (Physics.Raycast(origin, forward, out RaycastHit hitInfo, specialAttackDistance, enemyLayer))
        {
            Debug.Log("Sprint Attack");
            hitEnemy.Add(hitInfo.collider.gameObject);
            //讓forward必定是平行線
            forward = new Vector3(forward.x, 0, forward.z);
            Vector3 position = hitInfo.point + forward * detectDistance;
            StartCoroutine(Teleport(position, forward));
        }
        else
        {
            Debug.Log("Sprint");
            forward = new Vector3(forward.x, 0, forward.z);
            StartCoroutine(PlayerManager.instance.Sprint(forward, sprintDistance));
        }
    }

    private IEnumerator debug()
    {
        yield return new WaitForSeconds(1);
    }

    //使用coroutine來執行武士刀右鍵邏輯
    //因為要連續偵測，所以用此方式避免卡頓
    private IEnumerator Teleport(Vector3 position, Vector3 forward)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, specialAttackDetectSphere, enemyLayer);
        while(hitColliders.Length > 0)
        {
            position += forward * detectDistance;
            for(int i = 0; i < hitColliders.Length; i++)
            {
                hitEnemy.Add(hitColliders[i].gameObject);
            }
            yield return null;
            hitColliders = Physics.OverlapSphere(position, specialAttackDetectSphere, enemyLayer);
        }

        PlayerManager.instance.gameObject.GetComponent<Collider>().isTrigger = true;
        PlayerManager.instance.playerStatus = PlayerStatus.sprint;
        forward = new Vector3(position.x, PlayerManager.instance.rb.transform.position.y, position.z) - PlayerManager.instance.rb.transform.position;
        PlayerManager.instance.rb.AddForce(PlayerManager.instance.rb.mass * forward / 0.1f, ForceMode.Impulse);
        foreach (GameObject @object in hitEnemy)
        {
            Rigidbody @rb = @object.GetComponent<Rigidbody>();
            // 計算擊飛高度所需的初始速度
            float v0 = Mathf.Sqrt((float)(2 * 9.81 * knockUpHeight));
            // 使用 AddForce 將速度轉換為一個向上的力
            @rb.AddForce(Vector3.up * @rb.mass * v0 * 2, ForceMode.Impulse);
            yield return new WaitForSeconds(0.1f / hitEnemy.Count);
        }
        hitEnemy.Clear();
        PlayerManager.instance.gameObject.GetComponent<Collider>().isTrigger = false;
        PlayerManager.instance.playerStatus = PlayerStatus.move;
        yield break;
    }
}
