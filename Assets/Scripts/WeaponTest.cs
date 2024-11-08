using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponTest : MonoBehaviour
{
    public Transform keepPosition;
    public GameObject playerCamera;
    public PlayerControl inputActions;

    // Start is called before the first frame update
    void Start()
    {
        FlyingSickle.Instance.InitWeapon(keepPosition, playerCamera);
        inputActions = PlayerMove.Instance.inputActions;

        inputActions.player.leftclick.performed += LeftClick;
        inputActions.player.leftclick.canceled += Leftclick_canceled;

        inputActions.player.rightclick.performed += RightClick;
    }

    private void Leftclick_canceled(InputAction.CallbackContext obj)
    {
        FlyingSickle.Instance.LeftClick(2);
    }

    private void LeftClick(InputAction.CallbackContext context)
    {
        FlyingSickle.Instance.LeftClick(0);
    }

    private void RightClick(InputAction.CallbackContext context)
    {
        FlyingSickle.Instance.RightClick();
    }
}
