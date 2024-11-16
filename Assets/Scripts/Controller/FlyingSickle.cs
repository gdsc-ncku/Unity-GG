using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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


    public GameObject flyingSickle; //���I�C������
    [Tooltip("���U�Ǥ�")] public Image targetHeart;

    private GameObject playerCamera;   //���a��v���C������
    private Transform keepPosition; //�����m��transform
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

    [Header("����")]
    private Rigidbody rb = null;
    private Collider coll;

    // Update is called once per frame
    void FixedUpdate()
    {

        MiddleScroll();

        SelfRotate();

        //���I�l��
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
    /// ��l�ƪZ��
    /// �����m�B���ਤ��
    /// </summary>
    /// <param name="takePosition">�n�������m</param>
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
    /// ����Ĳ�o
    /// </summary>
    /// <param name="type">0, 1, 2 ���O�O�I���B����B��}</param>
    public void LeftClick(int type)
    {
        //���U
        if(type == 0)
        {
            ChoosePoint(false);
        }
        else if(type == 2)
        {
            //��}
            ChoosePoint(true);
            LockPoint();
        }
    }

    public void MiddleScroll()
    {
        // �ھڷƹ��u����s�`��
        float scroll = Input.mouseScrollDelta.y;
        depth = Mathf.Clamp(depth + scroll * scrollSpeed, minDepth, maxDepth);

        // ��s�Ǥߤj�p�ӤϬM�`�׭�
        float targetSize = Mathf.Lerp(maxTargetSize, minTargetSize, (depth - minDepth) / (maxDepth - minDepth));
        targetHeart.rectTransform.sizeDelta = new Vector2(targetSize, targetSize);
    }

    private void SelfRotate()
    {
        if(status > FlyingStatus.hold && status < FlyingStatus.drop)
        {
            // �ϥ� transform.Rotate �����I����C�����@�w����
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
        // ���I���ʨ�ؼ��I
        flyingSickle.transform.position = Vector3.MoveTowards(flyingSickle.transform.position, keepPosition.position, sickleSpeed * Time.deltaTime);

        // �ˬd���I�O�_��F�ؼ��I
        if (Vector3.Distance(flyingSickle.transform.position, keepPosition.position) < 0.1f)
        {
            lockPoint.Clear();

            // �i�J�U�Ӫ��A
            status = FlyingStatus.hold;
        }
    }

    /// <summary>
    /// �l�ܪ��a�]�w���I
    /// �}�l����� ���w�M��queue
    /// </summary>
    private void Track()
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
                    lockPoint.Clear();
                    status = FlyingStatus.drop;
                }

                hasTarget = false;
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
            //���^
            status = FlyingStatus.hold; 
        }
    }
}
