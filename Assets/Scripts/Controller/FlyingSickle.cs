using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Timeline;
using UnityEngine.UI;

/// <summary>
/// 飛鐮的飛行狀態
/// </summary>
public enum FlyingSickle_Status
{
    prepare = -1,
    hold,
    fly_1,
    fly_2,
    back,
    drop
}

public class FlyingSickle : Weapon
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

    public float rotationSpeed = 100f; // 旋轉速度，單位是度/秒
    [SerializeField] private FlyingSickle_Status status = FlyingSickle_Status.prepare;    //當前狀態

    [Header("中鍵-深度控制")]
    [SerializeField] private float depth = 0f;   //目前鎖定的深度
    public float scrollSpeed;
    public float minDepth = 10f;
    public float maxDepth = 100f;

    public float minTargetSize = 10f;   //最小準心大小
    public float maxTargetSize = 100f;  //最大準心大小

    private Queue<Vector3> lockPoint = new Queue<Vector3>();    //玩家鎖定的點

    [Header("左鍵-鎖定飛行點")]
    public float bulletTime = 0.7f;
    private Vector3 currentTarget;
    public float sickleSpeed = 5f;
    public bool hasTarget = false;

    [Header("掉落相關")]
    private Rigidbody rb;
    private MeshCollider coll;
    public float dropSpeed = 5f;

    [Header("穿透相關")]
    [SerializeField] private bool isLastFlyingBack = false;   //紀錄是否為右鍵觸發的返回，如果是 無法穿透

    [Header("手持相關")]
    private bool isHolding = false;

    private void Awake()
    {
        //防止重複創建
        if(_flyingSickle != null)
        {
            Debug.Log("Duplicate creating Flying Sickle Instance");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        MiddleScroll();

        SelfRotate();

        //飛鐮追蹤
        if (hasTarget)
        {
            Tracking();
        }
        else if(status == FlyingSickle_Status.back)
        {
            Backing();
        }
        //else if(status == FlyingSickle_Status.drop && isGrounded == false)
        //{
        //    Droping();
        //}
    }

    /// <summary>
    /// 飛鐮的初始化
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="camera"></param>
    protected override void Init()
    {
        base.Init();

        //追蹤相關
        currentTarget = Vector3.zero;

        //掉落與碰撞相關
        coll = flyingSickle.GetComponent<MeshCollider>();
        coll.isTrigger = true;

        rb = flyingSickle.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        EnterStatus(FlyingSickle_Status.hold);
    }

    /// <summary>
    /// 右鍵觸發
    /// </summary>
    /// <param name="type"></param>
    protected override void RightClickPerformed(InputAction.CallbackContext obj)
    {
        //if (status != FlyingSickle_Status.drop)
        //{
        //    EnterBack(isLastFlyingBack = false);
        //}

        if(status == FlyingSickle_Status.fly_1 || status == FlyingSickle_Status.fly_2)
        {
            //EnterBack(isLastFlyingBack = false);
            EnterStatus(FlyingSickle_Status.back, false);
        }
    }

    /// <summary>
    /// 左鍵觸發
    /// </summary>
    /// <param name="type">0, 1, 2 分別是點擊、按住、放開</param>
    protected override void LeftClickCanceled(InputAction.CallbackContext obj)
    {
        EnterBulletTime(false);
        LockPoint();
    }

    protected override void LeftClickPerformed(InputAction.CallbackContext obj)
    {
        EnterBulletTime(true);
    }

    /// <summary>
    /// R 鍵觸發
    /// </summary>
    /// <param name="obj"></param>
    public override void RClickPerformed(InputAction.CallbackContext obj)
    {
        //EnterHold();
        EnterStatus(FlyingSickle_Status.hold);
    }

    /// <summary>
    /// 中鍵滾動偵測
    /// </summary>
    public void MiddleScroll()
    {
        // 根據滑鼠滾輪更新深度
        float scroll = Input.mouseScrollDelta.y;
        depth = Mathf.Clamp(depth + scroll * scrollSpeed, minDepth, maxDepth);

        // 更新準心大小來反映深度值
        float targetSize = Mathf.Lerp(maxTargetSize, minTargetSize, (depth - minDepth) / (maxDepth - minDepth));
        targetHeart.rectTransform.sizeDelta = new Vector2(targetSize, targetSize);
    }

    /// <summary>
    /// 飛鐮旋轉的函數
    /// </summary>
    private void SelfRotate()
    {
        if (status == FlyingSickle_Status.fly_1 || status == FlyingSickle_Status.fly_2
            || status == FlyingSickle_Status.back)
        {
            // 使用 transform.Rotate 讓飛鐮物件每秒旋轉一定角度
            flyingSickle.transform.Rotate(Vector3.left * rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// 飛鐮回歸的函數
    /// </summary>
    private void Backing()
    {
        // 飛鐮移動到目標點
        flyingSickle.transform.position = Vector3.MoveTowards(flyingSickle.transform.position, keepPosition.position, sickleSpeed * Time.deltaTime);

        // 檢查飛鐮是否到達目標點
        if (Vector3.Distance(flyingSickle.transform.position, keepPosition.position) < 0.1f)
        {
            lockPoint.Clear();

            //重置穿透相關的變數
            isLastFlyingBack = false;   //更新返回觸發來源的狀態

            // 進入下個狀態
            EnterStatus(FlyingSickle_Status.hold);
        }
    }

    /// <summary>
    /// 追蹤玩家設定的點
    /// 開始執行後 必定清空queue
    /// </summary>
    private void Tracking()
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
                if (status != FlyingSickle_Status.back)
                {
                    EnterStatus(FlyingSickle_Status.drop);
                }else if(status == FlyingSickle_Status.back)
                {
                    //最後一段飛行回歸
                    EnterStatus(FlyingSickle_Status.back, true);
                }
            }
        }
    }

    /// <summary>
    /// 鎖定位置
    /// 根據當前飛鐮狀態，設置鎖定位置
    /// </summary>
    private void LockPoint()
    {
        //飛鐮處在合法的操作狀態
        //手持 且 沒有其他鎖定點
        //飛行1
        //飛行2
        if((status == FlyingSickle_Status.hold && lockPoint.Count == 0) 
            || (status > FlyingSickle_Status.hold && status < FlyingSickle_Status.back))
        {
            // 根據準心方向和深度計算鎖定點
            Vector3 rayOrigin = playerCamera.transform.position;
            Vector3 rayDirection = playerCamera.transform.forward;
            
            Vector3 point = rayOrigin + rayDirection * depth;

            if (status == FlyingSickle_Status.hold)
            {
                //第一次直接設定鎖定點
                currentTarget = point;
            }
            else
            {
                //第二次以後 將鎖定點加入buffer
                lockPoint.Enqueue(point);
            }

            // 進入下個狀態
            status = (FlyingSickle_Status)((int)(status + 1) % ((int)FlyingSickle_Status.back + 1));
            
            //脫手
            if(isHolding == true)
            {
                isHolding = false;
                flyingSickle.transform.SetParent(null);
            }
            
            //告訴飛鐮有目標了
            hasTarget = true;
        }
        else
        {
            Debug.Log("FlyingSickle: Illegal control");
            return;
        }
    }

    /// <summary>
    /// 選擇位置
    /// 如果要選擇，進入子彈時間
    /// </summary>
    /// <param name="isTrigger"></param>
    private void EnterBulletTime(bool isTrigger)
    {
        if(isTrigger == true)
        {
            EventManager.TriggerEvent<float>(NameOfEvent.TimeControl, bulletTime);
        }
        else
        {
            EventManager.TriggerEvent(NameOfEvent.TimeResume);
        }
    }

     /// <summary>
     /// 進入指定狀態
     /// 需要做的處理
     /// </summary>
     /// <param name="_status"></param>
     /// <param name="isLastFlying"></param>
    private void EnterStatus(FlyingSickle_Status _status, bool isLastFlying)
    {
        status = _status;
        hasTarget = false;

        //清空鎖定點buffer
        if (lockPoint.Count > 0)
        {
            lockPoint.Clear();
        }

        if(status == FlyingSickle_Status.back)
        {
            if (isLastFlying == true)
            {
                isLastFlyingBack = true;
            }

            ChangeGravity(false);
        }
    }

    /// <summary>
    /// 進入指定狀態
    /// 需要做的處理
    /// </summary>
    /// <param name="_status"></param>
    private void EnterStatus(FlyingSickle_Status _status)
    {
        status = _status;
        hasTarget = false;

        //清空鎖定點buffer
        if (lockPoint.Count > 0)
        {
            lockPoint.Clear();
        }

        if (_status == FlyingSickle_Status.drop)
        {
            ChangeGravity(true);
        }
        else if (_status == FlyingSickle_Status.hold)
        {
            isHolding = true;

            //回到手的準確位置
            flyingSickle.transform.SetParent(keepPosition.parent.parent);
            flyingSickle.transform.position = keepPosition.position;
            flyingSickle.transform.rotation = keepPosition.rotation;

            ChangeGravity(false);
        }
    }

    /// <summary>
    /// 改變重力
    /// 有或沒有
    /// </summary>
    /// <param name="use">是否使用重力</param>
    private void ChangeGravity(bool use)
    {
        //處理碰撞相關
        if (use == false && rb.useGravity == true)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            coll.isTrigger = true;
        }else if(use == true && rb.useGravity == false)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //收回
            EnterStatus(FlyingSickle_Status.hold);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enviroument" && isLastFlyingBack == false && status != FlyingSickle_Status.hold)
        {
            Debug.Log("FlyingSickle: touch object");

            //碰撞任意物體後掉落
            EnterStatus(FlyingSickle_Status.drop);
        }
    }
}
