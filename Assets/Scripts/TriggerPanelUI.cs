using UnityEngine;

public class TriggerPanelUI : MonoBehaviour
{
    [Tooltip("El panel especial aparece cuando toca el jugador este trigger")]
    public UIManager ui;

    [Header("Sonido al activar el panel")]
    public AudioClip sonidoTrigger;
    [Range(0f, 1f)] public float volumen = 1f;

    private AudioSource audioSource;

    private void Start()
    {
        if (ui == null)
            ui = FindObjectOfType<UIManager>();

        // Busca o crea un AudioSource en este GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
        audioSource.volume = volumen;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ui.MostrarPanelTrigger();

            if (sonidoTrigger != null)
                audioSource.PlayOneShot(sonidoTrigger, volumen);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            ui.OcultarPanelTrigger();
    }
}
