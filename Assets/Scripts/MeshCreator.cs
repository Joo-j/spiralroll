using UnityEngine;

public class MeshCreator : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Vector3[] _vertices;
    [SerializeField] private int[] _trinangles;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            Create();
    }

    public void Create()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = _vertices;
        mesh.triangles = _trinangles;

        meshFilter.mesh = mesh;
    }
}