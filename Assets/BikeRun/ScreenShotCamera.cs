using UnityEngine;
using System;

namespace BikeRun
{
	public class ScreenShotCamera : MonoBehaviour
	{
		[SerializeField] Camera myCamera;

		Texture2D GetCameraTexture(int width, int height)
		{
			var texture = new RenderTexture(width, height, 24);
			myCamera.targetTexture = texture;
			var screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
			myCamera.Render();
			RenderTexture.active = texture;
			screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
			Destroy(texture);
			myCamera.targetTexture = null;
			RenderTexture.active = null;

			return screenShot;
		}

		public float[] TakeGrayScaleShot(int width, int height)
		{
			var screenShot = GetCameraTexture(width, height);
			var pixels = screenShot.GetPixels();
			var grayPixels = new float[pixels.Length];
			for (var i = 0; i < pixels.Length; i++) {
				grayPixels[i] = (float)Math.Round(pixels[i].grayscale, 2, MidpointRounding.AwayFromZero);
			}
			return grayPixels;
		}
	}
}

