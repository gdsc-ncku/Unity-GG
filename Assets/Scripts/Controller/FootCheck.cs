using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootCheck : MonoBehaviour
{
    public Transform playerTransform; // 在 Inspector 中拖入 Player 的 Transform

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            // 把自己的位置改成 Player 的位置，並添加偏移
            transform.position = playerTransform.position + new Vector3(0, -1, 0);
        }
        else
        {
            Debug.LogWarning("Player Transform is not assigned.");
        }
        //PlayerManager.Instance.playerStatus = PlayerStatus.move; 
    }

    // 碰撞檢測
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != playerTransform)
        {
            Debug.Log($"Triggered with {other.gameObject.name}");
            PlayerManager.Instance.playerStatus = PlayerStatus.move;
        }
    }

}
