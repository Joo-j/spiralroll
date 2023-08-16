using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance => _instance;

    [SerializeField] private int _blockCount;
    [SerializeField] private Ground _groundPrefab;
    [SerializeField] private int _brickCount;
    [SerializeField] private BrickGroup _brickPrefab;
    [SerializeField] private int _obstacleCount;
    [SerializeField] private Obstacle _obstaclePrefab;

    private void Awake()
    {
        if (null != _instance)
            return;

        _instance = this;
    }

    public static void Init()
    {
        var blockPrefab = Instance._groundPrefab;
        var blockCount = Instance._blockCount;
        var blockDist = blockPrefab.transform.localScale.z;
        for (var i = 0; i < blockCount; i++)
        {
            var ground = Instantiate(Instance._groundPrefab);
            ground.transform.position = new Vector3(0, 0, blockDist * i);
        }

        var endPos = new Vector3(0, 0, blockDist * blockCount);
        var brickCount = Instance._brickCount;
        for (var i = 0; i < brickCount; i++)
        {
            var brick = Instantiate(Instance._brickPrefab);
            brick.transform.position = Vector3.Lerp(Vector3.zero, endPos, (float)(i + 1) / brickCount);
        }

        var obstacleCount = Instance._obstacleCount;
        for (var i = 0; i < brickCount; i++)
        {
            var obstacle = Instantiate(Instance._obstaclePrefab);
            obstacle.transform.position = Vector3.Lerp(Vector3.zero, endPos, (float)(i + 1) / obstacleCount);
            obstacle.transform.position += Vector3.up;
        }
    }
}
