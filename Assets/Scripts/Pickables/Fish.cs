using UnityEngine;

namespace SGJ
{
	public class Fish : MonoBehaviour, IEdible
	{
		[SerializeField]
		float fatValue = 20f;

		public void OnEaten(out float fatValueOut)
		{
			fatValueOut = fatValue;
			Destroy(gameObject);
		}
	}
}