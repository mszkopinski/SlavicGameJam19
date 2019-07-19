using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SGJ
{
	public class CameraController : MonoSingleton<CameraController>
	{
		public Camera ActiveCamera { get; private set; } 
		
		[SerializeField] float smoothSpeed = 0.0001f;
		[SerializeField] float screenEdgeSafeOffset = 4f;
		[SerializeField] float minCameraYPos = 8f;
		[SerializeField] Vector3 offset = Vector3.zero;

		Vector3 refVelocity = Vector3.zero;
		float currentVelocity;
		
		readonly List<Transform> objectsToTrack = new List<Transform>();
        
		void Awake()
		{    
			ActiveCamera = GetComponent<Camera>();
			objectsToTrack.AddRange(
				FindObjectsOfType<PenguinController>()
				.Select(obj => obj.transform));
		}

		void LateUpdate()
		{
			if(objectsToTrack.Count == 0)
				return;
			
			var targetPos = FindAveragePosition();
			targetPos += offset;
			transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref refVelocity, smoothSpeed);
			CalculateZoom(targetPos);
		}

		Vector3 FindAveragePosition()
	    {
	        var averagePos = Vector3.zero;
	        int targetsNum = 0;

	        foreach(var obj in objectsToTrack)
	        {
		        averagePos += obj.transform.position;
		        ++targetsNum;
	        }

	        if(targetsNum > 0)
	        {
		        averagePos /= targetsNum;
	        }

	        averagePos.y = transform.position.y;
		    return averagePos;
	    }

		void CalculateZoom(Vector3 targetPos)
		{
			float requiredSize = FindRequiredSize(targetPos);
			var newPos = transform.position;
			newPos.y = 90f / ActiveCamera.fieldOfView * requiredSize;
			newPos.y = Mathf.SmoothDamp(
				transform.position.y, 
				newPos.y, ref currentVelocity, smoothSpeed);
			transform.position = newPos;
		}
		
		float FindRequiredSize(Vector3 targetPosition)
	    {
	        var targetLocalPos = transform.InverseTransformPoint(targetPosition);
	        float size = 0f;
	        foreach(var obj in objectsToTrack)
	        {
		        var localPos = transform.InverseTransformPoint(obj.transform.position);
		        var desiredPosToTarget = localPos - targetLocalPos;
		        size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
		        size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / ActiveCamera.aspect);
	        }
	        size += screenEdgeSafeOffset;
	        size = Mathf.Max(size, minCameraYPos);
	        return size;
	    }
	}
}