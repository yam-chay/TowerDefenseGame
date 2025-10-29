using UnityEngine;
using UnityEngine.SceneManagement; // Required namespace

namespace KingdomScratch
{
    public class SceneLoader : MonoBehaviour
    {
        // Function to load a scene by its name
        public void LoadSceneByName()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Function to load a scene by its build index
        public void LoadSceneByIndex(int buildIndex)
        {
            SceneManager.LoadScene(buildIndex);
        }

        // Example of loading the next scene in the build settings
        public void LoadNextScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
}