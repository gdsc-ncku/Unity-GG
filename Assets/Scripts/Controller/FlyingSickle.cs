using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

/// <summary>
/// 飛鐮的飛行狀態
/// </summary>
public enum FlyingStatus
{
    prepare = -1,
    hold,
    fly_1,
    fly_2,
    back,
    drop
}

public class FlyingSickle : MonoBehaviour
{
    //instance mode
    private static FlyingSickle _flyingSickle;
    public static FlyingSickle Instance
    {
        get
        {
            if (!_flyingSickle)
            {
                _flyingSickle = FindObjectOfType(typeof(FlyingSickle)) as FlyingSickle;
                if (!_flyingSickle)
                {
                    Debug.LogError("No script");
                }
                else
                {
                    //init
                }
            }
            return _flyingSickle;
        }

    }


    public GameObject flyingSickle; //飛鐮遊戲物件
    [Tooltip("輔助準心")] public Image targetHeart;

    private GameObject playerCamera;   //玩家攝影機遊戲物件
    private Transform keepPosition; //手持位置的transform
    public float rotationSpeed = 100f; // 旋轉速度，單位是度/秒

    [SerializeField] private FlyingStatus status = FlyingStatus.prepare;    //當前狀態

    [Header("中鍵-深度控制")]
    [SerializeField] private float depth = 0f;   //目前鎖定的深度
    public float scrollSpeed;
    public float minDepth = 10f;
    public float maxDepth = 100f;

    public float minTargetSize = 10f;
    public float maxTargetSize = 100f;

    private Queue<Vector3> lockPoint = new Queue<Vector3>();    //玩家鎖定的點
    //public Transform debugBall;
    //public Transform debugBall1;

    [Header("左鍵-鎖定飛行點")]
    public float bulletTime = 0.7f;
    private Vector3 currentTarget;
    public float sickleSpeed = 5f;
    public bool hasTarget = false;

    [Header("掉落")]
    private Rigidbody rb = null;
    private Collider coll;

    // Update is called once per frame
    void FixedUpdate()
    {

        MiddleScroll();

        SelfRotate();

        //飛鐮追蹤
        if (hasTarget)
        {
            Track();
        }
        else if(status == FlyingStatus.back)
        {
            Back();
        }
        else if(status == FlyingStatus.hold)
        {
            Hold();
        }
        else if(status == FlyingStatus.drop)
        {
            Drop();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            status = FlyingStatus.hold;
        }
    }

    /// <summary>
    /// 初始化武器
    /// 手持位置、旋轉角度
    /// </summary>
    /// <param name="takePosition">要手持的位置</param>
    public void InitWeapon(Transform takePosition, GameObject _player)
    {
        //set value
        keepPosition = takePosition;
        playerCamera = _player;

        //init
        status = FlyingStatus.hold;
        coll = flyingSickle.GetComponent<Collider>();
    }

    public void RightClick()
    {
        if (status != FlyingStatus.drop)
        {
            hasTarget = false;
            status = FlyingStatus.back;
        }
    }

    /// <summary>
    /// 左鍵觸發
    /// </summary>
    /// <param name="type">0, 1, 2 分別是點擊、按住、放開</param>
    public void LeftClick(int type)
    {
        //按下
        if(type == 0)
        {
            ChoosePoint(false);
        }
        else if(type == 2)
        {
            //放開
            ChoosePoint(true);
            LockPoint();
        }
    }

    public void MiddleScroll()
    {
        // 根據滑鼠滾輪更新深度
        float scroll = Input.mouseScrollDelta.y;
        depth = Mathf.Clamp(depth + scroll * scrollSpeed, minDepth, maxDepth);

        // 更新準心大小來反映深度值
        float targetSize = Mathf.Lerp(maxTargetSize, minTargetSize, (depth - minDepth) / (maxDepth - minDepth));
        targetHeart.rectTransform.sizeDelta = new Vector2(targetSize, targetSize);
    }

    private void SelfRotate()
    {
        if(status > FlyingStatus.hold && status < FlyingStatus.drop)
        {
            // 使用 transform.Rotate 讓飛鐮物件每秒旋轉一定角度
            flyingSickle.transform.Rotate(Vector3.left * rotationSpeed * Time.deltaTime);
            Debug.Log("rotate");
        }
    }

    private void Hold()
    {
        if (rb != null)
        {
            Destroy(rb);
            coll.isTrigger = true;
        }
    
        flyingSickle.transform.position = keepPosition.position;
        flyingSickle.transform.eulerAngles = keepPosition.eulerAngles;
    }

    private void Back()
    {
        // 飛鐮移動到目標點
        flyingSickle.transform.position = Vector3.MoveTowards(flyingSickle.transform.position, keepPosition.position, sickleSpeed * Time.deltaTime);

        // 檢查飛鐮是否到達目標點
        if (Vector3.Distance(flyingSickle.transform.position, keepPosition.position) < 0.1f)
        {
            lockPoint.Clear();

            // 進入下個狀態
            status = FlyingStatus.hold;
        }
    }

    /// <summary>
    /// 追蹤玩家設定的點
    /// 開始執行後 必定清空queue
    /// </summary>
    private void Track()
    {
        // 飛鐮移動到目標點
        flyingSickle.transform.position = Vector3.MoveTowards(flyingSickle.transform.position, currentTarget, sickleSpeed * Time.deltaTime);

        // 檢查飛鐮是否到達目標點
        if (Vector3.Distance(flyingSickle.transform.position, currentTarget) < 0.1f)
        {
            // 當前目標點已到達，檢查是否還有下一個點
            if (lockPoint.Count > 0)
            {
                currentTarget = lockPoint.Dequeue();  // 設定下一個目標點
            }
            else
            {
                // 如果目標點設置沒有到返回的程度 那代表來不及設置 飛鐮掉落
                if (status != FlyingStatus.back)
                {
                    lockPoint.Clear();
                    status = FlyingStatus.drop;
                }

                hasTarget = false;
            }
        }
    }

    /// <summary>
    /// 鎖定位置
    /// 根據當前飛鐮狀態，設置鎖定位置
    /// </summary>
    private void LockPoint()
    {
        if((status == FlyingStatus.hold && lockPoint.Count == 0) 
            || (status > FlyingStatus.hold && status < FlyingStatus.back))
        {
            // 第一次發射
            // 根據準心方向和深度計算鎖定點
            Vector3 rayOrigin = playerCamera.transform.position;
            Vector3 rayDirection = playerCamera.transform.forward;
            
            Vector3 point = rayOrigin + rayDirection * depth;
            
            if(status == FlyingStatus.hold)
            {
                currentTarget = point;
            }
            else
            {
                lockPoint.Enqueue(point);
            }

            // 進入下個狀態
            status = (FlyingStatus)((int)(status + 1) % ((int)FlyingStatus.back + 1));
            hasTarget = true;
        }
        else
        {
            Debug.Log("illegal control");
            return;
        }
    }

    /// <summary>
    /// 選擇位置
    /// 如果要選擇，進入子彈時間
    /// </summary>
    /// <param name="isDown"></param>
    private void ChoosePoint(bool isDown)
    {
        if(isDown == false)
        {
            Time.timeScale = bulletTime;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    /// <summary>
    /// 處理掉落
    /// </summary>
    private void Drop()
    {
        if(rb == null)
        {
            rb = flyingSickle.AddComponent<Rigidbody>();

            coll.isTrigger = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //收回
            status = FlyingStatus.hold; 
        }
    }
}
