using System;
using System.Collections.Generic;

namespace Wagen
{
	public class WagenClient : IDisposable
	{
		WagenSocketWorker worker;

		public WagenClient()
		{
			worker = new WagenSocketWorker();
		}

		public void Dispose()
		{
			worker.Dispose();
		}

		public bool Jump(WagenEnvironment env)
		{
			var message = string.Join(",", new List<string>(ConvertStringArray(ConvertEnvironment(env))).ToArray());
			UnityEngine.Debug.Log(message);
			worker.SendMessage(message, res => {
				UnityEngine.Debug.Log(res);
			});
			return false;
		}

		IEnumerable<string> ConvertStringArray<T>(IEnumerable<T> enumerable)
		{
			foreach (var item in enumerable) {
				yield return item.ToString();
			}
		}

		IEnumerable<float> ConvertEnvironment(WagenEnvironment env)
		{
			yield return env.car.AvailableJumpCount;
			yield return env.car.Position.x;
			yield return env.car.Position.y;
			yield return env.car.Position.z;
			yield return env.car.Rotation.x;
			yield return env.car.Rotation.y;
			yield return env.car.Rotation.z;
			for (var i = 0; i < 20; i++) {
				if (i < env.stage.Parts.Count) {
					yield return env.stage.Parts[i].Position.x;
					yield return env.stage.Parts[i].Position.y;
					yield return env.stage.Parts[i].Position.z;
					yield return env.stage.Parts[i].Width;
					yield return env.stage.Parts[i].Height;
				} else {
					yield return 0;
					yield return 0;
					yield return 0;
					yield return 0;
					yield return 0;
				}
			}
		}
	}
}

