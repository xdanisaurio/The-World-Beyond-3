using UnityEngine;

public class TriggerFinalJuego : MonoBehaviour
{
    [Header("UiManager que controla los paneles")]
    public UIManager ui;

    private void Start()
    {
        if (ui == null)
            ui = FindObjectOfType<UIManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ui.MostrarFinalJuego();
        }
    }


}
