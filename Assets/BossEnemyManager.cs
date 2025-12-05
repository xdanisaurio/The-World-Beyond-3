using UnityEngine;

public class BossEnemyManager : MonoBehaviour
{


    public static BossEnemyManager Instance;

    [Header("Llave que suelta el último enemigo")]
    public GameObject llavePrefab;

    private int enemigosVivos = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Contar enemigos puestos en la escena al iniciar
        enemigosVivos = FindObjectsOfType<BossHealth>().Length;
        Debug.Log("Enemigos vivos al inicio: " + enemigosVivos);
    }

    public void EnemigoMuerto(Vector3 posicion)
    {
        enemigosVivos--;
        Debug.Log("Enemigos vivos: " + enemigosVivos);

        if (enemigosVivos == 0)
        {
            Instantiate(llavePrefab, posicion, Quaternion.identity);
            Debug.Log("Último enemigo muerto → Se soltó la llave");
        }
    }
}
