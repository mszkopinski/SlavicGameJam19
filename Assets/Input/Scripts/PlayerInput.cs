using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Rewired;
using UnityEngine.Events;

namespace ShadowOfTheRoad
{
    [System.Serializable]
    public class MovementEvent : UnityEvent<Vector2>
    {
    }
    
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private int playerId;
        [SerializeField] private MovementEvent Movement;
        [SerializeField] private UnityEvent Shoot;
        [SerializeField] private UnityEvent Fire;
        
        private Player player; 
        private Vector3 movementVector;
        
        void Awake()
        {
            player = ReInput.players.GetPlayer(playerId);
            player.AddInputEventDelegate(OnFireButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Fire");
            player.AddInputEventDelegate(OnSlideButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Slide");
        }

        void Update()
        {
            GetInput();
            ProcessMovementInput();
        }
        
        private void GetInput()
        {
            movementVector.x = player.GetAxis("Move Horizontal");
            movementVector.z = player.GetAxis("Move Vertical");
        }
        
        private void ProcessMovementInput()
        {
            if (movementVector.x != 0 || movementVector.z != 0)
            {
                Movement.Invoke(movementVector);
            }

        }

        private void OnFireButtonDown(InputActionEventData data)
        {
            Shoot.Invoke();
        }

        private void OnSlideButtonDown(InputActionEventData data)
        {
            Fire.Invoke();
        }
    }
}

