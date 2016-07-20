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
		/// <value>The current mileage.</value>
		float CurrentMileage { get; }

		int AvailableJumpCount { get; }

		/// <summary>
		/// 車を前にすすめる
		/// </summary>
		void MoveForward();

		void Jump();
	}

	public interface IMap : IResetable
	{
	}

	public interface IGameCallback
	{
		/// <summary>
		/// ゲームが終了した時（失敗、もしくは成功）に呼ばれるコールバック
		/// </summary>
		/// <param name="goal">ゴールならtrue、失敗ならfalse</param>
		void OnFinishGame(bool goal);
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

		public GameManager(ICar car, IMap map, IGameCallback gameCallback, IController controller)
		{
			this.map = map;
			this.car = car;
			this.gameCallback = gameCallback;
			this.controller = controller;
		}

		bool finish;

		public void Update()
		{
			if (finish) {
				Debug.Log("Finish return!");
				return;
			}

			if (car.IsCrashed) { // Game Over
				gameCallback.OnFinishGame(false);
				Debug.Log("Finish Game!");
				finish = true;
				Debug.Log("Finish : " + finish);
			} else if (car.CurrentMileage >= goalPointMileage) { // Game Clear
				gameCallback.OnFinishGame(true);
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