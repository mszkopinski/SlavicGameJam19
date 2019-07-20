using UnityEngine;

namespace SGJ
{
	[RequireComponent(typeof(Collider))]
	public class LethalVolume : MonoBehaviour
	{
		void OnTriggerEnter(Collider col)
		{
			var penguinController = col.GetComponent<PenguinController>();
			if(penguinController != null)
			{
				
			}
			Destroy(col.gameObject);
			Debug.Log("Should destroy collider");
		}
	}
}