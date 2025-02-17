using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITouchTester : MonoBehaviour
{
   
    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // 滑鼠左鍵點擊時檢測
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            Debug.Log("=== UI 點擊偵測結果 ===");
            foreach (var result in results)
            {
                Debug.Log($"Hit: {result.gameObject.name}");
            }

            if (results.Count == 0)
            {
                Debug.Log("⚠ 沒有 UI 物件被偵測到，可能是其他東西擋住了 UI");
            }
        }
    }
}
