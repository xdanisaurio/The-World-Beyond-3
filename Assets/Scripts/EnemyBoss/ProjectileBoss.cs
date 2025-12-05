using UnityEngine;

public class ProjectileBoss : MonoBehaviour
{
    private Vector3 targetPosition;
    public float speed = 12f;
    [SerializeField] public float destroyDistance = 0.5f;

    public GameObject inpactEffect;



    public void SetTarget(Vector3 position)
    {
        targetPosition = position;
    }


    
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) <= destroyDistance)
        {
            if (inpactEffect != null)
            {
               GameObject effect = Instantiate(inpactEffect, targetPosition, Quaternion.identity);
                Destroy(effect, 2f);
            }
            Destroy(gameObject);
        }
    }
}
