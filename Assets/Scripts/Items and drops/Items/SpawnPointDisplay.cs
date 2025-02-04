using UnityEngine;

/// <summary>
/// 重生點展示的道具
/// </summary>
public class SpawnPointDisplay: Item
{
    public GameObject spawnPointDisplayer;
    public float sensorRegion = 50f;
    public float destroyTime = 3f;

    public override void ItemUsing()
    {
        base.ItemUsing();

        //throw spawn point shower
        GameObject obj = Instantiate(spawnPointDisplayer);
        obj.transform.position = PlayerManager.Instance.gameObject.transform.position;

        obj.GetComponent<SpawnPointDisplayer>().InitDisplayer(sensorRegion, destroyTime);
        //init spawn point shower, eg. destroy time, sensor range
    }
}
