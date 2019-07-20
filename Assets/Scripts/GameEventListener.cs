using UnityEngine;
using UnityEngine.Events;

namespace SGJ
{
    [System.Serializable]
    public class ChangeEvent : UnityEvent<object> { };

    public class GameEventListener : MonoBehaviour
    {
        [SerializeField]
        public GameEvent gameEvent;

        public GameEvent Event
        {
            set
            {
                gameEvent = value;
                gameEvent.RegisterListener(this);
            }
        }

        public ChangeEvent ObjectResponse = new ChangeEvent();

        private void OnEnable()
        {
            if (gameEvent)
            {
                gameEvent.RegisterListener(this);
            }
        }

        private void OnDisable()
        {
            if (gameEvent)
            {
                gameEvent.UnregisterListener(this);
            }

        }

        public void OnEventRaised(object value)
        {
            ObjectResponse.Invoke(value);
        }
    }
}

