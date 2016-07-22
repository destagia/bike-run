using System;
using UnityEngine;
using Wagen;

namespace BikeRun
{
	/// <summary>
	/// こいつがWagenと実際にやり取りする
	/// </summary>
	public class WagenProxy : MonoBehaviour
	{
		WagenClient client;

		[SerializeField] ScreenShotCamera screenShooter;
		[SerializeField] string host;
		[SerializeField] int port;

		const int ScreenShotWidth = 64;
		const int ScreenShotHeight = 36;

		public int FrameCount { get; private set; }

		public float RewardForPreviousAction { private get; set; }
		public bool IsGameEnd { private get; set; } 

		void Awake()
		{
			client = new WagenClient(host, port);
		}

		public bool ShouldJump()
		{
			var start = DateTime.Now;
			var bytes = screenShooter.TakeGrayScaleShot(ScreenShotWidth, ScreenShotHeight);
			bool? jump = null;
			client.ShouldJump(bytes, RewardForPreviousAction, false, shouldJump => {
				jump = shouldJump;
			});
			while (!jump.HasValue && (DateTime.Now - start).Milliseconds < 500) {
			}
			FrameCount += 1;
			RewardForPreviousAction = 0;
			IsGameEnd = false;
			return jump.GetValueOrDefault();
		}

		public void Learn(Action callback)
		{
			Debug.LogFormat("Current Frame : {0}", FrameCount);
			if (FrameCount > 1000) {
				client.Learn(callback);
				FrameCount = 0;
			} else {
				callback();
			}
		}
	}
}

