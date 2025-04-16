using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    const float LIFE_TIME = 2f;

    void Start()
    {
        Destroy(gameObject, LIFE_TIME);
    }
}
