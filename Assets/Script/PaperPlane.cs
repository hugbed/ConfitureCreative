using System.Collections.Generic;
using UnityEngine;

public class PaperPlane : MonoBehaviour
{
    private void Start()
    {
        GameObject original = Resources.Load<GameObject>("PaperPlaneSegment");

        // Create a wing
        var gameObj = Instantiate(original);
        gameObj.transform.parent = transform;
        CreateTesselatedTriangle(20, gameObj.AddComponent<MeshFilter>().mesh);
        gameObj.transform.localScale = new Vector3(0.5f, 1.0f, 1.5f);
        gameObj.transform.Rotate(new Vector3(0, 1f, 0));

        // Create other wing
        gameObj = Instantiate(original);
        gameObj.transform.parent = transform;
        CreateTesselatedTriangle(20, gameObj.AddComponent<MeshFilter>().mesh);
        gameObj.transform.localScale = new Vector3(-0.5f, 1.0f, 1.5f);
        gameObj.transform.position = gameObj.transform.position + Vector3.right * 1f;
        gameObj.transform.Rotate(new Vector3(0, -1f, 0));

        // Create small section
        gameObj = Instantiate(original);
        gameObj.transform.parent = transform;
        CreateTesselatedTriangle(20, gameObj.AddComponent<MeshFilter>().mesh);
        gameObj.transform.localScale = new Vector3(-0.2f, 1.0f, 1f);
        gameObj.transform.position = gameObj.transform.position + Vector3.up * 1f;
    }

    private Mesh CreateTesselatedTriangle(uint resolution, Mesh mesh)
    {
        mesh.Clear();

        // Create vertices and UVs
        var vertices = new List<Vector3>();
        var uv = new List<Vector2>();
        for (int i = 0; i < resolution; ++i)
        {
            for (int j = 0; j < i + 1; ++j)
            {
                var value = new Vector3(i / (float)resolution, 0, j / (float)resolution);
                vertices.Add(value);
                uv.Add(new Vector2(value.x, value.z));
            }
        }

        // Create triangles
        var triangles = new List<int>();
        for (int i = 0; i < resolution - 1; ++i)
        {
            int richy = i * (i + 1) / 2;

            for (int j = 0; j <= i; ++j)
            {
                triangles.Add(richy);
                triangles.Add(richy + i + 1);
                triangles.Add(richy + i + 2);
                if (j != i)
                {
                    triangles.Add(richy);
                    triangles.Add(richy + i + 2);
                    triangles.Add(richy + 1);
                }
                ++richy;
            }
        }

        // Add normals
        var normals = new List<Vector3>();
        for (int i = 0; i < vertices.Count; ++i)
        {
            normals.Add(Vector3.up);
        }

        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();

        return mesh;
    }
}
