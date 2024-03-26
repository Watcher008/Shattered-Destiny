using System.Collections.Generic;
using UnityEngine;

//So at the moment this does not take into account differently sized objects,
//nor can I exclude other points from the list
//So I think the first thing that I want to do is find a way to exclude points from the start
//Once I do that, then I can do a pass with each radius, and then on the next pass exclude those points


public static class Poisson
{
    /// <summary>
    /// Returns a list of randomly placed and appropriately spaced points within the given bounds.
    /// </summary>
    public static List<Vector2> GeneratePoints(int seed, int radius, Vector2 sampleRegionSize, int maxAttempts = 30)
    {
        var rng = new System.Random(seed); //Seed the rng
        float cellSize = radius / Mathf.Sqrt(2); //

        int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize), Mathf.CeilToInt(sampleRegionSize.y / cellSize)];
        
        var points = new List<Vector2>();
        var spawnPoints = new List<Vector2>();
        spawnPoints.Add(sampleRegionSize / 2);

        while(spawnPoints.Count > 0)
        {
            int spawnIndex = rng.Next(0, spawnPoints.Count);
            Vector2 spawnCenter = spawnPoints[spawnIndex];

            bool candidateAccepted = false;
            for (int i = 0; i < maxAttempts; i++)
            {
                float angle = rng.Next(0, 100) * 0.01f * Mathf.PI * 2;
                Vector2 direction = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 candidate = spawnCenter + direction * rng.Next(radius, radius * 2);
                if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid))
                {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);
                    grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;
                    candidateAccepted = true;
                    break;
                }
            }
            if (!candidateAccepted) spawnPoints.RemoveAt(spawnIndex);
        }

        return points;
    }

    /// <summary>
    /// Checks all points on grid within radius of the candidate. Returns false if it is too close to another.
    /// </summary>
    private static bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, float radius, List<Vector2> points, int[,] grid)
    {
        if (candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 && candidate.y < sampleRegionSize.y)
        {
            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);

            int searchStartX = (Mathf.Max(0, cellX - 2));
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);

            int searchStartY = (Mathf.Max(0, cellY - 2));
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

            //Check each 
            for (int x = searchStartX; x <= searchEndX; x++)
            {
                for (int y = searchStartY; y <= searchEndY; y++)
                {
                    int pointIndex = grid[x, y] - 1;
                    if (pointIndex != -1) //Not equal to -1, so there is 
                    {
                        float sqrDist = (candidate - points[pointIndex]).sqrMagnitude;
                        if (sqrDist < radius * radius) return false; //candidate is too close to the point
                    }
                }
            }
            return true; //get through all cells and not too close to any
        }
        return false; //outside sample region
    }
}

public struct PoissonGroup
{
    public int ID;
    public int Radius;
}