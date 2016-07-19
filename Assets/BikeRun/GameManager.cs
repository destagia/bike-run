using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace BikeRun
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] Car car;
		[SerializeField] MapGenerator mapGenerator;

		public bool IsGameOver { get; private set; }
		public bool IsClear { get; private set; }

		public void GameOver()
		{
			IsGameOver = true;
			IsClear = false;
		}

		public void GameClear()
		{
			IsGameOver = true;
			IsClear = true;
		}

		public void Restart()
		{
			car.Reset();
			mapGenerator.Reset();
			IsGameOver = false;
		}

		void Awake()
		{
			Application.targetFrameRate = 20;
		}
	}
}