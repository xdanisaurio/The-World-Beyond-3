using UnityEngine;

public class DestroyProjectileByTag : MonoBehaviour
{
    public string projectileTag = "BossProjectile";
    public string effectTag = "ProjectileEffect";
    

    private void OnTriggerEnter(Collider other)
    {
        GameObject root = other.transform.root.gameObject;

        // Proyectil
        if (root.CompareTag(projectileTag))
        {
            Destroy(root);
            return;
        }

        // Efecto
        if (root.CompareTag(effectTag))
        {
            Destroy(root);
            return;
        }

        
    }
}
