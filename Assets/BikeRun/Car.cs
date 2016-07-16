using UnityEngine;
using System.Collections;
using Wagen;

namespace BikeRun
{
	public class Car : MonoBehaviour, IWagenCar
	{
		[SerializeField] GameManager gameManager;

		[SerializeField] Rigidbody rigid;
		[SerializeField] float jumpStrength;
		[SerializeField] float speed;

		const float AngleMax = 45;

		int jumpCount;

		#region IWagenCar implementation

		public Vector3 Position {
			get { return transform.position; }
		}

		public Vector3 Rotation {
			get { return transform.rotation.eulerAngles; }
		}

		public int AvailableJumpCount {
			get { return 2 - jumpCount; }
		}

		#endregion

		public void Jump()
		{
			if (jumpCount++ >= 2) {
				return;
			}
			rigid.AddForce(new Vector3(0, jumpStrength, 0), ForceMode.Impulse);
		}

		public void Reset()
		{
			transform.position = new Vector3(0, 8, 0);
			transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
			rigid.angularVelocity = Vector3.zero;
		}

		void OnCollisionEnter(Collision collision)
		{
			var horizontal = false;
			foreach (var cont in collision.contacts) {
				if (cont.normal.x < 0) {
					horizontal = true;
					break;
				}
			}
			if (horizontal) {
				Debug.Log("[GameManager] Game Over with collision to wall");
				gameManager.GameOver();
			}
			jumpCount = 0;
		}

		void OnCollisionStay(Collision collision)
		{
			var eulerRotation = transform.rotation.eulerAngles;
			if (85 <= eulerRotation.x && eulerRotation.x <= 275) {
				Debug.Log("[GameManager] Game Over with over rotation");
				gameManager.GameOver();
			}
		}

		void Update()
		{
			if (transform.position.y < -3) {
				Debug.Log("[GameManager] Game Over with under position");
				gameManager.GameOver();
			}

			transform.position = new Vector3(transform.position.x + speed, transform.position.y, 0);
		}
	}
}