using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public enum Status
{
    prepare = -1,
    hold = 1,
    reload = 2,
}

public class Shotgun : MonoBehaviour
{
    //instance mode
    private static Shotgun _shotgun;
    public static Shotgun Instance
    {
        get
        {
            if (!_shotgun)
            {
                _shotgun = FindObjectOfType(typeof(Shotgun)) as Shotgun;
                if (!_shotgun)
                {
                    Debug.LogError("No script");
                }
                else
                {
                    //init
                }
            }
            return _shotgun;
        }

    }

    public GameObject shotgun; 
    public GameObject bullet;
    public Transform shootPoint; 
    public int ammo=3;
    public float timer = 0f; 
    public float cd = 1f;
    public int maxAmmo=3;
    [Tooltip("���U�Ǥ�")] public Image targetHeart;
    private GameObject playerCamera;   
    public Transform keepPosition; 
    [SerializeField] private Status status = Status.hold;    


    void Awake()
    {
        if (_shotgun != null && _shotgun != this)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if(timer<cd)
        {
            timer+=Time.deltaTime;
        }
        if(status == Status.hold)
        {
            Hold();
        }
        else if(status == Status.reload)
        {
            //Debug.Log("Reload");
            Hold();
        }
        if (Input.GetKeyDown(KeyCode.R) && status != Status.reload)
        {
            StartCoroutine(Reload(2f)); //sec
            Debug.Log("Reload");
        }
        if (Input.GetMouseButtonDown(0) && status != Status.reload && timer >= cd) 
        {
            timer = 0;
            LeftClick();
            Debug.Log("Fire" + ammo.ToString());
        }
        
    }

    private void Update()
    {
        
    }

    
    public void InitWeapon(Transform takePosition, GameObject _player)
    {
        //set value
        keepPosition = takePosition;
        playerCamera = _player;
        ammo = maxAmmo;

        //init
        status = Status.hold;
    }
/// <summary>
/// 
/// </summary>
    public void RightClick()
    {
        Debug.Log("Shotgun have no ability on RightClick!");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    public void LeftClick()
    {
        if(ammo>0)
        {
            ammo--;
            Fire();
        }   
        else
        {
            EmptyAmmo();
        }
    }

    private void Fire()
    {
        Debug.Log("Fire!");
        
        //Random rand = new Random;
        for(int i=0;i<5;i++){
            GameObject temp = Instantiate(bullet, shootPoint.position, Quaternion.identity);
            Vector3 a =new Vector3(shootPoint.transform.eulerAngles.x + Random.Range(-12,12),shootPoint.transform.eulerAngles.y + Random.Range(-12,12),shootPoint.transform.eulerAngles.z + Random.Range(-12,12));
            temp.transform.eulerAngles = a;
        }
        
        
    }

    private void EmptyAmmo()
    {
        Debug.Log("Out of Ammo!");
    }
    

    private void Hold()
    {
        shotgun.transform.position = keepPosition.position;
        shotgun.transform.eulerAngles = keepPosition.eulerAngles;
    }

    private IEnumerator Reload(float reloadTime)
    {
        status = Status.reload; // 狀態設為重裝
        Debug.Log("Starting reload...");

        yield return new WaitForSeconds(reloadTime); // 等待重裝時間

        ammo = maxAmmo; // 恢復彈藥數量
        status = Status.hold; // 恢復狀態為持有
        Debug.Log("Reload complete!");
    }
}
