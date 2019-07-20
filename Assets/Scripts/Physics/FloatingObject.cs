using System;
using System.Collections.Generic;
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
		
		float forceFactor;
		Vector3 pointOfForce, upLiftForce;
		Rigidbody rb;
		BoxCollider boxCollider;
		readonly List<MeshFilter> childMeshes = new List<MeshFilter>();

		void Awake()
		{
			rb = GetComponent<Rigidbody>();
			boxCollider = GetComponent<BoxCollider>();

			childMeshes.AddRange(GetComponentsInChildren<MeshFilter>());
		}

		void Start()
		{
			UpdateColliderSize();
		}

		void Update()
		{
			// pointOfForce = transform.position + transform.TransformDirection(Vector3.zero);
			pointOfForce = boxCollider.bounds.center;
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(pointOfForce, 20f);
			forceFactor = 1f - (pointOfForce.y - waterLevel) / floatHeight;
			if(forceFactor <= 0f) return;
			upLiftForce = rb.mass * (forceFactor - (rb.velocity.y * bounceDeep)) * -Physics.gravity;
			rb.AddForceAtPosition(upLiftForce, pointOfForce);
		}

		void UpdateColliderSize()
		{
			bool hasBounds = false;
			var newBounds = new Bounds(Vector3.zero, Vector3.zero);

			foreach(var meshFilter in childMeshes)
			{
				var renderer = meshFilter.GetComponent<Renderer>();
				if(renderer == null)
					continue;
				
				if (hasBounds) 
				{
					newBounds.Encapsulate(renderer.bounds);
				}
				else 
				{
					newBounds = renderer.bounds;
					hasBounds = true;
				}
			}
             
			boxCollider.center = newBounds.center - transform.position;
			boxCollider.size = newBounds.size;
		}
	}
}