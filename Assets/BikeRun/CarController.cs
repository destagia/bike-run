using UnityEngine;
using System.Collections;

namespace BikeRun
{
	public class CarController : MonoBehaviour
	{
		[SerializeField] Car car;
		[SerializeField] GameManager gameManager;

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space)) {
				car.Jump();
			}
			if (Input.GetKeyDown(KeyCode.R)) {
				gameManager.Restart();
			}
		}
	}
}