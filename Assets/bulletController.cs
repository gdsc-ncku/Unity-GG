using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour
{
    public float speed = 20f;         // 子彈速度
    public float lifetime = 5f;       // 子彈最大存在時間
    public int damage = 10;           // 子彈傷害
    private Rigidbody rb;             // 子彈的剛體
    private float timeAlive = 0f;     // 記錄子彈已經存在的時間

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * speed;  // 根據子彈方向設置速度
        }

        // 開啟計時器，時間過長後銷毀子彈
        //Destroy(gameObject, lifetime);
    }

    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive >= lifetime)
        {
            Destroy(gameObject); // 當超過最大生命週期時，銷毀子彈
        }
    }

    // 碰撞檢測
    private void OnCollisionEnter(Collision collision)
    {
        
        Debug.Log("Bullet hit: " + collision.gameObject.name);
        //Destroy(gameObject); 
    }
}
