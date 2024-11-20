using UnityEngine;
using UnityEngine.InputSystem;

public class Revolver : RangedWeapon
{
    void Update()
    {
        if (Input.GetMouseButton(1)) // 右鍵按下
        {
            Aim();
        }
        if (Input.GetMouseButtonUp(1))
        {
            StartCoroutine(CancelAim());
        }
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }
    public override void LeftClickStarted(InputAction.CallbackContext obj)
    {
        Fire();
    }
    public override void RightClickPerformed(InputAction.CallbackContext obj)
    {
        Aim();
    }
    public override void RightClickCanceled(InputAction.CallbackContext obj)
    {
        StartCoroutine(CancelAim());
    }
}
