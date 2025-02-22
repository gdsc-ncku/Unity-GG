using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region 建立單例模式
    //instance mode
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Can't Find Player Manager Instance");
            }
            return _instance;
        }

    }

    #endregion

    private void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogError("Duplicate creating PlayerManager Instance");
            Destroy(gameObject);
        }
    }

    [SerializeField] float _playerGravity;
    public float playerGravity
    {
        get
        {
            return _playerGravity;
        }
    }
}
