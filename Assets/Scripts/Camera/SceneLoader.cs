using UnityEngine;
using UnityEngine.SceneManagement; // Required namespace

namespace KingdomScratch
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private GameObject pauseScreen;
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
        public void LoadPreviousScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex - 1);
        }
        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }
        public void QuitGame()
        {
            Application.Quit();
        }
        public void EnterPauseScreen()
        {
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
        }
        
        public void ExitPauseScreen()
        {
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
        }

    }

}