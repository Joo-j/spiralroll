using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private Rigidbody _rigidbody;

    private Vector3 _velocity;

    public void Init(Vector3 direction, float power)
    {
        _collider.enabled = false;
        _rigidbody.useGravity = false;
        _velocity = direction * power;
    }

    public void Activate()
    {
        _collider.enabled = true;
        _rigidbody.velocity = _velocity;
        _rigidbody.useGravity = true;
    }
}
