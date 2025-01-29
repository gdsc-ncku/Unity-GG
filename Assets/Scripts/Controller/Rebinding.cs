using UnityEngine;
using UnityEngine.UI; // 引入 UI 命名空間
using UnityEngine.InputSystem;


public class Rebinding : MonoBehaviour
{
    public Button targetButton; // 要監聽的按鈕
    

    public InputActionAsset inputActionAsset; // InputActionAsset 資產

    public InputAction rebindingAction;
    void Start()
    {
        if (targetButton != null)
        {
            // 為按鈕添加 onClick 事件監聽
            
        }
        else
        {
            Debug.LogError("Target Button is not assigned in the Inspector.");
        }
        
        
        //如何從一個ActionAsset中得到特定動作

        rebindingAction = inputActionAsset.FindAction("jump", true); // 假設動作名稱是 "Jump"

        if (rebindingAction != null)
        {
            Debug.Log("Found action: " + rebindingAction.name);
        }
        else
        {
            Debug.LogError("Action 'Jump' not found in InputActionAsset.");
        }

        rebindingAction.Disable();

        rebindingAction.PerformInteractiveRebinding()
            .OnComplete(callback => {
                Debug.Log("Rebinding complete.");
                callback.Dispose();
                // 在這裡查看綁定的按鍵
                foreach (var binding in rebindingAction.bindings)
                {
                    Debug.Log($"Binding: {binding.path}");
                }

                // 重綁定完成後啟用 action
                rebindingAction.Enable();
            })
            .Start();

        //輸出rebindingAction目前綁定的按鍵名稱
        

        
    }

    

    

    public void Jump()
    {

    }
}
