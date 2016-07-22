using UnityEngine;
using System;

namespace BikeRun
{
	public class Game : MonoBehaviour, IGameCallback
	{
		[SerializeField] Car car;
		[SerializeField] MapGenerator mapGenerator;
		[SerializeField] WagenProxy wagenProxy;
		[SerializeField] WagenController wagenController;
		[SerializeField] InputController inputController;

		[SerializeField] bool useInput;

		GameManager gameManager;

		void Start()
		{
			if (useInput) {
				gameManager = new GameManager(car, mapGenerator, this, inputController);
			} else {
				gameManager = new GameManager(car, mapGenerator, this, wagenController);
			}
			Application.targetFrameRate = 10;
		}

		Action onFinishHandler;
		bool learning;

		void Update()
		{
			if (onFinishHandler != null) {
				onFinishHandler.Invoke();
				onFinishHandler = null;
				return;
			}
			if (!learning) {
				wagenProxy.RewardForPreviousAction = gameManager.Reward;
				gameManager.Update();
			} else {
				Debug.Log("[Game] waiting learn...");
			}
		}

		#region IGameCallback implementation

		public void OnFinishGame()
		{
			Debug.Log("[Game] Game was over!");
			learning = true;
			wagenProxy.IsGameEnd = true;
			wagenProxy.Learn(OnFinishLearning);
		}

		#endregion

		void OnFinishLearning()
		{
			learning = false;
			onFinishHandler = gameManager.Restart;
		}
	}
}

