using UnityEngine;
using Wagen;
using System;

namespace BikeRun
{
	/// <summary>
	/// Wagenライブラリを用いてControllする
	/// </summary>
	public class WagenController : MonoBehaviour, IController
	{
		[SerializeField] Car car;
		[SerializeField] WagenProxy wagenProxy;

		public bool ShouldJump()
		{
			return wagenProxy.ShouldJump();
		}
	}
}

