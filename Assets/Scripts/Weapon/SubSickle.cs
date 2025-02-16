using Unity.VisualScripting;
using UnityEngine;

public class SubSickle : MonoBehaviour
{
    public FlyingSickle flyingSickle;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //收回
            flyingSickle.EnterStatus(FlyingSickle_Status.hold);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enviroument" && flyingSickle.isLastFlyingBack == false && flyingSickle.status != FlyingSickle_Status.hold)
        {
            Debug.Log("FlyingSickle: touch object");

            //碰撞任意物體後掉落
            flyingSickle.EnterStatus(FlyingSickle_Status.drop);
        }
    }
}
