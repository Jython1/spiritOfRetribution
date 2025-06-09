using UnityEngine;
using UnityEngine.SceneManagement;
namespace SceneLoaderScript
{
    public class SceneLoader : MonoBehaviour
    {
        public string levelName;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && levelName != "")
            {
                SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
                Debug.Log($"Миссия {levelName} загружена");
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && levelName != "")
            {
                SceneManager.UnloadSceneAsync(levelName);
                Debug.Log($"Миссия {levelName} загружена");
            }
        }
    }

}