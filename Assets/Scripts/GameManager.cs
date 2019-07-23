using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SGJ
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private GameEvent MatchStartedEvent;
        [SerializeField] private GameEvent PlayerWinEvent;
        private List<GameObject> players = new List<GameObject>();

        void Start()
        {
            MatchStartedEvent.Raise(gameObject);
        }

        public void OnPlayerReady(object value)
        {
            GameObject player = (GameObject) value;
            if (!players.Contains(player))
            {
                players.Add(player);
            }
        }

        public void OnPlayerDestroyed(object value)
        {
            GameObject player = (GameObject) value;
            if (players.Contains(player))
            {
                players.Remove(player);
            }

            if (players.Count == 1)
            {
                PlayerWinEvent.Raise(players[0]);
                Invoke("RestartGame", 5f);
            }
        }

        private void RestartGame()
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}