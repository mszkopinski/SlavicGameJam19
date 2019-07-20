using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Rewired;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SGJ
{
    [System.Serializable]
    public class MovementEvent : UnityEvent<Vector2>
    {
    }
    
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private int playerId;
        [SerializeField] private MovementEvent Movement;
        [FormerlySerializedAs("Shoot")] 
        [SerializeField] private UnityEvent Slide;
        [SerializeField] private UnityEvent Fire;
        [SerializeField] private UnityEvent Slap;

        [SerializeField] private GameEvent PlayerJoinedEvent; 
        [SerializeField] private GameEvent PlayerReadyEvent;
        [SerializeField] private GameEvent PlayerVoteRestartStart;
        [SerializeField] private GameEvent PlayerVoteRestartEnd;
        
        private Player player; 
        private Vector3 movementVector;
        private Color indicatorColor = new Color(1,1,1, 1);
        private Image[] indicators;

        private void Awake()
        {
            indicators = GetComponentsInChildren<Image>();
        }

        public Color IndicatorColor
        {
            get => indicatorColor;
            set
            {
                indicatorColor = value;
                foreach (var indicator in indicators)
                {
                    Color color = indicatorColor;
                    color.a = 0.9f;
                    indicator.color = color;
                }
            }
        }

        public int PlayerId
        {
            get { return playerId; }
            set
            {
                playerId = value;
                player = ReInput.players.GetPlayer(playerId);
                player.AddInputEventDelegate(OnFireButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Fire");
                player.AddInputEventDelegate(OnSlideButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Slide");
                player.AddInputEventDelegate(OnSlapButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Slap");
                player.AddInputEventDelegate(OnPlayerVoteRestartStart, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "VoteRestart");
                player.AddInputEventDelegate(OnPlayerVoteRestartEnd, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "VoteRestart");
                
                player.AddInputEventDelegate(OnReadyButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Ready");
                
                PlayerJoinedEvent.Raise(gameObject);
            }
        }

        void Update()
        {
            GetInput();
            ProcessMovementInput();
        }
        
        private void GetInput()
        {
            movementVector.x = player.GetAxis("Move Horizontal");
            movementVector.y = player.GetAxis("Move Vertical");
        }
        
        private void ProcessMovementInput()
        {
            if (movementVector.x != 0 || movementVector.y != 0)
            {
                Movement.Invoke(movementVector);
            }

        }

        private void OnFireButtonDown(InputActionEventData data)
        {
            Fire.Invoke();
        }

        private void OnSlideButtonDown(InputActionEventData data)
        {
            Slide.Invoke();
        }

        private void OnSlapButtonDown(InputActionEventData data)
        {
            Slap.Invoke();
        }

        private void OnReadyButtonDown(InputActionEventData data)
        {
            player.controllers.maps.SetMapsEnabled(false, "Ready");
            PlayerReadyEvent.Raise(gameObject);
        }

        private void OnPlayerVoteRestartStart(InputActionEventData data)
        {
            PlayerVoteRestartStart.Raise(gameObject);
        }

        private void OnPlayerVoteRestartEnd(InputActionEventData data)
        {
            PlayerVoteRestartEnd.Raise(gameObject);
        }

        private void OnDestroy()
        {
            player.RemoveInputEventDelegate(OnFireButtonDown);
            player.RemoveInputEventDelegate(OnSlideButtonDown);
            player.RemoveInputEventDelegate(OnReadyButtonDown);
            player.RemoveInputEventDelegate(OnPlayerVoteRestartStart);
            player.RemoveInputEventDelegate(OnPlayerVoteRestartEnd);
        }
    }
}

