using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reducer : Item
{
    public override void ItemUsing()
    {
        base.ItemUsing();
        Time.timeScale = 0.3f;
        Debug.Log("減速成功");
    }
}
