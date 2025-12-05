using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset;
    public void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}