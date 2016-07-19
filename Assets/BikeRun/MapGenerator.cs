using UnityEngine;
using System.Collections.Generic;
using Wagen;

namespace BikeRun
{
	public class MapGenerator : MonoBehaviour, IWagenStage
	{
		[SerializeField] GameObject partPrefab;

		[SerializeField] float maxPartWidth;
		[SerializeField] float minPartWidth;
		[SerializeField] float maxPartHeight;
		[SerializeField] float minPartHeight;

		[SerializeField] float heightGap;

		[SerializeField] float length;

		[SerializeField] Car car;

		[SerializeField] float holeProbability;
		[SerializeField] float maxHoleWidth;
		[SerializeField] float minHoleWidth;

		/// <summary>
		/// 既に生成が終わっている長さ
		/// </summary>
		float generatedLength = 20;


		/// <summary>
		/// 前回のパーツの高さはある程度考慮しないと無理ゲー化する
		/// </summary>
		float prevPartHeight;

		Queue<MapPart> carParts = new Queue<MapPart>();

		#region IWagenStage implementation

		public List<IWagenPart> Parts {
			get {
				var parts = new List<IWagenPart>();
				foreach (var p in carParts) {
					parts.Add(p);
				}
				return parts;
			}
		}

		#endregion

		public void Reset()
		{
			foreach (var part in carParts) {
				Destroy(part.GameObject);
			}
			carParts.Clear();
			generatedLength = 20;
		}

		void Start()
		{
		}

		void Update()
		{
			if (carParts.Count > 20) {
				for (var i = 0; i < carParts.Count - 20; i++) {
					var obj = carParts.Dequeue();
					Destroy(obj.GameObject);
				}
			}

			if (generatedLength - car.transform.position.x < length) {
				var currentLength = generatedLength;
				while (generatedLength - currentLength < length) {
					var partOpt = CreatePartRandom(prevPartHeight);
					prevPartHeight = partOpt.Height;

					var part = Instantiate<GameObject>(partPrefab);
					carParts.Enqueue(new MapPart(partOpt, part));
					part.transform.position = new Vector3(partOpt.CenterPosition.x + generatedLength, 0, 0);
					part.transform.localScale = new Vector3(partOpt.Width, partOpt.Height, 10);
					generatedLength += partOpt.Width;

					var holeRandom = Random.value;
					if (holeRandom < holeProbability) {
						var holeWidth = Random.value * (maxHoleWidth - minHoleWidth) + minHoleWidth;
						generatedLength += holeWidth;
					}
				}
			}
		}

		MapPartOption CreatePartRandom(float prevHeight)
		{
			var part = new MapPartOption();
			part.Width = Random.value * (maxPartWidth - minPartWidth) + minPartWidth;
			part.Width = Mathf.Max(part.Width, minPartWidth);
			part.Width = Mathf.Min(part.Width, maxPartWidth);

			part.Height = prevHeight;
			while (Mathf.Abs(part.Height - prevHeight) < 2f) {
				part.Height = prevHeight + (Random.value * 0.5f + 0.5f) * (Random.value < 0.5f ? -1 : 1) * heightGap;
				if (part.Height >= maxPartHeight) {
					part.Height = maxPartHeight - (Random.value * 0.5f + 0.5f) * heightGap;
				} else if (part.Height <= minPartHeight) {
					part.Height = minPartHeight + (Random.value * 0.5f + 0.5f) * heightGap;
				}
			}
			return part;
		}
	}

	public class MapPart : IWagenPart
	{
		public Vector3 Position { get { return GameObject.transform.position; } }
		public float Width { get { return option.Width; } }
		public float Height { get { return option.Height; } }

		public GameObject GameObject { get; private set; }

		MapPartOption option;

		public MapPart(MapPartOption option, GameObject gameObject)
		{
			this.GameObject = gameObject;
			this.option = option;
		}
	}

	public class MapPartOption
	{
		public float Height { get; set; }
		public float Width  { get; set; }

		public Vector3 CenterPosition {
			get {
				return new Vector3(Width / 2, Height / 2, 0);
			}
		}
	}
}