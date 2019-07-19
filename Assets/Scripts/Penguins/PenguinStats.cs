using System;
using UnityEngine;

namespace SGJ
{
	[Serializable]
	public class PenguinStats
	{
		public float MovementSpeed => movementSpeed;
		
		[SerializeField]
		float movementSpeed;
	}
}