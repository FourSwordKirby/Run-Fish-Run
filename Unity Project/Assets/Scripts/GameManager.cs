using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    private static GameManager instance;

    private static PlayerMovement _player;
    public static PlayerMovement Player
    {
        get
        {
            if (_player == null) FindPlayer();
            return _player;
        }
    }

    private static CameraControls _camera;
    public static CameraControls Camera
    {
        get
        {
            if (_camera == null) FindCamera();
            return _camera;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != instance)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void Start()
    {
        FindPlayer();
        FindCamera();
    }

    public static void LoadScene(string sceneName, bool persistPlayer = true)
    {
        if (persistPlayer)
            DontDestroyOnLoad(Player);
        else
        {
            Destroy(Player);
            _player = null;
        }

        Application.LoadLevel(sceneName);

        FindPlayer();
        FindCamera();
    }

    private static void FindCamera()
    {
        _camera = FindObjectOfType<CameraControls>();
        if (_camera == null)
        {
            Debug.Log("Cannot find camera on the current scene.");
        }
    }

    private static void FindPlayer()
    {
        _player = Object.FindObjectOfType<PlayerMovement>();
        if (_player == null)
        {
            Debug.Log("Cannot find player on the current scene.");
        }
    }

    public static void PlayerDeath()
    {
        Camera.Shake();
    }
}
