using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshController : MonoBehaviour
{
    private float radius;
    private int vertexCount;

    private MeshFilter meshFilter;
    private List<Vector3> vertices;
    private List<int> triangles;
    private Mesh mesh;

    private List<CircleMesh> circleMeshes;
    private List<LivePosition> anchors;

    private delegate void OnUpdate();
    private OnUpdate raiseOnUpdate;

    private bool runUpdate = false;
    public void OnInitilized(float radius, int vertexCount, Material material)
    {
        this.radius = radius;
        this.vertexCount = vertexCount;
        
        meshFilter = this.gameObject.AddComponent<MeshFilter>();
        this.gameObject.AddComponent<MeshRenderer>().material = material;

        vertices = new();
        triangles = new();
        circleMeshes = new();
        anchors = new();

        runUpdate = true;
    }

    public void AddPosition(LivePosition position)
    {
        raiseOnUpdate += position.OnUpdate;
        position.SetEndCall(() => raiseOnUpdate -= position.OnUpdate);
        anchors.Add(position);
    }

    public void CallOffUpdate()
    {
        runUpdate = false;
    }

    private void Update()
    {
        if(raiseOnUpdate != null)
        {
            raiseOnUpdate();
            GenerateMesh();
        }

    }


    private void GenerateMesh()
    {
        vertices.Clear();
        triangles.Clear();
        circleMeshes.Clear();
        mesh = new();

        CreateCircles();

        SetTrianglesBetweenCircles();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    private void SetTrianglesBetweenCircles()
    {
        for(int i = 0; i < circleMeshes.Count - 1; i++)
            TriangleBetweenCircles(circleMeshes[i], circleMeshes[i + 1]);
    }

    private void TriangleBetweenCircles(CircleMesh circle_1, CircleMesh circle_2)
    {
        int startIndex_1 = circle_1.StartIndex;
        int startIndex_2 = circle_2.StartIndex;

        for(int i = 0; i < vertexCount - 1; i++)
        {
            //First
            triangles.Add(startIndex_1 + i + 1);
            triangles.Add(startIndex_2 + i + 2);
            triangles.Add(startIndex_2 + i + 1);

            //Second
            triangles.Add(startIndex_1 + i + 1);
            triangles.Add(startIndex_1 + i + 2);
            triangles.Add(startIndex_2 + i + 2);
        }

        triangles.Add(startIndex_1 + vertexCount);
        triangles.Add(startIndex_2 + 1);
        triangles.Add(startIndex_2 + vertexCount);

        triangles.Add(startIndex_1 + vertexCount);
        triangles.Add(startIndex_1 + 1);
        triangles.Add(startIndex_2 + 1);

    }

    private void CreateCircles()
    {
        bool needTriangles, isFlipped;
        for(int i = 0; i < anchors.Count; i++)
        {
            needTriangles = i == 0 || i == anchors.Count - 1;
            isFlipped = i == anchors.Count - 1; 
            CircleMesh circleMesh = new(radius, vertexCount, i, anchors[i].GetPosition(), anchors[i].GetRotation(), needTriangles, isFlipped);

            circleMeshes.Add(circleMesh);
            vertices.AddRange(circleMesh.GetVertices());
            triangles.AddRange(circleMesh.GetTriangles());
        }
    }
}
