using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        // Hace que el texto siempre mire hacia la cámara
        transform.LookAt(transform.position + cam.forward);
    }
}