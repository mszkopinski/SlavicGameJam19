using UnityEngine;

namespace SGJ
{
	[RequireComponent(typeof(Rigidbody))]
	public class FloatingObject : MonoBehaviour
	{
		[SerializeField]
		float waterLevel = 4f;
		[SerializeField]
		float floatHeight = 2f;
		[SerializeField]
		float bounceDeep = .05f;
		[SerializeField]
		Vector3 centreOffset = Vector3.zero;
		
		float forceFactor;
		Vector3 pointOfForce, upLiftForce;
		Rigidbody rb;

		void Awake()
		{
			rb = GetComponent<Rigidbody>();
		}

		void Update()
		{
			pointOfForce = transform.position + transform.TransformDirection(centreOffset);
			forceFactor = 1f - (pointOfForce.y - waterLevel) / floatHeight;
			if(forceFactor <= 0f) return;
			upLiftForce = rb.mass * (forceFactor - (rb.velocity.y * bounceDeep)) * -Physics.gravity;
			rb.AddForceAtPosition(upLiftForce, pointOfForce);
		}
	}
}