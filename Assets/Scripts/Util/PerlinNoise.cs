using System.Collections;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PerlinNoise {
  private int seed;
  private int octaves;
  private float persistence;
  private float lacunarity;
  private float scale;

  public PerlinNoise (int seed, float scale) {
    this.seed = seed;
    this.persistence = 0.5f;
    this.lacunarity = 2;
    this.octaves = 4;
    this.scale = scale;
  }

  public NativeArray<float> GetNoiseMap (int lowerX, int upperX, int lowerZ, int upperZ) {
    int xRange = upperX - lowerX;
    int zRange = upperZ - lowerZ;

    var noiseMap = new NativeArray<float> (xRange * zRange, Allocator.TempJob);

    for (int i = 0; i < xRange; i++) {
      for (int j = 0; j < zRange; j++) {
        var frequency = 1f;
        var amplitude = 1f;
        var noise = 0f;

        for (int octave = 0; octave < octaves; octave++) {
          // var noiseValue = Mathf.PerlinNoise ((lowerX + i) / scale * frequency + seed, (lowerZ + j) / scale * frequency + seed);
          var noiseValue = Unity.Mathematics.noise.cnoise (new float2 ((lowerX + i) / scale * frequency + seed,
            (lowerZ + j) / scale * frequency + seed));
          noise += noiseValue * amplitude;

          amplitude *= persistence;
          frequency *= lacunarity;
        }

        // noiseMap[i, j] = noise;
        noiseMap[i + j * xRange] = noise;
      }
    }

    // normalize
    for (int i = 0; i < xRange; i++) {
      for (int j = 0; j < zRange; j++) {
        // noiseMap[i, j] = Mathf.InverseLerp (0.35f, 1.25f, noiseMap[i, j]);
        noiseMap[i + j * xRange] = math.unlerp (0.35f, 1.25f, noiseMap[i + j * xRange]);
      }
    }

    return noiseMap;
  }
}