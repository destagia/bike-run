using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace BikeRun
{
	public interface IResetable
	{
		void Reset();
	}

	public interface ICar : IResetable
	{
		/// <summary>
		/// 車が走行不能になっているかどうか
		/// これがtrueを返したフレームでゲームは終了する
		/// </summary>
		bool IsCrashed { get; }

		/// <summary>
		/// 現在の車の走行距離
		/// </summary>
		Vector3 Position { get; }

		Vector3 Rotation { get; }

		int AvailableJumpCount { get; }

		/// <summary>
		/// 車を前にすすめる
		/// </summary>
		void MoveForward();

		void Jump();

		bool IsInSky { get; }
	}

	public interface IMap : IResetable
	{
		Vector3? GetNearestHolePosition(Vector3 position);
	}

	public interface IGameCallback
	{
		/// <summary>
		/// ゲームが終了した時（失敗、もしくは成功）に呼ばれるコールバック
		/// </summary>
		void OnFinishGame();
	}

	public interface IController
	{
		bool ShouldJump();
	}

	public class GameManager
	{
		readonly ICar car;
		readonly IMap map;
		readonly IGameCallback gameCallback;
		readonly IController controller;

		const float goalPointMileage = 300;

		public float Reward { private set; get; }

		public GameManager(ICar car, IMap map, IGameCallback gameCallback, IController controller)
		{
			this.map = map;
			this.car = car;
			this.gameCallback = gameCallback;
			this.controller = controller;
		}

		bool finish;

		bool carKeepInSky;
		bool carJumpOverHole;
		Vector3? nearestHole;

		public void Update()
		{
			if (finish) {
				Debug.Log("Finish return!");
				return;
			}

			if (!carKeepInSky && car.IsInSky) {
				// Car was not in sky at previous frame
				nearestHole = map.GetNearestHolePosition(car.Position);
			} else if (carKeepInSky && car.IsInSky) {
				if (nearestHole.HasValue && !carJumpOverHole && car.Position.x > nearestHole.Value.x) {
					carJumpOverHole = true;
				}
			} else if (carKeepInSky && !car.IsInSky && carJumpOverHole) {
				// Car jump over a hole and got the ground!
				if (!(5 < car.Rotation.x && car.Rotation.x < 355)) {
					Debug.Log("[GameManager] car jumped over the hole!");
					Reward = 0.1f;
				} else {
					Debug.Log("[GameManager] car jumped, but failed");
					Reward = -0.1f;
				}
			}
			carKeepInSky = car.IsInSky;

			if (car.IsCrashed) { // Game Over
				Reward = -1;
				gameCallback.OnFinishGame();
				finish = true;
			} else if (car.Position.x >= goalPointMileage) { // Game Clear
				Reward = 1;
				gameCallback.OnFinishGame();
				finish = true;
			} else {
				if (controller.ShouldJump()) {
					car.Jump();
				}
				car.MoveForward();
			}
		}

		public void Restart()
		{
			car.Reset();
			map.Reset();
			finish = false;
		}

	}
}