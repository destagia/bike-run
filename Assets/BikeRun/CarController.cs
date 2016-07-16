using UnityEngine;
using System.Collections;

namespace BikeRun
{
	public class CarController : MonoBehaviour
	{
		[SerializeField] Car car;

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space)) {
				car.Jump();
			}
		}
	}
}