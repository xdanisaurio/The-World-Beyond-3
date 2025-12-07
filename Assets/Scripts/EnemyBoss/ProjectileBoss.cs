using UnityEngine;

public class ProjectileBoss : MonoBehaviour
{
    private Vector3 targetPosition;
    public float speed = 12f;
    public float destroyDistance = 0.5f;

    [Header("Impact Settings")]
    public GameObject impactEffect;
    public float shakeDuration = 1f;
    public float shakeMagnitude = 3f;

    public void SetTarget(Vector3 position)
    {
        targetPosition = position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) <= destroyDistance)
        {
            if (impactEffect != null)
            {
                var effect = Instantiate(impactEffect, targetPosition, Quaternion.identity);
                Destroy(effect, 2f);
            }

            // vibración personalizada del proyectil
            if (CameraShake.instance != null)
                CameraShake.instance.Shake(shakeDuration, shakeMagnitude);

            Destroy(gameObject);
        }
    }
}
