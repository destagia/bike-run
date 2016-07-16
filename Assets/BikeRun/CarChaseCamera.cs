using UnityEngine;
using System.Collections;

namespace BikeRun
{
	[RequireComponent(typeof(Camera))]
	public class CarChaseCamera : MonoBehaviour
	{
		[SerializeField] Car carToChase;
		[SerializeField] Vector3 offset;

		void LateUpdate()
		{
			var x = carToChase.transform.position.x + offset.x;
			var y = transform.position.y + offset.y;
			var z = transform.position.z + offset.z;
			transform.position = new Vector3(x, y, z);
		}
	}
}