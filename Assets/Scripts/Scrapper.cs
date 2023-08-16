using UnityEngine;
using System.Collections;

public class Scrapper : MonoBehaviour
{
    private static Scrapper _instance = null;
    public static Scrapper Instance => _instance;

    [SerializeField] private Transform _genPos;
    [SerializeField] private Spiral _genPrefab;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _digSpeed;
    [SerializeField] private Transform _rayStartPos;

    private bool _initiated;
    private Spiral _spiral;
    private float _originYpos;
    private float _originXrotation;
    private const float _minYpos = 1.3f;

    public static Vector3 Pos => Instance.transform.position;

    private void Awake()
    {
        if (null != _instance)
        {
            Debug.Log("[Scrapper] 인스턴스를 중복 생성하려 했습니다");
            return;
        }

        _instance = this;

        _originYpos = transform.position.y;
        _originXrotation = transform.rotation.x;
    }

    private void Start()
    {
        _initiated = true;
    }

    private void Update()
    {
        if (!_initiated)
            return;

        transform.position += Vector3.forward * Time.deltaTime * _moveSpeed;

        if (Input.GetMouseButtonDown(0))
        {
            if (null != _spiral)
            {
                Debug.Log("spiral is not null");
                _spiral.StopGenerate();
                _spiral = null;
            }

            _spiral = Instantiate<Spiral>(_genPrefab, _genPos);
            _spiral.Init();
        }
        else if (Input.GetMouseButton(0))
        {
            if (null == _spiral)
                return;

            if (transform.position.y > _minYpos)
            {
                transform.position += Vector3.down * Time.deltaTime * _digSpeed;
                return;
            }

            if (!Physics.Raycast(_rayStartPos.position, Vector3.down, out var hit, 100f))
                return;

            if (!hit.transform.TryGetComponent<Ground>(out var ground))
                return;

            ground.OnScrape();
            _spiral.Generate();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (null == _spiral)
            {
                Debug.Log("spiral is null");
                return;
            }

            _spiral.StopGenerate();
            _spiral = null;
        }
        else
        {
            if (transform.position.y < _originYpos)
                transform.position += Vector3.up * Time.deltaTime * _digSpeed;
        }
    }

    public void BreakDown()
    {
        _initiated = false;
        if (null != _spiral)
        {
            _spiral.StopGenerate();
            _spiral = null;
        }

        Extension.Invoke(3f, () => GameManager.Restart());
    }
}