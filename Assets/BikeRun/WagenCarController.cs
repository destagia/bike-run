using UnityEngine;
using Wagen;
using System;

namespace BikeRun
{
	public class WagenCarController : MonoBehaviour
	{
		WagenClient client;

		[SerializeField] Car car;
		[SerializeField] MapGenerator mapGenerator;
		[SerializeField] GameManager gameManager;

		void Start()
		{
			client = new WagenClient();
		}

		void Update()
		{
			if (gameManager.IsGameOver) {
				if (gameManager.IsClear) {
					client.LearnWin();
				} else {
					client.LearnLose();
				}
				gameManager.Restart();
				return;
			}
			var camera = Camera.main;
			var width = 64;
			var height = 36;

			var texture = new RenderTexture(width, height, 24);
			camera.targetTexture = texture;
			var screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
			camera.Render();
			RenderTexture.active = texture;
			screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
			Destroy(texture);
			camera.targetTexture = null;
			RenderTexture.active = null;

			var pixels = screenShot.GetPixels();
			var grayPixels = new float[pixels.Length];
			for (var i = 0; i < pixels.Length; i++) {
				grayPixels[i] = (float)Math.Round(pixels[i].grayscale, 2, MidpointRounding.AwayFromZero);
			}

			// if (client.ShouldJump(new WagenEnvironment(mapGenerator, car))) {
			if (client.ShouldJump(grayPixels)) {
				car.Jump();
			}
		}
	}
}

