using UnityEngine;

namespace SGJ
{
	public static class Extensions
	{
		public static bool IsEntity(this Collider collider)
		{
			return collider.CompareTag("Entity");
		}
		
		static readonly float MaxViewPosValue = .95f;
		static readonly float MinViewPosValue = 0.05f;
		
		public static bool IsVisibleToCam(this Transform transform, Camera targetCamera)
		{
			var viewPos = targetCamera.WorldToViewportPoint(transform.position);
			return viewPos.x >= MinViewPosValue && viewPos.x <= MaxViewPosValue && viewPos.y >= MinViewPosValue &&
			       viewPos.y <= MaxViewPosValue;
		}
	}
}