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
				gameManager.Update();
			}
		}

		#region IGameCallback implementation

		public void OnFinishGame(bool goal)
		{
			learning = true;
			if (goal) {
				wagenProxy.LearnWin(OnFinishLearning);
			} else {
				wagenProxy.LearnLose(OnFinishLearning);
			}
		}

		#endregion

		void OnFinishLearning()
		{
			Debug.Log("On Finish");
			learning = false;
			onFinishHandler = gameManager.Restart;
		}
	}
}

