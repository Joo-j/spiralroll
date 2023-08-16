using UnityEngine;

public class CameraContainer : MonoBehaviour
{
    private static CameraContainer _instance;
    public static CameraContainer Instance => _instance;

    private Vector3 _offset;

    private void Awake()
    {
        if (null != _instance)
            return;

        _instance = this;
    }

    public static void Init(Vector3 targetPos)
    {
        Instance._offset = Instance.transform.position - targetPos;
    }

    private void Update()
    {
        var targetPos = Scrapper.Pos + _offset;
        transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
    }
}
