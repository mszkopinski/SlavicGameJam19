using UnityEngine;

namespace SGJ
{
	[RequireComponent(typeof(Collider))]
	public class LethalVolume : MonoBehaviour
	{
		void OnTriggerEnter(Collider col)
		{
			if(!col.IsEntity())
				return;
			var penguinController = col.GetComponent<PenguinController>();
			if(penguinController != null)
			{
				Debug.Log("Should destroy penguin");
			}
			else
			{
				Destroy(col.gameObject);
				Debug.Log("Should destroy fish");
			}
		}
	}
}