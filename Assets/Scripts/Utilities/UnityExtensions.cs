using UnityEngine;

public static class UnityExtensions
{
    public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

    /// <summary>
    /// Calculate the positions for the forward[0], rear[1], right[2], and left[3] faces of a Transform in local space.
    /// </summary>
    /// <param name="center">Central point represented by a Transform.</param>
    /// <returns>Vector3[] containing forward[0], rear[1], right[2], and left[3] face positions in local space.</returns>
    public static Vector3[] CalculateLocalXZFacePoints(this Transform center)
    {
        Vector3[] facePoints = new Vector3[4];
        
        facePoints[0] = center.InverseTransformPoint(center.position + center.forward * center.localScale.z / 2);
        facePoints[1] = center.InverseTransformPoint(center.position - center.forward * center.localScale.z / 2);
        facePoints[2] = center.InverseTransformPoint(center.position + center.right * center.localScale.x / 2);
        facePoints[3] = center.InverseTransformPoint(center.position - center.right * center.localScale.x / 2);

        return facePoints;
    }
}
