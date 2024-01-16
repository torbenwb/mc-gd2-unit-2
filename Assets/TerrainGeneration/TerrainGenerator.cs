
using UnityEngine;

public class TerrainGenerator
{
    public static float noiseBias = 70f;
    public static float noiseScale = 12f;
    public static int seed = 30000;

    public static Voxel.Type GetVoxelType(int x, int y, int z){
        x += seed;
        z += seed;

        float bias = 1 / noiseBias;
        float heightMap1 = Mathf.PerlinNoise(x * bias, z * bias) * (noiseScale / 2);
        float heightMap2 = Mathf.PerlinNoise(x * bias * 5, z * bias * 5) * (noiseScale / 2) * (Mathf.PerlinNoise(x * (bias / 3), z * (bias / 3)) + 0.5f);

        float baseLandHeight = heightMap1 + heightMap2;
        if (y <= baseLandHeight){
            if (y > baseLandHeight - 1) return Voxel.Type.Grass;
            return Voxel.Type.Dirt;
        }

        return Voxel.Type.Air;
    }
}
