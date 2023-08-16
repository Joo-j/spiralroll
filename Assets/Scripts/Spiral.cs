using UnityEngine;
using System.Collections;

public class Spiral : MonoBehaviour
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private Rigidbody _rigidbody;

    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;
    private float _initTime;
    private Quaternion _generateRotation;
    private const float VerticeX = 0.1f;
    private const float VerticeY = 0.01f;
    private const float VerticeZ = 0.01f;
    private const float MaxTriangleCount = 2000;
    private readonly Vector3 _rigidbodyVelocity = new Vector3(0, 10, 23);

    public bool Max
    {
        get
        {
            if (null == _triangles)
                return true;

            var count = _triangles.Length / 3;
            if (count < MaxTriangleCount)
                return false;

            return true;
        }
    }

    public void Init()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        _generateRotation = new Quaternion();
        _generateRotation.eulerAngles = new Vector3(15, 0, 0);

        _collider.enabled = false;
        _rigidbody.useGravity = false;

        _mesh = new Mesh();
        _meshFilter.mesh = _mesh;

        _initTime = Time.time;
    }

    public void Generate()
    {
        if (null == _vertices)
        {
            CreateMesh();
            UpdateMesh();
        }

        if (Max)
        {
            StopGenerate();
            return;
        }

        RefreshVertices();
        AddVertices();
        UpdateMesh();
    }

    void CreateMesh()
    {
        _vertices = new Vector3[]
        {
            new Vector3(-VerticeX,VerticeY,0),
            new Vector3(VerticeX,VerticeY,0),
            new Vector3(-VerticeX,0,0),
            new Vector3(VerticeX,0,0),
            new Vector3(-VerticeX,VerticeY,VerticeZ),
            new Vector3(VerticeX,VerticeY,VerticeZ),
            new Vector3(-VerticeX,0,VerticeZ),
            new Vector3(VerticeX,0,VerticeZ)
        };

        _triangles = new int[] { 3, 2, 0, 3, 0, 1, 7, 4, 6, 7, 5, 4 };
    }

    private void RefreshVertices()
    {
        if (null == _vertices)
            return;

        int count = _vertices.Length;
        var offset = new Vector3(0, VerticeY + (Time.time - _initTime) * 0.1f, 0);
        for (int i = 0; i < count; i++)
        {
            _vertices[i] = _generateRotation * _vertices[i];
            _vertices[i] += offset;
        }

        var radius = Vector3.Distance(_collider.center, _vertices[_vertices.Length - 1]) / 2;
        transform.localPosition = new Vector3(0, radius, radius);
        transform.localRotation = Quaternion.identity;
    }

    private void AddVertices()
    {
        var vertexCount = _vertices.Length;
        var triangleCount = _triangles.Length;
        var newVertices = new Vector3[vertexCount + 4];
        var newTriangles = new int[triangleCount + 12];

        for (int i = 0; i < vertexCount; i++)
        {
            newVertices[i] = _vertices[i];
        }
        for (int i = 0; i < triangleCount; i++)
        {
            newTriangles[i] = _triangles[i];
        }

        newVertices[vertexCount] = new Vector3(-VerticeX, 0, 0);
        newVertices[vertexCount + 1] = new Vector3(VerticeX, 0, 0);
        newVertices[vertexCount + 2] = new Vector3(-VerticeX, 0, VerticeZ);
        newVertices[vertexCount + 3] = new Vector3(VerticeX, 0, VerticeZ);

        newTriangles[triangleCount] = vertexCount;
        newTriangles[triangleCount + 1] = vertexCount - 3;
        newTriangles[triangleCount + 2] = vertexCount + 1;
        newTriangles[triangleCount + 3] = vertexCount;
        newTriangles[triangleCount + 4] = vertexCount - 4;
        newTriangles[triangleCount + 5] = vertexCount - 3;
        newTriangles[triangleCount + 6] = vertexCount + 2;
        newTriangles[triangleCount + 7] = vertexCount + 3;
        newTriangles[triangleCount + 8] = vertexCount - 1;
        newTriangles[triangleCount + 9] = vertexCount + 2;
        newTriangles[triangleCount + 10] = vertexCount - 1;
        newTriangles[triangleCount + 11] = vertexCount - 2;

        _vertices = newVertices;
        _triangles = newTriangles;
    }

    private void UpdateMesh()
    {
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.RecalculateNormals();
    }

    public void StopGenerate()
    {
        if (null == _vertices)
            return;

        if (_rigidbody.useGravity)
            return;

        _rigidbody.useGravity = true;
        _rigidbody.velocity = _rigidbodyVelocity;
        _collider.center = new Vector3(0, _vertices[0].y, _vertices[0].z);
        _collider.height = VerticeX * 2;
        _collider.radius = Vector3.Distance(_collider.center, _vertices[_vertices.Length - 1]);
        _collider.enabled = true;
        transform.parent = null;

        Extension.Invoke(10f, () => Destroy(gameObject));
    }
}
