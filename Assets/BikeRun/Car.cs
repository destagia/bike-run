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
				gameManager.GameOver();
			}
			jumpCount = 0;
		}

		void Update()
		{
			if (transform.position.y < 0) {
				gameManager.GameOver();
			}

			transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, 0);
			var eulerRotation = transform.rotation.eulerAngles;
			transform.rotation = Quaternion.Euler(new Vector3(eulerRotation.x, 90, 0));
		}
	}
}