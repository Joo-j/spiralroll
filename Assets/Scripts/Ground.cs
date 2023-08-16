using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private float _deformationAmount;
    [SerializeField] private GameObject _surface;
    [SerializeField] private Coin _coinPrefab;

    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;

    private void Awake()
    {
        _mesh = _meshFilter.mesh;
        _vertices = _mesh.vertices;
        _triangles = _mesh.triangles;
    }

    public void OnScrape()
    {
        var dir = Random.value > 0.5f ? CoinGenDirection.Left : CoinGenDirection.Right;
        var coin = Instantiate<Coin>(_coinPrefab, transform);
        coin.Init(dir);

        _surface.SetActive(false);
    }

    public void OnScrape(Vector3 hitPoint)
    {
        for (int i = 0; i < _triangles.Length; i += 3)
        {
            Vector3 v1 = _vertices[_triangles[i]];
            Vector3 v2 = _vertices[_triangles[i + 1]];
            Vector3 v3 = _vertices[_triangles[i + 2]];

            float denominator = ((v2.y - v3.y) * (v1.x - v3.x) + (v3.x - v2.x) * (v1.y - v3.y));
            float aWeight = ((v2.y - v3.y) * (hitPoint.x - v3.x) + (v3.x - v2.x) * (hitPoint.y - v3.y)) / denominator;
            if (aWeight < 0)
                continue;

            float bWeight = ((v3.y - v1.y) * (hitPoint.x - v3.x) + (v1.x - v3.x) * (hitPoint.y - v3.y)) / denominator;
            if (bWeight < 0)
                continue;

            float cWeight = 1 - aWeight - bWeight;
            if (cWeight < 0)
                continue;

            var amount = Vector3.up * _deformationAmount;

            _vertices[_triangles[i]] -= amount;
            _vertices[_triangles[i + 1]] -= amount;
            _vertices[_triangles[i + 2]] -= amount;

            _mesh.vertices = _vertices;
            _mesh.RecalculateNormals();
            return;
        }
    }
}
