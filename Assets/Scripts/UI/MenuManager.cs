using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject panelMenuPrincipal;
    public GameObject panelOpciones;

    public void AbrirOpciones()
    {
        panelMenuPrincipal.SetActive(false);
        panelOpciones.SetActive(true);
    }

    public void VolverAlMenu()
    {
        panelOpciones.SetActive(false);
        panelMenuPrincipal.SetActive(true);
    }
}
