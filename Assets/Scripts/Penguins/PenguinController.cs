using System;
using System.Collections;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace SGJ
{
	public class PenguinController : MonoBehaviour
	{
		public static event Action<PenguinController> Spawned; 
		
		[SerializeField]
		PenguinStats Stats;
		[SerializeField]
		bool isEditorControllable = false;

		[SerializeField] private GameEvent PlayerDataChanged;

		public bool IsAffectingCracks
		{
			get => CurrentFatMeasure?.IsAffecting ?? false;
		}

		public int MaxStepsOnCrack
		{
			get => CurrentFatMeasure?.StepsToCrack ?? 0;
		}

		public int CurrentFatLevel
		{
			get => CurrentFatMeasure?.Level ?? 0;
		}
		
		public int MaxFatLevel
		{
			get
			{
				var lastFatMeasure = Stats.fatMeasures.LastOrDefault();
				return lastFatMeasure?.Level ?? 0;
			}
		}

		public float FatValueToReach
		{
			get
			{
				return CurrentFatMeasure?.MaxFatValue ?? 0;
			}
		}
		
		public float CurrentFatValue
		{
			get
			{
				return currentFatValue;
			}
			set
			{
				if(value == currentFatValue) return;
				currentFatValue = value;
				if(currentFatValue >= CurrentFatMeasure.MaxFatValue)
				{
					var nextFatMeasure = Stats.fatMeasures.FirstOrDefault(m => m.Level == CurrentFatMeasure.Level + 1);
					if(nextFatMeasure != null)
					{
						currentFatValue -= CurrentFatMeasure.MaxFatValue;
						CurrentFatMeasure = nextFatMeasure;
					}
				}
			}
		}
		
		float currentFatValue;
		
		public FatMeasure CurrentFatMeasure
		{
			get => currentFatMeasure;
			set
			{
				if(value == currentFatMeasure) return;
				currentFatMeasure = value;
				rb.mass = currentFatMeasure.Mass;
				Stats.movementSpeed -= Stats.movementSpeed * currentFatMeasure.DecelerationPercentage;
				OnFatMeasureChanged(currentFatMeasure);
			}
		}

		FatMeasure currentFatMeasure;

		Rigidbody rb;
		Vector2 lastInput;
		bool isOnCooldown;
		Vector3 initialScale;

		void Awake()
		{
			rb = GetComponent<Rigidbody>();
			OnSpawned();
		}

		void Start()
		{
			CurrentFatMeasure = Stats.fatMeasures.FirstOrDefault();
			CurrentFatValue = 0f;
			initialScale = transform.localScale;
		}

		public void HandleRewiredMovement(Vector2 input)
		{
			lastInput = input;
			
			var movementVector = Vector3.one;
			movementVector.x = lastInput.x * Stats.movementSpeed;
			movementVector.y = 0;
			movementVector.z = lastInput.y * Stats.movementSpeed;
			
			rb.AddForce(movementVector * Time.deltaTime, ForceMode.VelocityChange);
			transform.rotation = Quaternion.LookRotation(movementVector);
		}

		public void HandleSlide()
		{
			if(isOnCooldown)
				return;
			
			var slideVector = Vector3.zero;
			slideVector.x = lastInput.normalized.x;
			slideVector.z = lastInput.normalized.y;
			var force = CurrentFatMeasure.SlideForce * slideVector;
			Debug.Log($"SLIDING WITH FORCE {force.ToString()}");
			
			rb.AddForce(force, ForceMode.Force);

			StartCoroutine(SlideCooldown());
		}
		
		IEnumerator SlideCooldown()
		{
			isOnCooldown = true;
			yield return new WaitForSeconds(CurrentFatMeasure.SlideCooldown);
			isOnCooldown = false;
		}

		void OnCollisionEnter(Collision other)
		{
			if(other.collider.GetComponent(typeof(IEdible)) is IEdible edible)
			{
				edible.OnEaten(out var productFatValue);
				CurrentFatValue += productFatValue;
				PlayerDataChanged.Raise(gameObject);
			}
		}

		protected virtual void OnFatMeasureChanged(FatMeasure newMeasure)
		{
			rb.isKinematic = true;
			transform.DOScale(initialScale * newMeasure?.ScaleFactor ?? initialScale, 0.3f);
			transform.DOShakeScale(0.5f, 2f, 10, 0);
			rb.isKinematic = false;
			gameObject.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
		}

		protected virtual void OnSpawned()
		{
			Spawned?.Invoke(this);
		}
	}
}
