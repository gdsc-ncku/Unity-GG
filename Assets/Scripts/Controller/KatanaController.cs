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

    public override void RightClickPerformed(InputAction.CallbackContext obj)
    {
        Vector3 origin = Camera.main.transform.position, forward = Camera.main.transform.forward;
        if (Physics.Raycast(origin, forward, out RaycastHit hitInfo, specialAttackDistance, enemyLayer))
        {
            hitEnemy.Add(hitInfo.collider.gameObject);
            //讓forward必定是平行線
            forward = new Vector3(forward.x, 0, forward.z);
            Vector3 position = hitInfo.point + forward * detectDistance;
            StartCoroutine(Teleport(position, forward));
        }
        else
        {
            forward = new Vector3(forward.x, 0, forward.z);
            PlayerManager.Instance.Sprint(forward, sprintDistance);
        }
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

        PlayerManager.Instance.gameObject.GetComponent<Collider>().isTrigger = true;
        PlayerManager.Instance.playerStatus = PlayerStatus.sprint;
        forward = new Vector3(position.x, PlayerManager.Instance.rb.transform.position.y, position.z) - PlayerManager.Instance.rb.transform.position;
        PlayerManager.Instance.rb.AddForce(PlayerManager.Instance.rb.mass * forward / 0.1f, ForceMode.Impulse);
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
        PlayerManager.Instance.gameObject.GetComponent<Collider>().isTrigger = false;
        PlayerManager.Instance.playerStatus = PlayerStatus.move;
        yield break;
    }
}
