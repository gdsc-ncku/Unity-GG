using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] float _playerGravity;
    public float playerGravity
    {
        get
        {
            return _playerGravity;
        }
    }
}
