using DG.Tweening;
using UnityEngine;

namespace SGJ
{
	public class IceCrack : MonoBehaviour
	{
		[SerializeField]
		Vector3 minCrackScale;
		
		Vector3 initialScale;
		int currentPenguingSteps = 0;

		void Start()
		{
			initialScale = transform.localScale;
		}
		
		void OnCollisionEnter(Collision collision)
		{
			Debug.Log("COLLISION");
		}

		void OnTriggerEnter(Collider col)
		{
			var penguinController = col.GetComponent<PenguinController>();
			if(penguinController != null)
			{
				OnPenguinEntered(penguinController);
			}
		}

		void OnPenguinEntered(PenguinController penguin)
		{
			if(penguin.IsAffectingCracks == false)
				return;
			
			++currentPenguingSteps;
			var maxSteps = penguin.MaxStepsOnCrack;
			var stepsDifference = Mathf.Max(0, maxSteps - currentPenguingSteps);

			var value = 1f - (float)stepsDifference / maxSteps;
			var newScale = Vector3.Lerp(transform.localScale, minCrackScale, value);
			transform.DOScale(newScale, 0.3f);
			
			if(stepsDifference == 0)
			{
				Destroy(gameObject);
			}
		}
	}
}