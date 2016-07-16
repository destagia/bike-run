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
			car.transform.position = new Vector3(0, 8, 0);
			mapGenerator.Reset();
		}
	}
}