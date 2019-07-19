using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

namespace SGJ19
{
    public class PlayersManager : MonoBehaviour
    {
        [SerializeField]
        private int maxPlayers = 8;

        [SerializeField] private GameObject PlayerPrefab;

        private List<PlayerMap> playerMap = new List<PlayerMap>();// Maps Rewired Player ids to game player ids
        private int gamePlayerIdCounter = 0;
        private List<int> restartVoters = new List<int>();

        private void Update()
        {
            for(int i = 0; i < ReInput.players.playerCount; i++)
            {
                Player player = ReInput.players.GetPlayer(i);
                
                if(player.GetButtonDown("Join"))
                {
                    AssignNextPlayer(i);
                }
                
                else if (player.GetButtonDown("VoteRestart"))
                {
                    restartVoters.Add(player.id);
                    StartCoroutine(RemoveVote(player.id));
                }
            }

            if (playerMap.Count > 0 && restartVoters.Count == playerMap.Count)
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }

        private void OnDisable()
        {
            CancelInvoke();
        }

        void AssignNextPlayer(int rewiredPlayerId) {
            if(playerMap.Count >= maxPlayers) {
                // Debug.LogError("Max player limit already reached!");
                return;
            }

            int gamePlayerId = GetNextGamePlayerId();
            playerMap.Add(new PlayerMap(rewiredPlayerId, gamePlayerId));

            Player rewiredPlayer = ReInput.players.GetPlayer(rewiredPlayerId);

            rewiredPlayer.controllers.maps.SetMapsEnabled(false, "Assignment");

            // Enable UI control for this Player now that he has joined
            rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default");

            PlayerInput playerInput = Instantiate(PlayerPrefab).GetComponent<PlayerInput>();
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
