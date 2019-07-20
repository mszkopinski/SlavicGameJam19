using System.Collections.Generic;
using UnityEngine;

namespace SGJ
{
    [CreateAssetMenu(fileName = "new Game Event", menuName = "SGJ19/Game Event")]
    public class GameEvent : ScriptableObject
    {

        private List<GameEventListener> listeners = new List<GameEventListener>();

        public void Raise(object value = null) {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(value);
            }
        }

        public void RegisterListener(GameEventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(GameEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}

