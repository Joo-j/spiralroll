using UnityEngine;

public class BrickGroup : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private Vector2 _powerRange = new Vector2(5f, 40f);

    private Brick[] _blocks;

    private void Awake()
    {
        _blocks = GetComponentsInChildren<Brick>();
        for (var i = 0; i < _blocks.Length; i++)
        {
            var normal = (transform.position - _blocks[i].transform.position).normalized;
            var power = Random.Range(_powerRange.x, _powerRange.y);
            _blocks[i].Init(normal, power);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        OnEnter_Spiral(other);
    }

    private void OnEnter_Scrapper(Collider other)
    {
        if (!other.TryGetComponent<Scrapper>(out var scrapper))
            return;

        scrapper.BreakDown();
    }

    private void OnEnter_Spiral(Collider other)
    {
        if (!other.TryGetComponent<Spiral>(out var mesh))
            return;

        _collider.enabled = false;
        for (var i = 0; i < _blocks.Length; i++)
        {
            _blocks[i].Activate();
        }

        Extension.Invoke(5f, () => Destroy(gameObject));
    }
}
