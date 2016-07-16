using UnityEngine;
using System.Collections.Generic;
using Wagen;

namespace Test
{
	public class StubStage : IWagenStage
	{
		public List<IWagenPart> Parts {
			get {
				var parts = new List<IWagenPart>();
				for (var i = 0; i < 15; i++) {
					parts.Add(new StubPart());
				}
				return parts;
			}
		}
	}

	public class StubPart : IWagenPart
	{
		public Vector3 Position { get { return new Vector3(5.123f, 5.999f, 5.111f); } }
		public float Width { get { return 10f; } } 
		public float Height { get { return 5f; } }
	}

	public class StubCar : IWagenCar
	{
		public Vector3 Position {
			get { return new Vector3(1f, 10.5f, 0f); }
		}
		public Vector3 Rotation {
			get { return new Vector3(10f, 0f, 0f); }
		}
		public int AvailableJumpCount {
			get { return 1; }
		}
	}

	public class SocketTest : MonoBehaviour
	{
		WagenClient client;

		void OnDestroy()
		{
			if (client != null) {
				client.Dispose();
			}
		}

		void Start()
		{
			client = new WagenClient();
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.A)) {
				var env = new WagenEnvironment(new StubStage(), new StubCar());
				client.ShouldJump(env);
			}
		}
	}
}

