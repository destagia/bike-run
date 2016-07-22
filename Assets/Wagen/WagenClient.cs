using System;
using System.Collections.Generic;

namespace Wagen
{
	public class WagenClient : IDisposable
	{
		WagenSocketWorker worker;

		public WagenClient(string host, int port)
		{
			worker = new WagenSocketWorker(host, port);
		}

		public void Dispose()
		{
			worker.Dispose();
		}

		public void ShouldJump(float[] pixels, float prevReward, bool isGameEnd, Action<bool> callback)
		{
			var rawImage = string.Join(",", new List<string>(ConvertStringArray(pixels)).ToArray());
			var message = string.Format("get_action:{0};{1};{2}", rawImage, prevReward, isGameEnd);
			worker.SendMessage(message, res => { callback.Invoke(res == "t"); });
		}

//		public void ShouldJump(WagenEnvironment env, Action<bool> callback)
//		{
//			var message = string.Join(",", new List<string>(ConvertStringArray(ConvertEnvironment(env))).ToArray());
//			message = "get_action:" + message;
//			worker.SendMessage(message, res => callback.Invoke(res == "t"));
//		}

		public void Learn(Action callback)
		{
			worker.SendMessage("learn:", _ => callback.Invoke());
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
			for (var i = 0; i < 3; i++) {
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

