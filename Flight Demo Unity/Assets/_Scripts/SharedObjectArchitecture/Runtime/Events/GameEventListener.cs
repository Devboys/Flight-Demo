using UnityEngine;
using UnityEngine.Events;

namespace Devboys.SharedObjects.Events
{
    public class GameEventListener : MonoBehaviour
    {
        [Tooltip("The Game Event i am responding to / listening for")]
        public GameEvent Event; //which event am i listening to
        [Tooltip("The response to trigger when my event is called")]
        public UnityEvent Response;

        public void OnEventRaised()
        {
            Response.Invoke();
        }

        #region - Unity Message Callbacks -
        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }
        #endregion


    }
}