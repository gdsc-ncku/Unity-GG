using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KatanaController : Weapon
{
    [SerializeField] float specialAttackDistance, specialAttackDetectSphere, detectDistance, sprintDistance, knockUpHeight;
    private HashSet<GameObject> hitEnemy = new HashSet<GameObject>();

    protected override void RightClickPerformed(InputAction.CallbackContext obj)
    {
        Vector3 origin = Camera.main.transform.position, forward = Camera.main.transform.forward;
        if (Physics.Raycast(origin, forward, out RaycastHit hitInfo, specialAttackDistance, LayerMask.GetMask(LayerTagPack.Enemy)))
        {
            hitEnemy.Add(hitInfo.collider.gameObject);
            //��forward���w�O����u
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

    //�ϥ�coroutine�Ӱ���Z�h�M�k���޿�
    //�]���n�s�򰻴��A�ҥH�Φ��覡�קK�d�y
    private IEnumerator Teleport(Vector3 position, Vector3 forward)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, specialAttackDetectSphere, LayerMask.GetMask(LayerTagPack.Enemy));
        while(hitColliders.Length > 0)
        {
            position += forward * detectDistance;
            for(int i = 0; i < hitColliders.Length; i++)
            {
                hitEnemy.Add(hitColliders[i].gameObject);
            }
            yield return null;
            hitColliders = Physics.OverlapSphere(position, specialAttackDetectSphere, LayerMask.GetMask(LayerTagPack.Enemy));
        }

        PlayerManager.Instance.gameObject.GetComponent<Collider>().isTrigger = true;
        PlayerManager.Instance.playerStatus = PlayerStatus.sprint;
        forward = new Vector3(position.x, PlayerManager.Instance.rb.transform.position.y, position.z) - PlayerManager.Instance.rb.transform.position;
        PlayerManager.Instance.rb.AddForce(PlayerManager.Instance.rb.mass * forward / 0.1f, ForceMode.Impulse);
        foreach (GameObject @object in hitEnemy)
        {
            Rigidbody @rb = @object.GetComponent<Rigidbody>();
            // �p���������שһݪ���l�t��
            float v0 = Mathf.Sqrt((float)(2 * 9.81 * knockUpHeight));
            // �ϥ� AddForce �N�t���ഫ���@�ӦV�W���O
            @rb.AddForce(Vector3.up * @rb.mass * v0 * 2, ForceMode.Impulse);
            yield return new WaitForSeconds(0.1f / hitEnemy.Count);
        }
        hitEnemy.Clear();
        PlayerManager.Instance.gameObject.GetComponent<Collider>().isTrigger = false;
        PlayerManager.Instance.playerStatus = PlayerStatus.move;
        yield break;
    }
}
