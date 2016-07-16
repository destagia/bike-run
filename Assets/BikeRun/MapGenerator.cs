﻿using UnityEngine;
using System.Collections.Generic;

namespace BikeRun
{
	public class MapGenerator : MonoBehaviour
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
		float generatedLength;


		/// <summary>
		/// 前回のパーツの高さはある程度考慮しないと無理ゲー化する
		/// </summary>
		float prevPartHeight;

		Queue<GameObject> carParts = new Queue<GameObject>();

		void Start()
		{
			Random.seed = 1;
		}

		void Update()
		{
			if (carParts.Count > 20) {
				for (var i = 0; i < carParts.Count - 20; i++) {
					var obj = carParts.Dequeue();
					Destroy(obj);
				}
			}

			if (generatedLength - car.transform.position.x < length) {
				var currentLength = generatedLength;
				while (generatedLength - currentLength < length) {
					var partOpt = CreatePartRandom(prevPartHeight);
					prevPartHeight = partOpt.Height;

					var part = Instantiate<GameObject>(partPrefab);
					carParts.Enqueue(part);
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
			part.Height = prevHeight + Random.value * heightGap * 2 - heightGap;
			part.Height = Mathf.Max(part.Height, minPartHeight);
			part.Height = Mathf.Min(part.Height, maxPartWidth);
			return part;
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