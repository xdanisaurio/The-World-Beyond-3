using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    //public float shakeDuration = 0.1f;
    //public float shakeMagnitude = 0.2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Golpea " + other.name + "!");

            // vibración personalizada del ataque melee
            //if (CameraShake.instance != null)
                //CameraShake.instance.Shake(shakeDuration, shakeMagnitude);
        }
    }
}
