using UnityEngine;
using UnityEngine.AI;

public class LookAround : EnemyState
{
    public bool IsFinished => elapsedTime >= duration;
    NavMeshAgent navMeshAgent;
    float rotationSpeed = 100f; // 旋轉速度
    float lookAngle = 45f;      // 最大擺動角度
    float duration = 5f;        // 擺頭時間
    float elapsedTime = 0f;
    Quaternion originalRotation;
    public LookAround(EnemyBase enemy) : base(enemy)
    {
        navMeshAgent = enemy.GetComponent<NavMeshAgent>();
    }
    public override void OnStateEnter()
    {
        navMeshAgent.isStopped = true; // 停止 NavMeshAgent 移動
        elapsedTime = 0f;
        originalRotation = enemy.transform.rotation;
    }

    public override void OnStateUpdate()
    {
        elapsedTime += Time.deltaTime;

        // 計算擺頭角度（使用 Mathf.Sin 讓左右擺動平滑）
        float angleOffset = Mathf.Sin(elapsedTime * rotationSpeed * Mathf.Deg2Rad) * lookAngle;
        Quaternion targetRotation = Quaternion.Euler(0, originalRotation.eulerAngles.y + angleOffset, 0);

        // 平滑旋轉
        enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    public override void OnStateExit()
    {
        navMeshAgent.isStopped = false; // 恢復 NavMeshAgent
    }
}