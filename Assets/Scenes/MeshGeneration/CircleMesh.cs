using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMesh
{
    private float radius;
    private int resolution;
    private int index;
    private int startIndex;

    private Vector3 position;
    private Quaternion rotation;

    private List<Vector3> vertices;
    private List<int> traingles;

    private bool isFlipped;

    public CircleMesh(float radius, int resolution, int index, Vector3 position, Quaternion rotation, bool needTriangless, bool isFlipped)
    {
        this.radius = radius;
        this.resolution = resolution;
        this.index = index;
        this.position = position;
        this.rotation = rotation;
        this.isFlipped = isFlipped;

        startIndex = index * (resolution + 1);

        vertices = new();
        traingles = new();

        CreateVertices();

        if(needTriangless)
            CreateTriangles();
    }

    public Vector3[] GetVertices()
    {
        return vertices.ToArray();
    }

    public int[] GetTriangles()
    {
        return traingles.ToArray();
    }

    public int StartIndex
    {
        get => startIndex;
    }

    private void CreateVertices()
    {
        Vector3 center = position;
        vertices.Add(position);

        float angle = 360f / resolution;

        for(int i = 0; i < resolution; i++)
        {
            Vector3 vertex = center;

            vertex.x += radius * Mathf.Cos(angle * i * Mathf.Deg2Rad);
            vertex.y += radius * Mathf.Sin(angle * i * Mathf.Deg2Rad);

            vertex = rotation * (vertex - center) + center;
            vertices.Add(vertex);
        }
    }

    private void CreateTriangles()
    {
        for(int i = startIndex; i < resolution - 1 + startIndex; i++)
        {
            traingles.Add(startIndex);
            if (!isFlipped)
            {
                traingles.Add(i + 2);
                traingles.Add(i + 1);
            }
            else
            {
                traingles.Add(i + 1);
                traingles.Add(i + 2);
            }
        }

        traingles.Add(startIndex);

        if (!isFlipped)
        {
            traingles.Add(startIndex + 1);
            traingles.Add(resolution + startIndex);
        }
        else
        {
            traingles.Add(resolution + startIndex);
            traingles.Add(startIndex + 1);
        }
    }

}
