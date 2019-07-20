using UnityEngine;

namespace SGJ
{
	public class GameManager : MonoSingleton<GameManager>
	{
		[SerializeField] private GameEvent MatchStartedEvent;

		void Start()
		{
			MatchStartedEvent.Raise(gameObject);
		}
	}
}
