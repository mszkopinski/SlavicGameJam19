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
	}
}