using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarycentricDistance
{
    private MeshFilter m_MeshFilter;
    private Mesh m_Mesh;
    private int[] m_Triangles;
    private Vector3[] m_Vertices;
    private Transform m_Transform;

    public struct Result
    {
        public float distanceSquared;

        public float distance
        {
            get
            {
                return Mathf.Sqrt(distanceSquared);
            }
        }

        public int triangle;
        public Vector3 normal;
        public Vector3 centre;
        public Vector3 closestPoint;
    }

    public BarycentricDistance(MeshFilter meshFilter)
    {
        m_MeshFilter = meshFilter;
        m_Mesh = m_MeshFilter.sharedMesh;
        m_Triangles = m_Mesh.triangles;
        m_Vertices = m_Mesh.vertices;
        m_Transform = m_MeshFilter.transform;
    }

    public Result GetClosestTriangleAndPoint(Vector3 point)
    {

        point = m_Transform.InverseTransformPoint(point);
        var minDistance = float.PositiveInfinity;
        var finalResult = new Result();
        var length = (int)(m_Triangles.Length / 3);
        for (var t = 0; t < length; t++)
        {
            var result = GetTriangleInfoForPoint(point, t);
            if (minDistance > result.distanceSquared)
            {
                minDistance = result.distanceSquared;
                finalResult = result;
            }
        }
        finalResult.centre = m_Transform.TransformPoint(finalResult.centre);
        finalResult.closestPoint = m_Transform.TransformPoint(finalResult.closestPoint);
        finalResult.normal = m_Transform.TransformDirection(finalResult.normal);
        finalResult.distanceSquared = (finalResult.closestPoint - point).sqrMagnitude;
        return finalResult;
    }

    Result GetTriangleInfoForPoint(Vector3 point, int triangle)
    {
        Result result = new Result();

        result.triangle = triangle;
        result.distanceSquared = float.PositiveInfinity;

        if (triangle >= m_Triangles.Length / 3)
            return result;


        //Get the vertices of the triangle
        var p1 = m_Vertices[m_Triangles[0 + triangle * 3]];
        var p2 = m_Vertices[m_Triangles[1 + triangle * 3]];
        var p3 = m_Vertices[m_Triangles[2 + triangle * 3]];

        result.normal = Vector3.Cross((p2 - p1).normalized, (p3 - p1).normalized);

        //Project our point onto the plane
        var projected = point + Vector3.Dot((p1 - point), result.normal) * result.normal;

        //Calculate the barycentric coordinates
        var u = ((projected.x * p2.y) - (projected.x * p3.y) - (p2.x * projected.y) + (p2.x * p3.y) + (p3.x * projected.y) - (p3.x * p2.y)) /
                ((p1.x * p2.y) - (p1.x * p3.y) - (p2.x * p1.y) + (p2.x * p3.y) + (p3.x * p1.y) - (p3.x * p2.y));
        var v = ((p1.x * projected.y) - (p1.x * p3.y) - (projected.x * p1.y) + (projected.x * p3.y) + (p3.x * p1.y) - (p3.x * projected.y)) /
                ((p1.x * p2.y) - (p1.x * p3.y) - (p2.x * p1.y) + (p2.x * p3.y) + (p3.x * p1.y) - (p3.x * p2.y));
        var w = ((p1.x * p2.y) - (p1.x * projected.y) - (p2.x * p1.y) + (p2.x * projected.y) + (projected.x * p1.y) - (projected.x * p2.y)) /
                ((p1.x * p2.y) - (p1.x * p3.y) - (p2.x * p1.y) + (p2.x * p3.y) + (p3.x * p1.y) - (p3.x * p2.y));

        result.centre = p1 * 0.3333f + p2 * 0.3333f + p3 * 0.3333f;

        //Find the nearest point
        var vector = (new Vector3(u, v, w)).normalized;


        //work out where that point is
        var nearest = p1 * vector.x + p2 * vector.y + p3 * vector.z;
        result.closestPoint = nearest;
        result.distanceSquared = (nearest - point).sqrMagnitude;

        if (float.IsNaN(result.distanceSquared))
        {
            result.distanceSquared = float.PositiveInfinity;
        }
        return result;
    }

}
