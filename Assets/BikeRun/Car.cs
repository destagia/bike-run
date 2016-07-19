using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using Wagen;

namespace BikeRun
{
	public class Car : MonoBehaviour, IWagenCar, ICar
	{
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

		#endregion

		public int AvailableJumpCount {
			get { return 2 - jumpCount; }
		}

		#region ICar implementation

		public bool IsCrashed { get; private set; }

		public float CurrentMileage {
			get { return transform.position.x; }
		}

		public void MoveForward()
		{
			transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, 0);
		}

		#endregion

		float lastJump;

		public void Jump()
		{
			if (Time.time - lastJump < 0.3f) {
				return;
			}
			if (jumpCount >= 2) {
				return;
			}
			jumpCount++;
			lastJump = Time.time;
			rigid.AddForce(new Vector3(0, jumpStrength, 0), ForceMode.Impulse);
		}

		public void Reset()
		{
			transform.position = new Vector3(0, 8, 0);
			transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
			rigid.angularVelocity = Vector3.zero;
			rigid.velocity = Vector3.zero;
			IsCrashed = false;
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
				IsCrashed = true;
			}
			jumpCount = 0;
			lastJump = 0;
		}

		void OnCollisionStay(Collision collision)
		{
			var eulerRotation = transform.rotation.eulerAngles;
			if (85 <= eulerRotation.x && eulerRotation.x <= 275) {
				Debug.Log("[GameManager] Game Over with over rotation");
				IsCrashed = true;
			}
		}

		void Update()
		{
			if (IsCrashed) {
				return;
			}

			if (transform.position.y < -3) {
				Debug.Log("[GameManager] Game Over with under position");
				IsCrashed = true;
				return;
			}
		}
	}
}