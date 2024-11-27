using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

/// <summary>
/// ���I�����檬�A
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


    public GameObject flyingSickle; //���I�C������
    [Tooltip("���U�Ǥ�")] public Image targetHeart;

    //private GameObject playerCamera;   //���a��v���C������
    //private Transform keepPosition; //�����m��transform
    public float rotationSpeed = 100f; // ����t�סA���O��/��

    [SerializeField] private FlyingStatus status = FlyingStatus.prepare;    //��e���A

    [Header("����-�`�ױ���")]
    [SerializeField] private float depth = 0f;   //�ثe��w���`��
    public float scrollSpeed;
    public float minDepth = 10f;
    public float maxDepth = 100f;

    public float minTargetSize = 10f;
    public float maxTargetSize = 100f;

    private Queue<Vector3> lockPoint = new Queue<Vector3>();    //���a��w���I
    //public Transform debugBall;
    //public Transform debugBall1;

    [Header("����-��w�����I")]
    public float bulletTime = 0.7f;
    private Vector3 currentTarget;
    public float sickleSpeed = 5f;
    public bool hasTarget = false;

    [Header("��������")]
    private Rigidbody rb;
    //private Collider coll;
    private MeshCollider coll;

    [Header("��z����")]
    [SerializeField] private bool isLastFlyingBack = false;   //�����O�_���k��Ĳ�o����^�A�p�G�O �L�k��z

    private void Awake()
    {
        //����ƳЫ�
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

        //���I�l��
        if (hasTarget)
        {
            Tracking();
        }
        else if(status == FlyingStatus.back)
        {
            Backing();
        }
        else if(status == FlyingStatus.hold)
        {
            Holding();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            EnterHold();
        }
    }

    /// <summary>
    /// ���I����l��
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="camera"></param>
    public override void Init(Transform transform, Camera camera)
    {
        base.Init(transform, camera);

        //init
        //coll = flyingSickle.GetComponent<Collider>();
        coll = flyingSickle.GetComponent<MeshCollider>();
        coll.isTrigger = true;

        rb = flyingSickle.GetComponent<Rigidbody>();
        rb.useGravity = false;
        EnterHold();
    }

    ///// <summary>
    ///// ��l�ƪZ��
    ///// �����m�B���ਤ��
    ///// </summary>
    ///// <param name="takePosition">�n�������m</param>
    //public void InitWeapon(Transform takePosition, GameObject _player)
    //{
    //    //set value
    //    keepPosition = takePosition;
    //    playerCamera = _player.GetComponent<Camera>();

    //    //init
    //    //coll = flyingSickle.GetComponent<Collider>();
    //    coll = flyingSickle.GetComponent<MeshCollider>();
    //    coll.isTrigger = true;

    //    rb = flyingSickle.GetComponent<Rigidbody>();
    //    rb.useGravity = false;
    //    EnterHold();
    //}

    /// <summary>
    /// �k��Ĳ�o
    /// </summary>
    /// <param name="type"></param>
    public override void RightClickPerformed(InputAction.CallbackContext obj)
    {
        if (status != FlyingStatus.drop)
        {
            EnterBack(isLastFlyingBack = false);
        }
    }

    /// <summary>
    /// ����Ĳ�o
    /// </summary>
    /// <param name="type">0, 1, 2 ���O�O�I���B����B��}</param>
    public override void LeftClickCanceled(InputAction.CallbackContext obj)
    {
        ChoosePoint(true);
        LockPoint();
    }

    public override void LeftClickPerformed(InputAction.CallbackContext obj)
    {
        ChoosePoint(false);
    }

    /// <summary>
    /// ����u�ʰ���
    /// </summary>
    public void MiddleScroll()
    {
        // �ھڷƹ��u����s�`��
        float scroll = Input.mouseScrollDelta.y;
        depth = Mathf.Clamp(depth + scroll * scrollSpeed, minDepth, maxDepth);

        // ��s�Ǥߤj�p�ӤϬM�`�׭�
        float targetSize = Mathf.Lerp(maxTargetSize, minTargetSize, (depth - minDepth) / (maxDepth - minDepth));
        targetHeart.rectTransform.sizeDelta = new Vector2(targetSize, targetSize);
    }

    /// <summary>
    /// ���I���઺���
    /// </summary>
    private void SelfRotate()
    {
        if(status > FlyingStatus.hold && status < FlyingStatus.drop)
        {
            // �ϥ� transform.Rotate �����I����C�����@�w����
            flyingSickle.transform.Rotate(Vector3.left * rotationSpeed * Time.deltaTime);
            Debug.Log("rotate");
        }
    }

    /// <summary>
    /// ���I��b��W�����
    /// </summary>
    private void Holding()
    {
        flyingSickle.transform.position = keepPosition.position;
        flyingSickle.transform.eulerAngles = keepPosition.eulerAngles;
    }

    /// <summary>
    /// ���I�^�k�����
    /// </summary>
    private void Backing()
    {
        // ���I���ʨ�ؼ��I
        flyingSickle.transform.position = Vector3.MoveTowards(flyingSickle.transform.position, keepPosition.position, sickleSpeed * Time.deltaTime);

        // �ˬd���I�O�_��F�ؼ��I
        if (Vector3.Distance(flyingSickle.transform.position, keepPosition.position) < 0.1f)
        {
            lockPoint.Clear();

            //���m��z�������ܼ�
            isLastFlyingBack = false;   //��s��^Ĳ�o�ӷ������A

            // �i�J�U�Ӫ��A
            EnterHold();
        }
    }

    /// <summary>
    /// �l�ܪ��a�]�w���I
    /// �}�l����� ���w�M��queue
    /// </summary>
    private void Tracking()
    {
        // ���I���ʨ�ؼ��I
        flyingSickle.transform.position = Vector3.MoveTowards(flyingSickle.transform.position, currentTarget, sickleSpeed * Time.deltaTime);

        // �ˬd���I�O�_��F�ؼ��I
        if (Vector3.Distance(flyingSickle.transform.position, currentTarget) < 0.1f)
        {
            // ��e�ؼ��I�w��F�A�ˬd�O�_�٦��U�@���I
            if (lockPoint.Count > 0)
            {
                currentTarget = lockPoint.Dequeue();  // �]�w�U�@�ӥؼ��I
            }
            else
            {
                // �p�G�ؼ��I�]�m�S�����^���{�� ���N��Ӥ��γ]�m ���I����
                if (status != FlyingStatus.back)
                {
                    EnterDrop();
                }else if(status == FlyingStatus.back)
                {
                    //�̫�@�q����^�k
                    EnterBack(isLastFlyingBack=true);
                }
            }
        }
    }

    /// <summary>
    /// ��w��m
    /// �ھڷ�e���I���A�A�]�m��w��m
    /// </summary>
    private void LockPoint()
    {
        if((status == FlyingStatus.hold && lockPoint.Count == 0) 
            || (status > FlyingStatus.hold && status < FlyingStatus.back))
        {
            // �Ĥ@���o�g
            // �ھڷǤߤ�V�M�`�׭p����w�I
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

            // �i�J�U�Ӫ��A
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
    /// ��ܦ�m
    /// �p�G�n��ܡA�i�J�l�u�ɶ�
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
    /// �B�z����
    /// </summary>
    private void EnterDrop()
    {
        status = FlyingStatus.drop;
        hasTarget = false;

        if(lockPoint.Count > 0)
        {
            lockPoint.Clear();
        }

        if(rb.useGravity == false)
        {
            rb.useGravity = true;
            coll.isTrigger = false;
        }
    }

    /// <summary>
    /// �i�J����A
    /// </summary>
    private void EnterHold()
    {
        status = FlyingStatus.hold;
        hasTarget = false;

        if (lockPoint.Count > 0)
        {
            lockPoint.Clear();
        }

        //�B�z�I������
        if (rb.useGravity == true)
        {
            rb.useGravity = false;
            coll.isTrigger = true;
        }
    }

    private void EnterBack(bool isLastFlying)
    {
        hasTarget = false;
        status = FlyingStatus.back;

        if(isLastFlying == true)
        {
            isLastFlyingBack = true;
        }

        if (lockPoint.Count > 0)
        {
            lockPoint.Clear();
        }

        //�B�z�I������
        if (rb.useGravity == true)
        {
            rb.useGravity = false;
            coll.isTrigger = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //���^
            EnterHold();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enviroument" && isLastFlyingBack == false)
        {
            Debug.Log("touch object");

            //�I�����N����ᱼ��
            EnterDrop();
        }
    }
}
