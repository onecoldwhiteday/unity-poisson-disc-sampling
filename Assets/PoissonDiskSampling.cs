using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoissonDiskSampling
{
   public static List<Vector3> GeneratePoints(float radius, Vector3 sampleRegionSize, int numSamplesBeforeRejection = 30 ) 
   {
       float cellSize = radius/Mathf.Sqrt(2);

       int[,,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x/cellSize), Mathf.CeilToInt(sampleRegionSize.y/cellSize), Mathf.CeilToInt(sampleRegionSize.z/cellSize)];
       List<Vector3> points = new List<Vector3>();
       List<Vector3> spawnPoints = new List<Vector3>();

       spawnPoints.Add(sampleRegionSize/2);

       while (spawnPoints.Count > 0) 
       {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector3 spawnCenter = spawnPoints[spawnIndex];

           bool candidateAccepted = false;

           for (int i = 0; i < numSamplesBeforeRejection; i++) 
           {
               float angle = Random.value * Mathf.PI * 2;

               Vector3 dir = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), Mathf.Tan(angle));

               Vector3 candidate = spawnCenter + dir * Random.Range(radius, 2 * radius);

               if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid)) 
               {
                   points.Add(candidate);
                   spawnPoints.Add(candidate);
                   grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize), (int)(candidate.z / cellSize)] = points.Count;
                   candidateAccepted = true;
                   break;
               }
           }

        if (!candidateAccepted) {
            spawnPoints.RemoveAt(spawnIndex);
        }
       }
       return points;
   }

   static bool IsValid(Vector3 candidate, Vector3 sampleRegionSize, float cellSize, float radius, List<Vector3> points, int[,,] grid)
   {
       if (candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 && candidate.y < sampleRegionSize.y && candidate.z >=0 && candidate.z < sampleRegionSize.z) 
       {
           int cellX = (int)(candidate.x/cellSize);
           int cellY = (int)(candidate.y/cellSize);
           int cellZ = (int)(candidate.z/cellSize);

           int searchStartX = Mathf.Max(0, cellX - 2);
           int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);

           int searchStartY = Mathf.Max(0, cellY - 2);
           int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

            int searchStartZ = Mathf.Max(0, cellZ - 2);
            int searchEndZ = Mathf.Min(cellZ + 2, grid.GetLength(2) - 1);

            for (int x = searchStartX; x <= searchEndX; x++) 
           {
               for(int y = searchStartY; y <= searchEndY; y++)
               {
                   for (int z = searchStartZ; z <= searchEndZ; z++)
                   {
                        int pointIndex = grid[x,y,z] - 1;
                        if(pointIndex != -1)
                        {
                            float sqrDist = (candidate - points[pointIndex]).sqrMagnitude;
                            if(sqrDist < radius*radius)
                            {
                                return false;
                            }
                        }
                   }
               }
           }
           return true;
       }
       return false;
   }
}
