using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private void Awake()
    {
        if (null != _instance)
            return;

        _instance = this;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        LevelManager.Init();
        CameraContainer.Init(Scrapper.Pos);
    }

    public static void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
