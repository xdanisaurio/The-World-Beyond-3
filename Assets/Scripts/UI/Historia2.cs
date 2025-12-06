using UnityEngine;

public class Historia2 : MonoBehaviour
{
    [Tooltip("El panel especial aparece cuando toca el jugador este trigger")]
    public UIManager ui;

    [Tooltip("El clip de audio que sonará al recoger la nota")]
    public AudioClip sonidoNota;

    // Si quieres controlar el volumen desde aquí
    [Range(0f, 1f)] public float volumen = 1f;

    private AudioSource audioSource;

    private void Start()
    {
        if (ui == null)
            ui = FindObjectOfType<UIManager>();

        // Intenta obtener un AudioSource en este mismo GameObject.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Si no hay, lo añadimos (esto NO afecta otros AudioSources).
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Asegúrate de que no suene al comenzar
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f; // 0 = 2D (si quieres que se oiga igual sin importar la distancia)
        audioSource.volume = volumen;

        // No asignamos audioSource.clip para usar PlayOneShot (evita problemas de restart)
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador colisionó con la nota"); // para depurar
            ui.MostrarPanelTrigger2();

            if (sonidoNota != null)
            {
                // PlayOneShot es la forma segura para reproducir un clip sin interferir con otros sonidos.
                audioSource.PlayOneShot(sonidoNota, volumen);
            }
            else
            {
                Debug.LogWarning("Historia2: no hay AudioClip asignado en 'sonidoNota' en el Inspector.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            ui.OcultarPanelTrigger();
    }
}

