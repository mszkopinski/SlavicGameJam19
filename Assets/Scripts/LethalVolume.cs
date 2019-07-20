using UnityEngine;

namespace SGJ
{
	[RequireComponent(typeof(Collider))]
	public class LethalVolume : MonoBehaviour
	{
		[SerializeField] private GameEvent PlayerFeltIntoWaterEvent;
		void OnTriggerEnter(Collider col)
		{
			if(!col.IsEntity())
				return;
			var penguinController = col.GetComponent<PenguinController>();
			if(penguinController != null)
			{
				Debug.Log("Should destroy penguin");
				PlayerFeltIntoWaterEvent.Raise(col.gameObject);
				Destroy(col.gameObject);
			}
			else
			{
				Destroy(col.gameObject);
				Debug.Log("Should destroy fish");
			}
		}
	}
}