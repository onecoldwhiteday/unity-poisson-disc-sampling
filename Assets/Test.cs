using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Test : MonoBehaviour
{

    public float radius = 1;
    public Vector3 regionSize = Vector3.one;
    public int rejectionSamples = 30; 
    public float displayRadius = 1;

    List<Vector3> points;
    
    void OnValidate() {
        points = PoissonDiskSampling.GeneratePoints(radius, regionSize, rejectionSamples);
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireCube(regionSize/2, regionSize);
        if (points != null) {
            foreach (Vector3 point in points) {
                Gizmos.DrawSphere(point, displayRadius);
            }
        }
    }
}
