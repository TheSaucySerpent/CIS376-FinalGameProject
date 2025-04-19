using UnityEngine;

public class PerlinTerrain : MonoBehaviour
{

    
    public int width = 100;
    public int depth = 100;
    public float scale = 10f;
    public float heightMultiplier = 10f;
    
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh mesh;

    void Start()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
        mesh = meshFilter.mesh;
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        Vector3[] vertices = new Vector3[(width + 1) * (depth + 1)];
        int[] triangles = new int[width * depth * 6];
        Vector2[] uvs = new Vector2[vertices.Length];
        Color[] colors = new Color[vertices.Length];

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.blue, 0.2f),   // Water
                new GradientColorKey(Color.black, 0.3f),  // Grass
                new GradientColorKey(Color.green, 0.7f), // Sand
                new GradientColorKey(Color.gray, 0.8f),   // Rock
                new GradientColorKey(Color.white, 1.0f)   // Snow
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            }
        );

        float minHeight = float.MaxValue;
        float maxHeight = float.MinValue;

        // Generate vertices
        for (int z = 0; z <= depth; z++)
        {
            for (int x = 0; x <= width; x++)
            {
                float y = Mathf.PerlinNoise(x / scale, z / scale) * heightMultiplier;
                vertices[z * (width + 1) + x] = new Vector3(x, y, z);
                uvs[z * (width + 1) + x] = new Vector2((float)x / width, (float)z / depth);
                
                if (y < minHeight) minHeight = y;
                if (y > maxHeight) maxHeight = y;
            }
        }

        // Assign colors based on height
        for (int i = 0; i < vertices.Length; i++)
        {
            float heightNormalized = Mathf.InverseLerp(minHeight, maxHeight, vertices[i].y);
            colors[i] = gradient.Evaluate(heightNormalized);
        }

        // Generate triangles
        int tris = 0;
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                int vertexIndex = z * (width + 1) + x;
                triangles[tris++] = vertexIndex;
                triangles[tris++] = vertexIndex + width + 1;
                triangles[tris++] = vertexIndex + 1;

                triangles[tris++] = vertexIndex + 1;
                triangles[tris++] = vertexIndex + width + 1;
                triangles[tris++] = vertexIndex + width + 2;
            }
        }

        // Apply to mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.colors = colors;
        mesh.RecalculateNormals();
    }
}
