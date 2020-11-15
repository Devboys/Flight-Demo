using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devboys.SharedObjects.Events
{
    [CreateAssetMenu(menuName = "SharedObjects/Game Event")]
    public class GameEvent : ScriptableObjectBase
    {
        private List<GameEventListener> listeners = new List<GameEventListener>();

        //boolean to detect a recursive call stack.
        private bool eventInProgress = false;

        /* Reset eventInProgress flag once this object is loaded. Since scriptable object state is
         * saved permanently in the assets database, we may end up in situations where the player
         * sets the eventInProgress flag and is immediately closed (before the event resolves).
         * This would leave the eventInProgress flag toggled on, locking this event forever. */
        public void OnEnable()
        {
            eventInProgress = false;
        }

        /// <summary>
        /// Triggers this event to raise the "OnEventRaised()" methods for any subscribed event 
        /// listeners.
        /// </summary>
        public void Raise()
        {
            //prevent recursive calls to this event
            if(eventInProgress == true) throw new System.Exception("Caught recursive call to event");

            eventInProgress = true;
            // loop backwards through list because listener list might change as a response to events 
            // (i.e. listeners removing themselves)
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised();
            }

            //reset event flag when all events have been called.
            eventInProgress = false;
        }

        //returns how many event listeners are currently subscribed to this event;
        public int GetNumSubscribers()
        {
            return listeners.Count;
        }

        /// <summary>
        /// Subscribes an event listener to this event. Subscribed event listeners will have their 
        /// "OnEventRaised()" method called whenever this event is raised.
        /// </summary>
        public void RegisterListener(GameEventListener listener)
        {
            listeners.Add(listener);
        }

        /// <summary>
        /// Unsubscribe an event listener from this event. This event will no longer trigger 
        /// "OnEventRaised()" for the given listener.
        /// </summary>
        public void UnregisterListener(GameEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}
