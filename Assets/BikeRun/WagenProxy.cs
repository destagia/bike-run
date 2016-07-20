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

		const int ScreenShotWidth = 64;
		const int ScreenShotHeight = 36;

		void Awake()
		{
			client = new WagenClient();
		}

		public bool ShouldJump()
		{
			var start = DateTime.Now.Ticks;
			var bytes = screenShooter.TakeGrayScaleShot(ScreenShotWidth, ScreenShotHeight);
			bool? jump = null;
			client.ShouldJump(bytes, shouldJump => {
				jump = shouldJump;
			});
			while (!jump.HasValue && (DateTime.Now.Ticks - start) < 5000000) {
			}
			return jump.Value;
		}

		public void LearnLose(Action callback)
		{
			client.LearnLose(callback);
		}

		public void LearnWin(Action callback)
		{
			client.LearnWin(callback);
		}
	}
}

