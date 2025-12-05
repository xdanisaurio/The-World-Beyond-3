using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instancia;

    [Header("UI de carga (debe ser hijo del LoadingManager)")]
    public GameObject loadingScreen;
    public TextMeshProUGUI loadingText;

    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject); // Se mantiene en TODAS las escenas
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
            return;
        }

        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }

    public void CargarEscena(string nombre)
    {
        if (string.IsNullOrEmpty(nombre))
        {
            Debug.LogError("❌ Intentaste cargar una escena sin nombre.");
            return;
        }

        StartCoroutine(LoadAsync(nombre));
    }

    private IEnumerator LoadAsync(string nombre)
    {
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        if (loadingText != null)
            loadingText.text = "Cargando...";

        AsyncOperation op = SceneManager.LoadSceneAsync(nombre);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            if (op.progress >= 0.9f)
                op.allowSceneActivation = true;

            yield return null;
        }

        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }
}

