using UnityEngine;
using System.Collections;

namespace BikeRun
{
	/// <summary>
	/// ユーザーがキーボードで車をControllする
	/// </summary>
	public class InputController : MonoBehaviour, IController
	{
		public bool ShouldJump()
		{
			return Input.GetKeyDown(KeyCode.Space);
		}
	}
}