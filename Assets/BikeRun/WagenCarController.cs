using UnityEngine;
using Wagen;

namespace BikeRun
{
	public class WagenCarController : MonoBehaviour
	{
		WagenClient client;

		[SerializeField] Car car;
		[SerializeField] MapGenerator mapGenerator;

		void Start()
		{
			client = new WagenClient();
		}

		void Update()
		{
			if (client.ShouldJump(new WagenEnvironment(mapGenerator, car))) {
				car.Jump();
			}
		}
	}
}

