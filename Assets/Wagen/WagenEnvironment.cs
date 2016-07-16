using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wagen
{
	/// <summary>
	/// AIに渡す現在の環境情報
	/// </summary>
	public class WagenEnvironment
	{
		public readonly IWagenStage stage;
		public readonly IWagenCar car;

		public WagenEnvironment(IWagenStage stage, IWagenCar car)
		{
			this.stage = stage;
			this.car = car;
		}
	}

	public interface IWagenStage
	{
		List<IWagenPart> Parts { get; }
	}

	public interface IWagenPart
	{
		Vector3 Position { get; }
		float Width { get; }
		float Height { get; }
	}

	public interface IWagenCar
	{
		Vector3 Position { get; }
		Vector3 Rotation { get; }
		int AvailableJumpCount { get; }
	}
}

