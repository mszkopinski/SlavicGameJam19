using UnityEngine;

namespace SGJ
{
	public class PenguinController : MonoBehaviour
	{
		[SerializeField]
		PenguinStats Stats;
		[SerializeField]
		bool isEditorControllable = false;
		
		Rigidbody rb;

		void Awake()
		{
			rb = GetComponent<Rigidbody>();
		}
	
		void Update()
		{
			HandleMovement();
		}

		void HandleMovement()
		{
			if(!isEditorControllable)
				return;
			
			var horizontal = Input.GetAxis("Horizontal");
			var vertical = Input.GetAxis("Vertical");

			var movementVector = Vector3.one;
			movementVector.x = horizontal * Stats.MovementSpeed;
			movementVector.z = vertical * Stats.MovementSpeed;
			rb.MovePosition(transform.position + movementVector * Time.deltaTime); 
		}
	}
}