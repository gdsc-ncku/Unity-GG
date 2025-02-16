using UnityEngine;

public class Shotgun : RangedWeapon
{
    //instance mode
    private static Shotgun _shotgun;
    public static Shotgun Instance
    {
        get
        {
            if (!_shotgun)
            {
                _shotgun = FindAnyObjectByType(typeof(Shotgun)) as Shotgun;
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

    //子彈擴散的角度 
    [SerializeField] float angle = 0.2f;
    
    //發射一次有幾顆小子彈
    [SerializeField] int num_of_bullets = 5; 


    void Awake()
    {
        if (_shotgun != null)
        {
            Destroy(gameObject);
        }
    }

    protected override void Fire(Vector3 direction)
    {
        base.Fire(direction);
        //Random rand = new Random;
        for (int i=0;i<num_of_bullets;i++){
            GameObject temp = Instantiate(ammoPrefab, firePoint.position, Quaternion.identity);
            Vector3 a =new Vector3(firePoint.transform.eulerAngles.x + Random.Range(-angle,angle), firePoint.transform.eulerAngles.y + Random.Range(-angle,angle), firePoint.transform.eulerAngles.z + Random.Range(-angle,angle));
            temp.transform.eulerAngles = a;
            temp.GetComponent<Rigidbody>().linearVelocity = temp.transform.forward * ammoSpeed;
        }
    }
}
