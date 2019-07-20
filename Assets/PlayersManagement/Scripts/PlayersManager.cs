using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rewired;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SGJ
{
    public class PlayersManager : MonoBehaviour
    {
        
        [Tooltip("Min players required to start the game")] 
        [SerializeField]
        private int minPlayers = 2;
        
        [SerializeField]
        private int maxPlayers = 8;

        [SerializeField] private GameObject PlayerPrefab;

        private List<PlayerMap> playerMap = new List<PlayerMap>();// Maps Rewired Player ids to game player ids
        private int gamePlayerIdCounter = 0;
        private List<int> restartVoters = new List<int>();
        //private List<int> playersReady = new List<int>();
        private readonly List<GameObject> playersReady = new List<GameObject>();

        [SerializeField]
        private UnityEvent PlayersReady;

        [SerializeField] private Transform[] SpawnPoints;

        private void Update()
        {
            Player player;
            
            for(int i = 0; i < ReInput.players.playerCount; i++)
            {
               player =  ReInput.players.GetPlayer(i);
                
                if(player.GetButtonDown("Join"))
                {
                    AssignNextPlayer(i);
                }
                
                /*else if (player.GetButtonDown("VoteRestart"))
                {
                    restartVoters.Add(player.id);
                    StartCoroutine(RemoveVote(player.id));
                }*/
            }

            /*if (playerMap.Count > 0 && restartVoters.Count == playerMap.Count)
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }*/
        }

        void AssignNextPlayer(int rewiredPlayerId) {
            if(playerMap.Count >= maxPlayers) {
                // Debug.LogError("Max player limit already reached!");
                return;
            }

            int gamePlayerId = GetNextGamePlayerId();
            var player = new PlayerMap(rewiredPlayerId, gamePlayerId);
            playerMap.Add(new PlayerMap(rewiredPlayerId, gamePlayerId));

            Player rewiredPlayer = ReInput.players.GetPlayer(rewiredPlayerId);

            rewiredPlayer.controllers.maps.SetMapsEnabled(false, "Joining");

            // Enable UI control for this Player now that he has joined
            rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Ready");

            PlayerInput playerInput = Instantiate(PlayerPrefab, SpawnPoints[gamePlayerId].position, Quaternion.identity).GetComponent<PlayerInput>();
            playerInput.PlayerId = rewiredPlayerId;
        }
        
        private int GetNextGamePlayerId() {
            return gamePlayerIdCounter++;
        }

        private IEnumerator RemoveVote(int playerId, float delay = 0.5f) 
        {
            yield return new WaitForSeconds(delay);
            restartVoters.Remove(playerId);
        }

        public void OnPlayerReady(object value)
        {
            PlayerInput playerInput = ((GameObject) value).GetComponent<PlayerInput>();
            playersReady.Add((GameObject)value);

            if (playersReady.Count >= minPlayers && playersReady.Count == gamePlayerIdCounter)
            {
                foreach (var player in playerMap)
                {
                    Player rewiredPlayer = ReInput.players.GetPlayer(player.rewiredPlayerId);
                    rewiredPlayer.controllers.maps.SetMapsEnabled(false, "Ready");
                    rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default");
                }
                
                PlayersReady.Invoke();
            }
        }

        // This class is used to map the Rewired Player Id to your game player id
        private class PlayerMap {
            public int rewiredPlayerId;
            public int gamePlayerId;

            public PlayerMap(int rewiredPlayerId, int gamePlayerId) {
                this.rewiredPlayerId = rewiredPlayerId;
                this.gamePlayerId = gamePlayerId;
            }
        }
        
        
    }
}
