using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Boomerang : MonoBehaviour
{
    [SerializeField] private float maxLifeTime = 5f; // tiempo antes de autodestrucción
    private float lifeTimer;


    public float speed = 10f;
    public float returnSpeed = 15f;
    public float maxDistance = 7.5f;
    public float rotatioSpeed = 360f;
    public Action OnBoomerangReturn;

    private Transform pivotReference;
    private Vector3 targetPosition;//Posicion fija del jugador al lanzar el boomerang.
    private bool isReturning = false;


    public void Launch(Transform enemy, Transform player)
    {
        if (enemy == null || player == null)
        {
            Destroy(gameObject);
            return;
        }


        pivotReference = enemy;
        targetPosition = player.position;// Guarda la pocision del jugador al lanzar el boomerang.
        isReturning = false;
    }

    private void Update()
    {
        // Contador de vida del boomerang
        lifeTimer += Time.deltaTime;

        // Si pasa demasiado tiempo sin regresar destruir
        if (lifeTimer >= maxLifeTime)
        {
            Debug.Log("Boomerang destruido por tiempo límite");
            Destroy(gameObject);
            return;
        }

        if (!isReturning)
        {
            //Mover el boomerang hacia el jugador
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            transform.Rotate(Vector3.forward, rotatioSpeed * Time.deltaTime);

            if (pivotReference == null)return;
            
            
            // si el boomerang llega al jugador o supera la distancia maxima, regresa
            if (Vector3.Distance(transform.position,  pivotReference.position) >= maxDistance ||
                Vector3.Distance(transform.position, targetPosition) <0.5f)
            {
                isReturning =true;
            } 
        }
        else
        {
            //Mover el boomerang de vuelta al enemigo
            transform.position = Vector3.MoveTowards(transform.position, pivotReference.position, returnSpeed * Time.deltaTime);
            transform.Rotate(Vector3.forward, rotatioSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, pivotReference.position) < 0.5f)
            {
                OnBoomerangReturn?.Invoke();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        //Detectar la collicion con el jugador para regrasar
        if (!isReturning && other.CompareTag("Player"))
        {
            isReturning = true;
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
