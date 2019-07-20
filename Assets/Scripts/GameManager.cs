using System.Collections.Generic;
using UnityEngine;

namespace SGJ
{
	public class GameManager : MonoSingleton<GameManager>
	{
		[SerializeField] private GameEvent MatchStartedEvent;
		private List<GameObject> players = new List<GameObject>();

		void Start()
		{
			MatchStartedEvent.Raise(gameObject);
		}

		public void OnPlayerReady(object value)
		{
			Debug.Log("On player ready ");
			GameObject player = (GameObject) value;
			if (!players.Contains(player))
			{
				players.Add(player);
			}
		}

		public void OnPlayerDestroyed(object value)
		{
			Debug.Log("On player destroyed ");
			
			GameObject player = (GameObject) value;
			if (players.Contains(player))
			{
				players.Remove(player);
			}

			if (players.Count == 1)
			{
				Debug.Log($"Player { player.GetComponent<PlayerInput>().PlayerId} Wins");
			}
		}
	}
}
