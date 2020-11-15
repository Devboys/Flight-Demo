using UnityEngine;
using UnityEditor;
using Devboys.SharedObjects.Events;

namespace Devboys.SharedObjects.EditorObjects
{
    [CustomEditor(typeof(GameEvent))]
    public class GameEventInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GameEvent gameEvent = (GameEvent)target;

            //draws the regular inspector window.
            DrawDefaultInspector();

            GUILayout.Label("No. Subscribed Events (only works in playmode): \n" + gameEvent.GetNumSubscribers().ToString());
            GUILayout.Space(10); //10px space.
            if (GUILayout.Button("Raise Event"))
            {
                gameEvent.Raise();
            }
        }
    }
}
