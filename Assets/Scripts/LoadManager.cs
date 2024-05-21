using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public static LoadManager Instance;
    [SerializeField] private GameObject loadingScreen;

    private void Start()
    {
        Instance = this;
        
        loadingScreen.SetActive(false);
    }

    public void LoadScene(int indexScene)
    {
        StartCoroutine(LoadSceneAsync(indexScene));
    }

    private IEnumerator LoadSceneAsync(int indexScene)
    {
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(indexScene);

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
