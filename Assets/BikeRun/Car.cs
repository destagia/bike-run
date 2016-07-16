using UnityEngine;
using System.Collections;

namespace BikeRun
{
	public class Car : MonoBehaviour
	{
		[SerializeField] GameManager gameManager;

		[SerializeField] Rigidbody rigid;
		[SerializeField] float jumpStrength;
		[SerializeField] float speed;

		const float AngleMax = 45;

		int jumpCount;

		public void Jump()
		{
			if (jumpCount++ >= 2) {
				return;
			}
			rigid.AddForce(new Vector3(0, jumpStrength, 0), ForceMode.Impulse);
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