using UnityEngine;

namespace SGJ
{
	public class CameraController : MonoBehaviour
	{
		public Camera ActiveCamera { get; private set; } 
		
		[SerializeField] Vector3 cameraOffset;
		[SerializeField] float smoothSpeed = 0.0001f;
		[SerializeField] Transform initialTarget;

		Transform currentTarget;                                                                                           
		Vector3 refVelocity = Vector3.zero;
        
		void Awake()
		{    
			ActiveCamera = GetComponent<Camera>();
		}

		void Start()
		{
			currentTarget = initialTarget ? initialTarget : null;
		}

		void LateUpdate()
		{
			if(ReferenceEquals(currentTarget, null))
				return;

			var targetPos = currentTarget.position + cameraOffset;
			var newPosition = Vector3.SmoothDamp(
				ActiveCamera.transform.position,
				targetPos,
				ref refVelocity,
				smoothSpeed);
			ActiveCamera.transform.position = newPosition;
		}
	}
}