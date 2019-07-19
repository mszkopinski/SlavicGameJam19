using System;
using System.Linq;
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

		public int CurrentFatLevel
		{
			get
			{
				return CurrentFatMeasure.Level;
			}
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
				OnFatMeasureChanged(currentFatMeasure);
			}
		}

		FatMeasure currentFatMeasure;
		
		Rigidbody rb;

		void Awake()
		{
			rb = GetComponent<Rigidbody>();
			OnSpawned();
		}

		void Start()
		{
			CurrentFatMeasure = Stats.fatMeasures.FirstOrDefault();
			CurrentFatValue = 0f;
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
			movementVector.x = horizontal * Stats.movementSpeed;
			movementVector.z = vertical * Stats.movementSpeed;
			rb.AddForce(movementVector * Time.deltaTime, ForceMode.VelocityChange);
		}

		void OnTriggerEnter(Collider col)
		{
			if(col.GetComponent(typeof(IEdible)) is IEdible edible)
			{
				edible.OnEaten(out var productFatValue);
				CurrentFatValue += productFatValue;
			}
		}

		protected virtual void OnFatMeasureChanged(FatMeasure newMeasure)
		{
			
		}

		protected virtual void OnSpawned()
		{
			Spawned?.Invoke(this);
		}
	}
}
