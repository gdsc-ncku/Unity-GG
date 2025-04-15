using UnityEngine;
using UnityEngine.UI;

public class BulletUI_Selecting : MonoBehaviour
{
    //public GameObject BulletUIPrefab;
    public GameObject BulletUI;
    public Transform up_quar;
    public Transform down_quar;
    public Transform left_quar;
    public Transform right_quar;
    public string big_quar_name = "none";
    public Sprite select_sprite;
    public Sprite dark_sprite;
    public Transform BulletUI_trans;
    public Transform wheel;
    void Start()
    {
        Transform BulletUI_trans = BulletUI.transform;
        Transform wheel = BulletUI_trans.Find("Item_wheel");
    }
    
    
    public bool isselecting = false;
    public bool startselect = false;
    public bool expanding = false;
    
    int GetAngle()
    {   
            
        Vector3 mousePosition = Input.mousePosition;

        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        Vector3 direction = mousePosition - screenCenter;

        float ang = Mathf.Atan2(-direction.y, direction.x) * Mathf.Rad2Deg; 
        ang += 90;

        if (ang < 0)
        {
            ang += 360;
        }

        return Mathf.RoundToInt(ang);
    }

    void Resetquar()
    {
        Transform BulletUI_trans = BulletUI.transform;
        Transform wheel = BulletUI_trans.Find("Item_wheel");

        string[] list = { "up_quar", "right_quar", "down_quar", "left_quar"};
        for (int i = 0;i<list.Length;i++)
        {
            Transform target_trans = wheel.Find(list[i]);
            Vector3 scale = target_trans.localScale; 
            scale.x = 1.0f; 
            scale.y = 1.0f;
            target_trans.localScale = scale; 
            //將target_trans的sprite換成指定的
            Image sr = target_trans.GetComponent<Image>();
            if (sr != null)
            {
                sr.sprite = dark_sprite;
            }
        }
    }

    void Setquar(int angle)
    {
        
        Transform BulletUI_trans = BulletUI.transform;
        Transform wheel = BulletUI_trans.Find("Item_wheel");

        up_quar = wheel.Find("up_quar");
        down_quar = wheel.Find("down_quar");
        left_quar = wheel.Find("left_quar");
        right_quar = wheel.Find("right_quar");
        
        
        if((angle>=315 && angle<=360) || (angle>=0 && angle<45))
        {
            if(big_quar_name!="up_quar")
            {
                Resetquar();
            }
            big_quar_name = "up_quar";
            Transform target_trans = up_quar;
            //Debug.Log(target_trans);
            Vector3 scale = target_trans.localScale; 
            scale.x = 1.2f; 
            scale.y = 1.2f;
            target_trans.localScale = scale; 
            Image sr = target_trans.GetComponent<Image>();
            sr.sprite = select_sprite;
        }
        else if(angle>=45 && angle<135)
        {
            if(big_quar_name!="right_quar")
            {
                Resetquar();
            }
            big_quar_name = "right_quar";
            Transform target_trans = right_quar;
            //Debug.Log(target_trans);
            Vector3 scale = target_trans.localScale; 
            scale.x = 1.2f; 
            scale.y = 1.2f;
            target_trans.localScale = scale; 
            Image sr = target_trans.GetComponent<Image>();
            sr.sprite = select_sprite;
        }
        else if(angle>=135 && angle<225)
        {
            if(big_quar_name!="down_quar")
            {
                Resetquar();
            }
            big_quar_name = "down_quar";
            Transform target_trans = down_quar;
            //Debug.Log(target_trans);
            Vector3 scale = target_trans.localScale; 
            scale.x = 1.2f; 
            scale.y = 1.2f;
            target_trans.localScale = scale; 
            Image sr = target_trans.GetComponent<Image>();
            sr.sprite = select_sprite;
        }
        else if(angle>=225 && angle<315)
        {
            if(big_quar_name!="left_quar")
            {
                Resetquar();
            }
            big_quar_name = "left_quar";
            Transform target_trans = left_quar;
            //Debug.Log(target_trans);
            Vector3 scale = target_trans.localScale; 
            scale.x = 1.2f; 
            scale.y = 1.2f;
            target_trans.localScale = scale; 
            Image sr = target_trans.GetComponent<Image>();
            sr.sprite = select_sprite;
        }
    }
    void Expand(Image radialImage)
    {
        float fillSpeed = 1.0f;
        Debug.Log("test4");
        if (radialImage.fillAmount < 1.0f)
        {
            radialImage.fillAmount += fillSpeed * Time.deltaTime;

            
            if (radialImage.fillAmount > 1.0f)
            {
                radialImage.fillAmount = 1.0f;
                expanding = false;
                Debug.Log("fin");
            }
        }
    }
    
    void Update()
    {
        if(startselect == true)
        {
            //Resetquar();
            Setquar(GetAngle());
            isselecting = true;
            expanding = false;
        }
        if(startselect == false && isselecting == true)
        {
            //big_quar_name = "none";
            isselecting = false;
            expanding = true;
            //Debug.Log("test2");
        }
        if(expanding == true)
        {
            /*
            Transform BulletUI_trans = BulletUI.transform;
            Transform wheel = BulletUI_trans.Find("Item_wheel");
            string[] list = { "up_quar", "right_quar", "down_quar", "left_quar"};
            
            Transform target_trans;
            target_trans = wheel.Find(big_quar_name);*/
            //Debug.Log("test3");
            Transform BulletUI_trans = BulletUI.transform;
            Transform wheel = BulletUI_trans.Find("Item_wheel");
            Transform target_trans;
            target_trans = wheel.Find(big_quar_name);
            Image sr = target_trans.GetComponent<Image>();
            Expand(sr);
            /*
            Vector3 scale = target_trans.localScale; 
            scale.x = 1.8f; 
            scale.y = 1.8f;
            target_trans.localScale = scale;
            */

        }
                
           
    }
}
