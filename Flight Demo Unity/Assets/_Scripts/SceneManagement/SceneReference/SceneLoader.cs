using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneReferenceSystem.Sample
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private SceneReference sceneToLoad = null;

        private void Start()
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}