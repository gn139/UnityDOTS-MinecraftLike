using Unity.Mathematics;
using UnityEngine;

namespace Util {
    public class PerlinNoiseUtil {
        static int textureWidth = 200;
        static int textureHeight = 200;

        static float scale1 = 1f;
        static float scale2 = 10f;
        static float scale3 = 20f;
        public static Texture2D GenerateHeightMap (float offsetX, float offsetY) {
            Texture2D heightMap = new Texture2D (textureWidth, textureHeight);

            for (int x = 0; x < textureWidth; x++) {
                for (int y = 0; y < textureHeight; y++) {
                    Color color = CalculateColor (x, y, offsetX, offsetY);
                    heightMap.SetPixel (x, y, color);
                }
            }
            heightMap.Apply ();

            return heightMap;
        }
        static Color CalculateColor (int x, int y, float offsetX, float offsetY) {
            float xCoord1 = (float) x / textureWidth * scale1 + offsetX;
            float yCoord1 = (float) y / textureHeight * scale1 + offsetY;
            float xCoord2 = (float) x / textureWidth * scale2 + offsetX;
            float yCoord2 = (float) y / textureHeight * scale2 + offsetY;
            float xCoord3 = (float) x / textureWidth * scale3 + offsetX;
            float yCoord3 = (float) y / textureHeight * scale3 + offsetY;

            float sample1 = noise.snoise (new float2 (xCoord1, yCoord1)) / 20;
            float sample2 = noise.snoise (new float2 (xCoord2, yCoord2)) / 20;
            float sample3 = noise.snoise (new float2 (xCoord3, yCoord3)) / 20;

            return new Color (sample1 + sample2 + sample3, sample1 + sample2 + sample3, sample1 + sample2 + sample3);
        }
    }
}