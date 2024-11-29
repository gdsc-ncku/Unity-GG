using UnityEngine;

public static class LayerManager
{
    public static bool IsPlayer(this GameObject obj)
    {
        return obj.layer == LayerMask.NameToLayer("Player");
    }
}
