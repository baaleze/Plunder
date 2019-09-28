using UnityEngine;
using System.Collections;

public static class FalloffGenerator {



	public static float[,] GenerateFalloffMap(int size, Texture2D heightmap) {
		float[,] map = new float[size,size];

		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {


              //  float x = heightmap.GetPixel(i, j).grayscale; // i / (float)size * 2 - 1;

              //  float y = heightmap.GetPixel(i, j).grayscale;

				//float value = Mathf.Max (Mathf.Abs (i / (float)size * 2 - 1), Mathf.Abs (j / (float)size * 2 - 1));
                map[i, j] = heightmap.GetPixel(i, j).grayscale ; // Evaluate(value);
			}
		}

		return map;
	}

	static float Evaluate(float value) {
		float a = 3;
		float b = 2.2f;

		return Mathf.Pow (value, a) / (Mathf.Pow (value, a) + Mathf.Pow (b - b * value, a));
	}
}
