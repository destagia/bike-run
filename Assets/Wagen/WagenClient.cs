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

		public bool ShouldJump(float[] pixels)
		{
			var message = string.Join(",", new List<string>(ConvertStringArray(pixels)).ToArray());
			message = "get_action:" + message + "\n";
			bool? isJump = null;
			worker.SendMessage(message, res => isJump = res == "t");
			while (!isJump.HasValue) {

			}
			return isJump.Value;
		}

		public bool ShouldJump(WagenEnvironment env)
		{
			var message = string.Join(",", new List<string>(ConvertStringArray(ConvertEnvironment(env))).ToArray());
			UnityEngine.Debug.Log(message);
			message = "get_action:" + message + "\n";
			bool? isJump = null;

			worker.SendMessage(message, res => isJump = res == "t");
			while (!isJump.HasValue) {

			}

			return isJump.Value;
		}

		public void LearnWin()
		{
			var finish = false;
			worker.SendMessage("learn_win:void\n", _ => finish = true);
			while (!finish) {}
		}

		public void LearnLose()
		{
			var finish = false;
			worker.SendMessage("learn_lose:void\n", _ => finish = true);
			while (!finish) {}
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

