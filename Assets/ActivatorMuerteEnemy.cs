using UnityEngine;

public class ActivatorMuerteEnemy : MonoBehaviour
{
    public GameObject panelEnemyMuerte;

    public Collider colliderDesactivado;

    

    private void Start()
    {
        panelEnemyMuerte.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        

        if (other.CompareTag("Player"))
        {
            panelEnemyMuerte.SetActive(true);
        }
   
    }

    private void OnTriggerExit(Collider other)
    {
        

        if (other.CompareTag("Player"))
        {
            panelEnemyMuerte.SetActive(false);
            colliderDesactivado.enabled = false;
        }
    }

    

    




}
