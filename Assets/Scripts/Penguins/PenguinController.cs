using System;
using System.Collections;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SGJ
{
	public class PenguinController : MonoBehaviour
	{
		public static event Action<PenguinController> Spawned;

		public event Action<int> FatLevelChanged;
		public event Action<float> FatValueChanged;

		[SerializeField] private AudioSource audioSource;
		
		[SerializeField]
		PenguinStats Stats;
		[SerializeField]
		bool isEditorControllable = false;

		[SerializeField] private GameEvent PlayerDataChanged;
		[SerializeField] private GameEvent PlayerDestroyed;

		[SerializeField] private AudioClip[] EatSounds;
		[SerializeField] private AudioClip[] FatChangeSounds;

		private bool isGrounded = true;

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
				FatValueChanged?.Invoke(currentFatValue);
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
		Vector3 initialScale = Vector3.one;
		CinemachineImpulseSource impulseSource;
		SkinnedMeshRenderer skinnedMeshRenderer;
		Animator animator;
		static readonly int velocityAnimId = Animator.StringToHash("velocity");
		static readonly int slidingAnimId = Animator.StringToHash("isSliding");

		void Awake()
		{
			rb = GetComponent<Rigidbody>();
			impulseSource = GetComponent<CinemachineImpulseSource>();
			animator = GetComponentInChildren<Animator>();
			skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>(true);
			OnSpawned();
		}

		void Start()
		{
			CurrentFatMeasure = Stats.fatMeasures.FirstOrDefault();
			CurrentFatValue = 0f;
			FatValueChanged?.Invoke(CurrentFatValue);
		}

		public void HandleRewiredMovement(Vector2 input)
		{
			lastInput = input;
			
			animator.SetFloat(velocityAnimId, Mathf.Max(Mathf.Abs(lastInput.y), Mathf.Abs(lastInput.x)));
			
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
			
			animator.SetBool(slidingAnimId, true);
			
			var slideVector = Vector3.zero;
			slideVector.x = lastInput.normalized.x;
			slideVector.z = lastInput.normalized.y;
			var force = CurrentFatMeasure.SlideForce * slideVector;
			Debug.Log($"SLIDING WITH FORCE {force.ToString()}");
			
			rb.AddForce(force, ForceMode.Force);

			StartCoroutine(DoAfter(.5f, () => { animator.SetBool(slidingAnimId, false); }));
			StartCoroutine(SlideCooldown());
		}

		public void HandleJump()
		{
			if (isGrounded)
			{
				isGrounded = false;
				rb.AddForce(Vector3.up * rb.mass * 5f, ForceMode.Impulse);	
			}
		}

		IEnumerator DoAfter(float seconds, Action callback)
		{
			yield return new WaitForSeconds(seconds);
			callback?.Invoke();
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
				audioSource.PlayOneShot(EatSounds[Random.Range(0, EatSounds.Length)]);
				PlayerDataChanged.Raise(gameObject);
			}

			isGrounded = true;
		}

		protected virtual void OnFatMeasureChanged(FatMeasure newMeasure)
		{
			if(newMeasure == null)
				return;
			
			rb.constraints = (RigidbodyConstraints)84;
			Tweener tweener = transform.DOScale(initialScale * newMeasure.ScaleFactor, 0.7f);
			
			transform.DOShakeScale(0.5f, 2f, 10, 0);
			
			rb.constraints = (RigidbodyConstraints)80;
			impulseSource.GenerateImpulse();
			audioSource.PlayOneShot(FatChangeSounds[Random.Range(0, FatChangeSounds.Length)]);
			
			FatLevelChanged?.Invoke(newMeasure.Level);
		}

		public void OnPlayerWin(object value)
		{
			GameObject player = (GameObject) value;
			if (player.Equals(gameObject))
			{
				rb.constraints = RigidbodyConstraints.FreezeAll;
				GetComponentInChildren<Animator>().SetTrigger("IsWinner");
				GetComponent<PlayerInput>().enabled = false;
			}
		}

		protected virtual void OnSpawned()
		{
			Spawned?.Invoke(this);
		}

		private void OnDestroy()
		{
			PlayerDestroyed.Raise(gameObject);
		}
	}
}
