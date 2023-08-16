
using UnityEngine;

public enum CoinGenDirection
{
    Left,
    Right,
}

public class Coin : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    public void Init(CoinGenDirection direction)
    {
        switch (direction)
        {
            case CoinGenDirection.Left:
                transform.localPosition = new Vector3(-1, 0, 0);
                _rigidbody.velocity = new Vector3(-1, 1, 0);
                break;

            case CoinGenDirection.Right:
                transform.localPosition = new Vector3(1, 0, 0);
                _rigidbody.velocity = new Vector3(1, 1, 0);
                break;
        }

        Extension.Invoke(5f, () => Destroy(this.gameObject));
    }
}
