using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SGJ
{
	public class CameraController : MonoBehaviour
	{
		public Camera ActiveCamera { get; private set; } 
		
		[SerializeField] Vector3 cameraOffset;
		[SerializeField] float smoothSpeed = 0.0001f;
		[SerializeField] Transform initialTarget;

		Vector3 refVelocity = Vector3.zero;
		float heightToAdd = 0f;
		
		readonly List<PenguinController> spawnedPenguins = new List<PenguinController>();
        
		void Awake()
		{    
			ActiveCamera = GetComponent<Camera>();
			spawnedPenguins.AddRange(FindObjectsOfType<PenguinController>());
		}

		void LateUpdate()
		{
			if(spawnedPenguins.Count == 0)
				return;
			
			bool allPenguinsVisible = spawnedPenguins.All(p => p.transform.IsVisibleToCam(ActiveCamera));
			heightToAdd = !allPenguinsVisible ? 5f : - 5f;

			var targetPos = GetPenguinPositionsCenter() + cameraOffset;
			targetPos.y += heightToAdd;
			var newPosition = Vector3.SmoothDamp(
				ActiveCamera.transform.position,
				targetPos,
				ref refVelocity,
				smoothSpeed);
			ActiveCamera.transform.position = newPosition;
		}

		Vector3 GetPenguinPositionsCenter()
		{
			var sum = Vector3.zero;
			if(spawnedPenguins.Count == 0)
				return sum;
			foreach(var spawnedPenguin in spawnedPenguins)
			{
				sum += spawnedPenguin.transform.position;
			}
			return sum / spawnedPenguins.Count;
		}
	}
}