using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody sphereRigidbody;
    public float moveForce = 1f;
    public float rotationSpeed = 100f; // 控制旋轉速度

    private void Awake() 
    {
        sphereRigidbody = GetComponent<Rigidbody>();
    }

    public void Sprint()
    {
        Debug.Log("Sprint!");
        sphereRigidbody.AddForce(transform.forward * moveForce * 3f, ForceMode.Impulse);
    }

    public void Move_a()
    {
        
        
        Debug.Log("a");
        // 向左旋轉
        transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
    }

    public void Move_s()
    {
        Debug.Log("s");
        // 向後移動
        sphereRigidbody.AddForce(-transform.forward * moveForce, ForceMode.Impulse);
    }

    public void Move_d()
    {
        Debug.Log("d");
        // 向右旋轉
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    public void Move_w()
    {
        Debug.Log("w");
        // 向前移動
        sphereRigidbody.AddForce(transform.forward * moveForce, ForceMode.Impulse);
    }
}
