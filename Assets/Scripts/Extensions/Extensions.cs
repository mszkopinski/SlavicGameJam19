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

	public static class UIExtensions
	{
		public static void SetSizeDelta(this RectTransform rectTransform, float? x = null, float? y = null)
		{
			var newSizeDelta = rectTransform.sizeDelta;
			newSizeDelta.x = x ?? newSizeDelta.x;
			newSizeDelta.y= y ?? newSizeDelta.y;
			rectTransform.sizeDelta = newSizeDelta;
		}
	}
}