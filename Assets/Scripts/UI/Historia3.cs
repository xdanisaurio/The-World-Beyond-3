using UnityEngine;

public class Historia3 : MonoBehaviour
{
    [Tooltip("El panel especial aparece cuando toca el jugador este trigger")]
    public UIManager ui;

    [Header("Sonido al activar la nota")]
    public AudioClip sonidoNota;
    [Range(0f, 1f)] public float volumen = 1f;

    private AudioSource audioSource;

    private void Start()
    {
        if (ui == null)
            ui = FindObjectOfType<UIManager>();

        // Busca un AudioSource, si no existe lo crea
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Configuración segura
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
        audioSource.volume = volumen;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ui.MostrarPanelTrigger3();

            if (sonidoNota != null)
                audioSource.PlayOneShot(sonidoNota, volumen);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            ui.OcultarPanelTrigger();
    }
}

