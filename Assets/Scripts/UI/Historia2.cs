using UnityEngine;

public class Historia2 : MonoBehaviour
{
    [Tooltip("El panel especial aparece cuando toca el jugador este trigger")]
    public UIManager ui;



    private void Start()
    {

        if (ui == null)
            ui = FindObjectOfType<UIManager>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            ui.MostrarPanelTrigger2();

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            ui.OcultarPanelTrigger();
    }
}
