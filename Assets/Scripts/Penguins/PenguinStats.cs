using System;
using System.Collections.Generic;
using UnityEngine;

namespace SGJ
{
	[Serializable]
	public class PenguinStats
	{
		[SerializeField]
		public float movementSpeed;

		[SerializeField]
		public List<FatMeasure> fatMeasures;
	}

	[Serializable]
	public class FatMeasure
	{
		public int Level;
		public float MaxFatValue;
		
		[Header("Mass")]
		public float Mass = 20f;
		public float DecelerationPercentage = 0.1f;
		
		[Header("Slide")]
		public float SlideForce = 20f;
		public float SlideCooldown = 5f;
	}
}