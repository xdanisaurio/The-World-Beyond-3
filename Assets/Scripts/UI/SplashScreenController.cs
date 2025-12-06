using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour
{
    [Header("Tiempo en pantalla del logo")]
    public float splashDuration = 3f;

    [Header("Nombre de la escena del menú")]
    public string menuSceneName = "MainMenu";

    private void Start()
    {
        StartCoroutine(LoadMenuAfterDelay());
    }

    private System.Collections.IEnumerator LoadMenuAfterDelay()
    {
        yield return new WaitForSeconds(splashDuration);

        SceneManager.LoadScene(menuSceneName);
    }
}
