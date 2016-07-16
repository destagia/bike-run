using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace BikeRun
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] Car car;
		[SerializeField] MapGenerator mapGenerator;

		public void GameOver()
		{
			car.Reset();
			mapGenerator.Reset();
		}
	}
}