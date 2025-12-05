using UnityEngine;

public class GameManagerScene : MonoBehaviour
{
    [SerializeField] private string nombreEscena;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Seguridad: ¿existe el LoadingManager?
            if (LoadingManager.Instancia != null)
            {
                LoadingManager.Instancia.CargarEscena(nombreEscena);
            }
            else
            {
                Debug.LogError("No existe un LoadingManager en la escena.");
            }
        }
    }
}

